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


namespace LoL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ViewModel _vm = new ViewModel();

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
            }
            catch (Exception )
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          /*  double d = Canvas.GetLeft(tbRecentChamps);
            Canvas.SetBottom(rect13, 2000);

            Rectangle exampleRectangle = new Rectangle();
            exampleRectangle.Width = 150;
            exampleRectangle.Height = 150;
            // Create a SolidColorBrush and use it to
            // paint the rectangle.
            SolidColorBrush myBrush = new SolidColorBrush(Colors.Green);
            exampleRectangle.Stroke = Brushes.Red;
            exampleRectangle.StrokeThickness = 4;
            exampleRectangle.Fill = myBrush;
            canvas.Children.Insert(0, exampleRectangle);*/


          /*  if (pnlValues.ActualHeight > 0)
            {
                canvas.Children.Clear();
                Rectangle exampleRectangle = new Rectangle();
                exampleRectangle.Width = pnlValues.ActualWidth-3;
                exampleRectangle.Height = pnlValues.ActualHeight - 293;
                SolidColorBrush myBrush = new SolidColorBrush(Color.FromRgb(0x1b, 0x1b, 0x1b));
                exampleRectangle.Fill = myBrush;
                canvas.Children.Insert(0, exampleRectangle);
            }*/
        }

       
    }
}
