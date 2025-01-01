using Microsoft.Windows.Themes;
using MvvmHelpers;
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
using System.Windows.Shapes;

namespace AppShopWFP
{
    /// <summary>
    /// Логика взаимодействия для ChangeUrlWindow.xaml
    /// </summary>
    public partial class ChangeUrlWindow : Window
    {
        public ChangeUrlWindow(object viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
