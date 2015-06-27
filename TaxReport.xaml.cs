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
//using System.Windows.Forms;
using System.ComponentModel;

using System.Windows.Markup;


namespace RestaurentMngment
{
    /// <summary>
    /// Interaction logic for TexReport.xaml
    /// </summary>
    public partial class TaxReport : Window
    {

        String ttltext = null;
        String taxcrgtext = null;
        string conn = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        public TaxReport()
        {
            InitializeComponent();
            loadForm();
        }

        private void loadForm()
        {
            SqlConnection sqlConFill1 = new SqlConnection(conn);
            SqlCommand cmdFill1 = new SqlCommand();
            cmdFill1.CommandText = "SELECT tax FROM prime WHERE Id=" + '1';
            cmdFill1.Connection = sqlConFill1;
            sqlConFill1.Open();
            SqlDataReader rd = cmdFill1.ExecuteReader();
            String cat = "";
            if (rd.HasRows)
            {
                rd.Read();
                CurrentTax.Text = rd[0].ToString();
                cat = cat + rd[0].ToString();
                sqlConFill1.Close();
            }

        }


        private void loadForm1()
        {
            //Selecting date time and generate report

            if (date1 != null && date2 != null)
            {
                if (DateTime.Compare(date1.SelectedDate.Value.Date, date2.SelectedDate.Value.Date) > 0)
                {
                    MessageBox.Show("End Date must be greater then Start Date");
                }
                else
                {
                    String dt1 = date1.SelectedDate.Value.Date.ToShortDateString();
                    String dt2 = date2.SelectedDate.Value.Date.ToShortDateString();
                    SqlConnection sqlConFill = new SqlConnection(conn);
                    SqlCommand cmdFill = new SqlCommand();
                    cmdFill.CommandText = "SELECT * FROM ladger WHERE date BETWEEN @d1 AND @d2";
                    cmdFill.Parameters.AddWithValue("@d1", dt1);
                    cmdFill.Parameters.AddWithValue("@d2", dt2);
                    cmdFill.Connection = sqlConFill;
                    sqlConFill.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmdFill);
                    DataTable dt = new DataTable("Ladger");
                    sda.Fill(dt);
                    grdLadger.ItemsSource = dt.DefaultView;
                    sqlConFill.Close();

                    cmdFill.CommandText = "SELECT SUM(ttl) AS ttl1, SUM(taxcrg) AS taxcrg1 FROM ladger WHERE date BETWEEN @d3 AND @d4";
                    cmdFill.Parameters.AddWithValue("@d3", dt1);
                    cmdFill.Parameters.AddWithValue("@d4", dt2);
                    cmdFill.Connection = sqlConFill;
                    sqlConFill.Open();
                    SqlDataReader rd = cmdFill.ExecuteReader();
                    if (rd.HasRows)
                    {
                        rd.Read();
                        var outputParam = rd[0];
                        if (!(outputParam is DBNull))
                        {
                            
                            ttltext = Convert.ToString(rd[0]);
                            
                            taxcrgtext = Convert.ToString(rd[1]);
                        }
                        rd.Close();
                    }
                    sqlConFill.Close();
                    print.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Invalid Start or End Date");
            }

        }


        private void genarate_Click(object sender, RoutedEventArgs e)
        {
            loadForm1();
        }

