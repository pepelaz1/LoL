﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using System.ComponentModel;
using System.Windows;

namespace LoL
{
    /// <summary>
    /// Displays a WinForms.WebBrowser control over a given placement target element in a WPF Window.
    /// Applies the opacity of the Window to the WebBrowser control.
    /// </summary>
    public class WebBrowserOverlayWF
    {
        System.Windows.Window _owner;
        FrameworkElement _placementTarget;
        Form _form; // the top-level window holding the WebBrowser control
        WebBrowser _wb = new WebBrowser();

        public WebBrowser WebBrowser { get { return _wb; } set { _wb = value; } }

        public WebBrowserOverlayWF(FrameworkElement placementTarget)
        {
            _placementTarget = placementTarget;
            Window owner = Window.GetWindow(placementTarget);
            Debug.Assert(owner != null);
            _owner = owner;

            _form = new Form();
            _form.Opacity = owner.Opacity;
            _form.ShowInTaskbar = false;
            _form.FormBorderStyle = FormBorderStyle.None;
            _wb.Dock = DockStyle.Fill;
            _form.Controls.Add(_wb);

            //owner.SizeChanged += delegate { OnSizeLocationChanged(); };
            owner.LocationChanged += delegate { OnSizeLocationChanged(); };
            _placementTarget.SizeChanged  += delegate { OnSizeLocationChanged(); };

            if (owner.IsVisible)
                InitialShow();
            else
                owner.SourceInitialized += delegate
                {
                     InitialShow();
                };

            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(UIElement.OpacityProperty, typeof(Window));
            dpd.AddValueChanged(owner, delegate { _form.Opacity = _owner.Opacity; });

            _form.FormClosing += delegate { _owner.Close(); };
        }

        void InitialShow()
        {
            NativeWindow owner = new NativeWindow();
            owner.AssignHandle(((HwndSource)HwndSource.FromVisual(_owner)).Handle);
            _form.Show(owner);
            owner.ReleaseHandle();
        }

        DispatcherOperation _repositionCallback;

        public void OnSizeLocationChanged()
        {
            // To reduce flicker when transparency is applied without DWM composition, 
            // do resizing at lower priority.
            if (_repositionCallback == null)
                _repositionCallback =  _owner.Dispatcher.BeginInvoke(Reposition, DispatcherPriority.Input);
        }

    
        void Reposition()
        {
            _repositionCallback = null;

            Point offset = _placementTarget.TranslatePoint(new Point(), _owner);
            Point size = new Point(_placementTarget.ActualWidth, _placementTarget.ActualHeight);
            HwndSource hwndSource = (HwndSource)HwndSource.FromVisual(_owner);
            CompositionTarget ct = hwndSource.CompositionTarget;
            offset = ct.TransformToDevice.Transform(offset);
            size = ct.TransformToDevice.Transform(size);

            Win32.POINT screenLocation = new Win32.POINT(offset);
            Win32.ClientToScreen(hwndSource.Handle, ref screenLocation);
            Win32.POINT screenSize = new Win32.POINT(size);

            Win32.MoveWindow(_form.Handle, screenLocation.X, screenLocation.Y, screenSize.X, screenSize.Y, true);
            //_form.SetBounds(screenLocation.X, screenLocation.Y, screenSize.X, screenSize.Y);
            //_form.Update();
        }

        internal void Dispose()
        {
            _placementTarget.SizeChanged -= delegate { OnSizeLocationChanged(); };
            _owner.LocationChanged -= delegate { OnSizeLocationChanged(); };
            _form.FormClosing -= delegate { _owner.Close(); };
            _wb.Dispose();
            _wb = null;
            _form.Dispose();
            _form = null;
        }
    };
}
