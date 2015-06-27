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

namespace RestaurentMngment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrderSelect_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuSelect_Click(object sender, RoutedEventArgs e)
        {
            BodyText.Text = "menu edit";
        }

        private void SellSelect_Click(object sender, RoutedEventArgs e)
        {
            BodyText.Text = "to see total sell";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BodyText.Text = "Do nothing";
        }
    }
}
