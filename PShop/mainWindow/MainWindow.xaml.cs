using Microsoft.Data.SqlClient;
using PShop.Tables;
using PShop.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            orderData.ItemsSource = order;
            orderData.Items.Refresh();
        }

        public List<Order> order = App.dbContext.Order.ToList();


        public void downloadData(string query, DataGrid tableData)
        {
            SqlConnection conection = new SqlConnection(@"Data Source=LAPTOP-9A79R96U;Initial Catalog=rtvDatabase;Integrated Security=True");
            conection.Open();
            SqlCommand command = new SqlCommand(query, conection);
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            tableData.ItemsSource = table.DefaultView;
            command.Dispose();
            conection.Close();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            //if(findOrder.Text != "")
            //{
            //    string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '0' AND orders.id='{(int.TryParse(findOrder.Text, out int number) ? number : 0)}' OR customers.surname='{findOrder.Text}' GROUP BY orders.id, customers.customer_name, customers.surname";
            //    downloadData(query, orderData);
            //}
            //else
            //{
            //    string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '0' GROUP BY orders.id, customers.customer_name, customers.surname";
            //    downloadData(query, orderData);
            //}
            //if (orderData.Items.Count == 0)
            //{
            //    MessageBox.Show("Brak zamówienia lub zrealizowane");
            //}

            orderData.ItemsSource = App.dbContext.Order.ToList();
            orderData.Items.Refresh();
        }

        private void btnFindClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {   if (findClient.Text != "")
                {
                    string query = $"SELECT customer_name AS Imie, surname AS Nazwisko, company_name AS Frima, company_number AS NIP, street AS Ulica, street_number AS [Numer domu], flat_number AS [Numer mieszkania], post_code AS [Kod pocztowy], city AS Miasto, phone_number AS [Numer telefonu], mail AS Mail  FROM dbo.Customers WHERE phone_number='{int.Parse(findClient.Text)}'";
                    downloadData(query, clientData);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnbtnFindFulfilledOrder_Click(object sender, RoutedEventArgs e)
        {
            if (findFulfilledOrder.Text != "")
            {
                string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '1' AND orders.id='{(int.TryParse(findFulfilledOrder.Text, out int number) ? number : 0)}' OR customers.surname='{findFulfilledOrder.Text}' GROUP BY orders.id, customers.customer_name, customers.surname";
                downloadData(query, fulfilledOrder);
            }
            else
            {
                string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '1' GROUP BY orders.id, customers.customer_name, customers.surname";
                downloadData(query, fulfilledOrder);
            }

        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();

            addCustomer.Show();
        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
