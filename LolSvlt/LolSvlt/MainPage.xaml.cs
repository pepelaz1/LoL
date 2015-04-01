using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LolSvlt.Model;

namespace LolSvlt
{
    public partial class MainPage : UserControl
    {

        private ViewModel _vm = new ViewModel();
        public MainPage()
        {

            DataContext = _vm;

            InitializeComponent();

            btnSearch.Focus();

            try
            {
                ImageBrush myBrush = new ImageBrush();
               /* myBrush.ImageSource = new BitmapImage
                (new Uri("Images/HomeBackground.png"));*/

                myBrush.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString("Images/HomeBackground.png");
                gridMain.Background = myBrush;

                _vm.SelectedRegion = lbRegions.SelectedItem as Region;
            }
            catch (Exception)
            {

            }
        }


        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton b = e.OriginalSource as ToggleButton;
            if (b.IsChecked == true)
                lbRegions.Visibility = Visibility.Visible;
            else
                lbRegions.Visibility = Visibility.Collapsed;
        }

        private void tbSummonerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //_vm.SummonerName = tbSummonerName.Text;
                //QueryData();
                //e.Handled = true;
            }
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            HtmlDocument doc = HtmlPage.Document;
            HtmlElement el = doc.GetElementById("gameFrame");
            //el.SetAttribute("src", "http://www.lolnexus.com/NA/search?name=Brrando&region=NA");
           // QueryData();
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               // QueryData();
            }
        }

        private void lbRegions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbRegions.Visibility = Visibility.Collapsed;
            btnRegions.IsChecked = false;
            tbRegion.Text = (lbRegions.SelectedItem as Region).Name;
        }
    }
}