        private void tax_button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(NewTax.Text))
            {
                SqlConnection sqlConFill = new SqlConnection(conn);
                SqlCommand cmdFill = new SqlCommand();
                cmdFill.CommandText = "UPDATE prime SET tax=@NewTax WHERE Id=" + '1';
                cmdFill.Parameters.AddWithValue("@NewTax", NewTax.Text);
                cmdFill.Connection = sqlConFill;
                sqlConFill.Open();
                cmdFill.ExecuteNonQuery();
                sqlConFill.Close();
                MessageBox.Show("Tax Rate Changed");
                CurrentTax.Text = NewTax.Text;
                NewTax.Clear();
            }
            else
            {
                MessageBox.Show("Invalid Tax Rate");
            }
        }

        private void password_button_Click(object sender, RoutedEventArgs e)
        {
            String current = CurrentPass.Password;
            String new_pass = NewPass.Password;
            if (!String.IsNullOrEmpty(current) && !String.IsNullOrEmpty(new_pass))
            {
                if (new_pass.Length < 5)
                {
                    MessageBox.Show("Minimum Password length is 5");
                }
                else
                {
                    SqlConnection sqlConFill = new SqlConnection(conn);
                    SqlCommand cmdFill = new SqlCommand();
                    cmdFill.CommandText = "SELECT tax_password FROM prime WHERE Id=" + '1';
                    cmdFill.Connection = sqlConFill;
                    sqlConFill.Open();
                    SqlDataReader rd = cmdFill.ExecuteReader();
                    String cat = "";
                    if (rd.HasRows)
                    {
                        rd.Read();
                        cat = rd[0].ToString();
                        sqlConFill.Close();
                    }
                    rd.Close();
                    sqlConFill.Close();
                    if (cat == current)
                    {
                        sqlConFill = new SqlConnection(conn);
                        cmdFill = new SqlCommand();
                        cmdFill.CommandText = "UPDATE prime SET tax_password=@new_pass WHERE Id=" + '1';
                        cmdFill.Parameters.AddWithValue("@new_pass", new_pass);
                        cmdFill.Connection = sqlConFill;
                        sqlConFill.Open();
                        cmdFill.ExecuteNonQuery();
                        sqlConFill.Close();

                        MessageBox.Show("Password Changed");
                        CurrentPass.Clear();
                        NewPass.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Password not Matched");
                    }
                }
            }
            else
            {
                MessageBox.Show("Current or New Password Field is Empty");
            }
        }

        private void logout_button_Click(object sender, RoutedEventArgs e)
        {
            MainMenu win2 = new MainMenu();
            win2.Show();
            this.Close();
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
            if (pd.ShowDialog() != true) return;

            // create a document
            FixedDocument document = new FixedDocument();
            document.DocumentPaginator.PageSize = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

            // create a page
            FixedPage page1 = new FixedPage();
            page1.Width = document.DocumentPaginator.PageSize.Width;
            page1.Height = document.DocumentPaginator.PageSize.Height;

            //adding header
            TextBlock page2Text = new TextBlock();
            page2Text.Text = "Summary";
            page2Text.FontSize = 20;
            page2Text.Margin = new Thickness(300, 0, 0, 0);
            page1.Children.Add(page2Text);

            //adding date
            TextBlock date11 = new TextBlock();
            date11.FontFamily = new FontFamily("Consolas");
            date11.Text = "From \t: " + date1.SelectedDate.Value.Date.ToShortDateString() + "\nTo \t: " + date2.SelectedDate.Value.Date.ToShortDateString();
            date11.FontSize = 15;
            date11.Margin = new Thickness(250, 22, 0, 0);
            page1.Children.Add(date11);

            //adding grid
            sp_tax.Children.Remove(grdLadger);
            grdLadger.Margin = new System.Windows.Thickness(0, 65, 0, 0);
            page1.Children.Add(grdLadger);


            //add extra footer data
            int m = Convert.ToInt32(grdLadger.ActualHeight);
            m = m + 80;
            String one = "Total Food Sell \t: " + ttltext + " Tk\nTotal Tax \t\t: " + taxcrgtext + " Tk\n";
            TextBlock footer = new TextBlock();
            footer.Text = one;
            footer.FontFamily = new FontFamily("Consolas");
            footer.FontSize = 12;
            footer.Margin = new Thickness(200, m, 0, 0);
            page1.Children.Add(footer);

            // add the page to the document
            PageContent page1Content = new PageContent();
            ((IAddChild)page1Content).AddChild(page1);
            document.Pages.Add(page1Content);
            pd.PrintDocument(document.DocumentPaginator, Title);
            page1.Children.Remove(grdLadger);
            grdLadger.Margin = new Thickness(0, 0, 0, 0);
            sp_tax.Children.Add(grdLadger);
        }

    }
}
