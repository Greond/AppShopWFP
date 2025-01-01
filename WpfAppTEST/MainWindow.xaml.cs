using AppShopWFP;
using AppShopWFP.Data;
using MvvmHelpers;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppShopWFP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ElementPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["DefaultConnectionString"].ConnectionString = HttpRequest.BaseUrl;
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChangeUrlWindow changeUrlWindow = new ChangeUrlWindow(this.DataContext);
            changeUrlWindow.Show();
        }
    }
}