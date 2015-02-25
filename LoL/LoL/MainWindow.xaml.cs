﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LoL.Model;
using mshtml;


namespace LoL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ViewModel _vm = new ViewModel();
        private WebBrowserOverlayWF _wbo;

        public MainWindow()
        {
            DataContext = _vm;

            InitializeComponent();

            btnSearch.Focus();

            try
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage
                (new Uri(@"pack://application:,,,/LoL;component/HomeBackground.png"));
                gridMain.Background = myBrush;

                btnMinimize.Content = (char)0x25A1;
                _vm.SelectedRegion = lbRegions.SelectedItem as Region;

                QueryBanner();
            }
            catch (Exception)
            {

            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            QueryData();
        }

        private async void QueryData()
        {
            Cursor = Cursors.Wait;
            try
            {
                await _vm.QueryData();
                tcMain.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(_vm.LastError);
            }
            Cursor = Cursors.Arrow;
        }


        private void tbSummonerName_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (_vm.SummonerName == "Summoner Name...")
                _vm.SummonerName = "";
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryData();
            }
        }
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Form_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Form_Min(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Form_max_restore(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton b = e.Source as ToggleButton;
            if (b.IsChecked == true)
                lbRegions.Visibility = Visibility.Visible;
            else
                lbRegions.Visibility = Visibility.Collapsed;
        }

        private void lbRegions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbRegions.Visibility = Visibility.Collapsed;
            btnRegions.IsChecked = false;
            tbRegion.Text = (lbRegions.SelectedItem as Region).Name;
        }



        private void tbSummonerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _vm.SummonerName = tbSummonerName.Text;
                QueryData();
                e.Handled = true;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _wbo.OnSizeLocationChanged();
        }

        WebBrowser wb1 = new WebBrowser();
         private const string DisableScriptError = @"function noError() { return true; } window.onerror = noError;";
        private async void QueryBanner()
        {

            try
            {
                  await _vm.QueryBanner();
                _wbo = new WebBrowserOverlayWF(_webBrowserPlacementTarget);
                System.Windows.Forms.WebBrowser wb = _wbo.WebBrowser;
                wb.ScriptErrorsSuppressed = true;
                wb.Navigate(new Uri("http://pagead2.googlesyndication.com/pagead/imgad?id=CICAgKDju5bCsAEQ2AUYWjIIw8F3bkbGHTA"));

            }
            catch (Exception ex)
            {

            }
        }


        void wb_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            InjectDisableErrorScript();
        }

        private void InjectDisableErrorScript()
        {
            var doc = wb1.Document as HTMLDocument;
            if (doc != null)
            {
                //Create the sctipt element
                var scriptErrorSuppressed = (IHTMLScriptElement)doc.createElement("SCRIPT");
                scriptErrorSuppressed.type = "text/javascript";
                scriptErrorSuppressed.text = DisableScriptError;
                //Inject it to the head of the page
                IHTMLElementCollection nodes = doc.getElementsByTagName("head");
                foreach (IHTMLElement elem in nodes)
                {
                    var head = (HTMLHeadElement)elem;
                    head.appendChild((IHTMLDOMNode)scriptErrorSuppressed);
                }
            }

        }
        private void wb_LoadCompleted(object sender, NavigationEventArgs e)
        {
            
            int t = 4;
            t = 4;


         /*   try
            {
                HTMLDocument doc = wb1.Document as HTMLDocument;
               // wb1.InvokeScript("adsbygoogle");
                foreach (var s in doc.scripts)
                {
                    HTMLScriptElement se = s as HTMLScriptElement;
                    //wb1.InvokeScript(se.src);
                    int tt = 4;
                }

                int t = 4;
                t = 4;
            }
            catch(Exception ex)
            { }*/

        }

    }

}
