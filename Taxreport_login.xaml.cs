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

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Sql;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace RestaurentMngment
{
    /// <summary>
    /// Interaction logic for texreport_login.xaml
    /// </summary>
    public partial class Taxreport_login : Window
    {

        string conn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        public Taxreport_login()
        {
            InitializeComponent();
        }

        private void logintex_Click(object sender, RoutedEventArgs e)
        {
            String tex_pass = passtex.Password;
            //String tex_cat = "12346";



            SqlConnection sqlConFill1 = new SqlConnection(conn);
            SqlCommand cmdFill = new SqlCommand();
            cmdFill.CommandText = "SELECT tax_password FROM prime WHERE Id = " + '1';
            cmdFill.Connection = sqlConFill1;
            sqlConFill1.Open();
            SqlDataReader rd = cmdFill.ExecuteReader();
            String tex_cat = "";
            if (rd.HasRows)
            {
                rd.Read();
                tex_cat = tex_cat + rd[0].ToString();
                rd.Close();
            }
            sqlConFill1.Close();



            if (tex_cat == tex_pass)
            {
                TaxReport wintex = new TaxReport();
                wintex.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Password", " ", MessageBoxButton.OK, MessageBoxImage.Error);
                passtex.Clear();
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MainMenu win2 = new MainMenu();
            win2.Show();
            this.Close();
        }

    }
}
