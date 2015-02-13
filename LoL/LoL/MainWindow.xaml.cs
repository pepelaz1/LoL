using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


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
                MessageBox.Show(ex.Message);
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

       
       
    }
}
