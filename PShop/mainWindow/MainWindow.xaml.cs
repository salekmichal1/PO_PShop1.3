using Microsoft.Data.SqlClient;
using PShop.Tables;
using PShop.Windows;
using System;
using System.Collections;
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
using static PShop.LoginWindow;
namespace PShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //public void downloadData(string query, DataGrid tableData)
        //{
        //    SqlConnection conection = new SqlConnection(@"Data Source=LAPTOP-9A79R96U;Initial Catalog=rtvDatabase;Integrated Security=True");
        //    conection.Open();
        //    SqlCommand command = new SqlCommand(query, conection);
        //    DataTable table = new DataTable();
        //    SqlDataAdapter adapter = new SqlDataAdapter(command);
        //    adapter.Fill(table);
        //    tableData.ItemsSource = table.DefaultView;
        //    command.Dispose();
        //    conection.Close();
        //}


        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            if (findOrder.Text != "")
            {
                int number;
                var orders = from Order in App.dbContext.Orders
                             join OrderedProduts in App.dbContext.OrderedProducts on Order.Id equals OrderedProduts.OrderId
                             join Products in App.dbContext.Products on OrderedProduts.ProductId equals Products.Id
                             join Customers in App.dbContext.Customers on Order.CustomerId equals Customers.Id
                             where Order.WhetherTheOrderFulfilled == false && Order.Id == (int.TryParse(findOrder.Text, out number) ? number : 0) || Order.WhetherTheOrderFulfilled == false && Customers.Surname == findOrder.Text
                             group new { Order, Customers, OrderedProduts, Products } by new { Order.Id, Customers.CustomerName, Customers.Surname } into gr
                             select new
                             {
                                 NumerZamówienia = gr.Key.Id,
                                 Imie = gr.Key.CustomerName,
                                 Nazwisko = gr.Key.Surname,
                                 Wartość = gr.Sum(x => x.OrderedProduts.Quantity * x.Products.NetSellingPrice)
                             };

                orderData.ItemsSource = orders.ToList();
                orderData.Items.Refresh();
                //string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '0' AND orders.id='{(int.TryParse(findOrder.Text, out int number) ? number : 0)}' OR customers.surname='{findOrder.Text}' GROUP BY orders.id, customers.customer_name, customers.surname";
                //downloadData(query, orderData);
            }
            else
            {
                var orders = from Order in App.dbContext.Orders
                             join OrderedProduts in App.dbContext.OrderedProducts on Order.Id equals OrderedProduts.OrderId
                             join Products in App.dbContext.Products on OrderedProduts.ProductId equals Products.Id
                             join Customers in App.dbContext.Customers on Order.CustomerId equals Customers.Id
                             where Order.WhetherTheOrderFulfilled == false
                             group new { Order, Customers, OrderedProduts, Products } by new { Order.Id, Customers.CustomerName, Customers.Surname } into gr
                             select new
                             {
                                 NumerZamówienia = gr.Key.Id,
                                 Imie = gr.Key.CustomerName,
                                 Nazwisko = gr.Key.Surname,
                                 Wartość = gr.Sum(x => x.OrderedProduts.Quantity * x.Products.NetSellingPrice)
                             };

                orderData.ItemsSource = orders.ToList();
                orderData.Items.Refresh();
                //string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '0' GROUP BY orders.id, customers.customer_name, customers.surname";
                //downloadData(query, orderData);
            }
            if (orderData.Items.Count == 0)
            {
                MessageBox.Show("Brak zamówienia lub zrealizowane");
            }

        }

        private void btnFindClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {   if (findClient.Text != "")
                {
                    //string query = $"SELECT customer_name AS Imie, surname AS Nazwisko, company_name AS Frima, company_number AS NIP, street AS Ulica, street_number AS [Numer domu], flat_number AS [Numer mieszkania], post_code AS [Kod pocztowy], city AS Miasto, phone_number AS [Numer telefonu], mail AS Mail  FROM dbo.Customers WHERE phone_number='{int.Parse(findClient.Text)}'";
                    //downloadData(query, clientData);
                    var customers = from Customer in App.dbContext.Customers
                                    where Customer.PhoneNumber == findClient.Text || Customer.CompanyNumber == int.Parse(findClient.Text)
                                    select new
                                    {
                                        Imie = Customer.CustomerName,
                                        Nazwisko = Customer.Surname,
                                        Frima = Customer.CompanyName,
                                        NIP = Customer.CompanyNumber,
                                        Ulica = Customer.Street,
                                        Numer_domu = Customer.StreetNumber,
                                        Numer_mieszkania = Customer.FlatNumber,
                                        Kod_pocztowy = Customer.PostCode,
                                        Miasto = Customer.City,
                                        Numer_telefonu = Customer.PhoneNumber,
                                        Mail = Customer.Mail
                                    };
                    clientData.ItemsSource = customers.ToList();
                    clientData.Items.Refresh();
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
            int number;
            if (findFulfilledOrder.Text != "")
            {
                //string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '1' AND orders.id='{(int.TryParse(findFulfilledOrder.Text, out int number) ? number : 0)}' OR customers.surname='{findFulfilledOrder.Text}' GROUP BY orders.id, customers.customer_name, customers.surname";
                //downloadData(query, fulfilledOrder);
                var fulfilledOrders = from Order in App.dbContext.Orders
                             join OrderedProduts in App.dbContext.OrderedProducts on Order.Id equals OrderedProduts.OrderId
                             join Products in App.dbContext.Products on OrderedProduts.ProductId equals Products.Id
                             join Customers in App.dbContext.Customers on Order.CustomerId equals Customers.Id
                             where Order.WhetherTheOrderFulfilled == true && Order.Id == (int.TryParse(findFulfilledOrder.Text, out number) ? number : 0) || Order.WhetherTheOrderFulfilled == true && Customers.Surname == findFulfilledOrder.Text
                             group new { Order, Customers, OrderedProduts, Products } by new { Order.Id, Customers.CustomerName, Customers.Surname } into gr
                             select new
                             {
                                 NumerZamówienia = gr.Key.Id,
                                 Imie = gr.Key.CustomerName,
                                 Nazwisko = gr.Key.Surname,
                                 Wartość = gr.Sum(x => x.OrderedProduts.Quantity * x.Products.NetSellingPrice)
                             };

                fulfilledOrder.ItemsSource = fulfilledOrders.ToList();
                fulfilledOrder.Items.Refresh();
            }
            else
            {
                string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '1' GROUP BY orders.id, customers.customer_name, customers.surname";
                //downloadData(query, fulfilledOrder);
                var fulfilledOrders = from Order in App.dbContext.Orders
                                      join OrderedProduts in App.dbContext.OrderedProducts on Order.Id equals OrderedProduts.OrderId
                                      join Products in App.dbContext.Products on OrderedProduts.ProductId equals Products.Id
                                      join Customers in App.dbContext.Customers on Order.CustomerId equals Customers.Id
                                      where Order.WhetherTheOrderFulfilled == true
                                      group new { Order, Customers, OrderedProduts, Products } by new { Order.Id, Customers.CustomerName, Customers.Surname } into gr
                                      select new
                                      {
                                          NumerZamówienia = gr.Key.Id,
                                          Imie = gr.Key.CustomerName,
                                          Nazwisko = gr.Key.Surname,
                                          Wartość = gr.Sum(x => x.OrderedProduts.Quantity * x.Products.NetSellingPrice)
                                      };
                fulfilledOrder.ItemsSource = fulfilledOrders.ToList();
                fulfilledOrder.Items.Refresh();
            }
            if (fulfilledOrder.Items.Count == 0)
            {
                MessageBox.Show("Brak zamówienia lub w realizacji");
            }

        }

        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();
            
            addCustomer.Show();
        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            NewOrder newOrder = new NewOrder();

            newOrder.Show();
        }

        public List<Customer> customers = new List<Customer>();

        private void newOrderFindClient1_GotFocus(object sender, RoutedEventArgs e)
        {
            //customers = (from customer in App.dbContext.Customers
            //             select new
            //             {
            //                 customer.Surname
            //             }).ToDictionary();

            var empnamesEnum = from Product in App.dbContext.Products
                               select $"{(Product.Id).ToString()} {Product.ProductName}";

            List<string> empnames = empnamesEnum.ToList();
            newOrderFindProduct.ItemsSource = empnames;

            //foreach (string cus in empnames)
            //{
            //    MessageBox.Show(cus);
            //}

        }

        private void btnNewOrderFindClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (newOrderFindClient.Text != "")
                {
                    var customers = from Customer in App.dbContext.Customers
                                    where Customer.PhoneNumber == newOrderFindClient.Text || Customer.CompanyNumber == int.Parse(newOrderFindClient.Text)
                                    select new
                                    {
                                        Imie = Customer.CustomerName,
                                        Nazwisko = Customer.Surname,
                                        Frima = Customer.CompanyName,
                                        NIP = Customer.CompanyNumber,
                                        Ulica = Customer.Street,
                                        Numer_domu = Customer.StreetNumber,
                                        Numer_mieszkania = Customer.FlatNumber,
                                        Kod_pocztowy = Customer.PostCode,
                                        Miasto = Customer.City,
                                        Numer_telefonu = Customer.PhoneNumber,
                                        Mail = Customer.Mail
                                    };
                    newOrderClientData.ItemsSource = customers.ToList();
                    newOrderClientData.Items.Refresh();
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

        private void btnNewOrderFindProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int number;
                if (newOrderFindProduct.Text != "")
                {
                    var produts = from Product in App.dbContext.Products
                                    where Product.ProductName == newOrderFindProduct.Text || Product.Id == (int.TryParse(newOrderFindProduct.Text, out number) ? number : 0)
                                  select new
                                    {
                                        SKU = Product.Id,
                                        Nazwa = Product.ProductName,
                                        Cena = Product.NetSellingPrice
                                    };
                    newOrderProductData.ItemsSource = produts.ToList();
                    newOrderProductData.Items.Refresh();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void newOrderClientData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null) return;

            MessageBox.Show("dupa"); 
        }

        private void newOrderProductData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                    e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null) return;

            IEnumerable<ItemsControl> enumerable = (IEnumerable<ItemsControl>)newOrderProductData.ItemsSource;
            List<ItemsControl> mylist = enumerable.ToList();

            MessageBox.Show((newOrderProductData.ItemsSource).GetType().ToString());
        }

        private void newOrderAddedProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
    
}
