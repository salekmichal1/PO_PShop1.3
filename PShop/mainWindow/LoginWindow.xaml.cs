using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace PShop
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        public int count;

        public class Global
        {
            public static int? employeeId;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-9A79R96U;Initial Catalog=rtvDatabase;Integrated Security=True";

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                if(sqlConnection.State == ConnectionState.Closed)
                {
                    sqlConnection.Open();
                    string query = "SELECT id FROM Employees WHERE login=@login AND password=@password";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@login", txtUsername.Text);
                    sqlCommand.Parameters.AddWithValue("@password", txtPassword.Password);
                    count = Convert.ToInt32(sqlCommand.ExecuteScalar());
                    if(Convert.ToBoolean(count))
                    {
                        Global.employeeId = count;
                        MainWindow dashboard = new MainWindow();
                        dashboard.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Login lub hasło są nieprawidłowe");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
