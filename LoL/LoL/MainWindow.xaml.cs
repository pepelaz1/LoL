using System;
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
            if (_wbo != null)
                _wbo.OnSizeLocationChanged();
        }

        WebBrowser wb1 = new WebBrowser();


         private const string DisableScriptError = @"function noError() { return true; } window.onerror = noError;";
        private async void QueryBanner()
        {

            try
            {
               //  await _vm.QueryBanner();
                _wbo = new WebBrowserOverlayWF(_webBrowserPlacementTarget);
                _wbo.WebBrowser.Visible = false;
                System.Windows.Forms.WebBrowser wb = _wbo.WebBrowser;
                wb.DocumentCompleted +=  new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(DocumentReady);
                wb.NewWindow += WebBrowser1_NewWindow;
                wb.ScriptErrorsSuppressed = true;
                //  wb.Navigate(new Uri("http://pagead2.googlesyndication.com/pagead/imgad?id=CICAgKDju5bCsAEQ2AUYWjIIw8F3bkbGHTA"));
                wb.ScrollBarsEnabled = false;
                wb.Navigate(new Uri("http://firekickz.com/"));

            }
            catch (Exception ex)
            {

            }
        }

        private void WebBrowser1_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
         //   e.Cancel = true;
         //  _wbo.WebBrowser.Navigate(_wbo.WebBrowser.StatusText, true);

            //foreach (System.Windows.Forms.HtmlElement tag in _wbo.WebBrowser.Document.All)
            //{
            //    switch (tag.TagName.ToUpper())
            //    {
            //        case "A":
            //            {
            //                tag.MouseUp += new System.Windows.Forms.HtmlElementEventHandler(link_MouseUp);
            //                break;
            //            }
            //    }
            //}
        }
        

        private void DocumentReady(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            // Print the document now that it is fully loaded.
            /*((WebBrowser)sender).Print();

            // Dispose the WebBrowser now that the task is complete. 
            ((WebBrowser)sender).Dispose();*/
            if (e.Url != _wbo.WebBrowser.Url)
                return;

            _wbo.WebBrowser.Visible = true;
            _wbo.WebBrowser.Document.Window.ScrollTo(0, 171);
            _webBrowserPlacementTarget.Visibility = Visibility.Visible;

            foreach (System.Windows.Forms.HtmlElement tag in _wbo.WebBrowser.Document.All)
            {
                switch (tag.TagName.ToUpper())
                {
                    case "A":
                        {
                            tag.MouseUp += new System.Windows.Forms.HtmlElementEventHandler(link_MouseUp);
                            break;
                        }
                }
            }

           // _wbo.WebBrowser.Navigating += webBrowser1_Navigating;
        }

       /* private void webBrowser1_Navigating(object sender, System.Windows.Forms.WebBrowserNavigatingEventArgs e)
        {
            // Redirect expertsexchange to stackoverflow
          //  if (e.Url.ToString().Contains("experts-exchange"))
          //  {
            //    e.Cancel = true;
            //    _wbo.WebBrowser.Navigate("http://stackoverflow.com");
           // }
            e.Cancel = true;
            _wbo.WebBrowser.Navigate(_wbo.WebBrowser.StatusText, true);
        }*/


        void link_MouseUp(object sender, System.Windows.Forms.HtmlElementEventArgs e)
        {
            System.Windows.Forms.HtmlElement link = (System.Windows.Forms.HtmlElement)sender;
            HTMLAnchorElementClass a = (HTMLAnchorElementClass)link.DomElement;
            switch (e.MouseButtonsPressed)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    {
                        _wbo.WebBrowser.Navigate(_wbo.WebBrowser.StatusText, true);
                        /*if ((a.target != null && a.target.ToLower() == "_blank") ||
                            e.ShiftKeyPressed ||
                            e.MouseButtonsPressed == System.Windows.Forms.MouseButtons.Middle)
                        {
                            // add new tab
                        }
                        else
                        {
                            // open in current tab
                        }*/
                        break;
                    }
                case System.Windows.Forms.MouseButtons.Right:
                    {
                        // show context menu
                        break;
                    }
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
