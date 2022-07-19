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
using static PShop.Windows.NewOrder;

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
        public static class GlobalsMainWindow
        {
           static public List<int> selectedProductId = new List<int>();
           static public List<string> productQunatity = new List<string>();
           static public int max { get; set; }

           public static DataGridCellInfo cellInfoOrders { get; set; }
           public static string selectedOrders { get; set; }
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
                MessageBox.Show("Brak zamówienia, zrealizowane lub bez produtków");
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

        private void btnNewOrderFindClient_Click(object sender, RoutedEventArgs e)
        {
            GlobalsMainWindow.max = (from Order in App.dbContext.Orders
                where
                  Order.WhetherTheOrderFulfilled == false
                orderby
                  Order.Id descending
                select Order.Id).FirstOrDefault();

            try
            {
                
                if (newOrderFindClient.Text != "")
                {
                    var customers = from Customer in App.dbContext.Customers
                                    where Customer.PhoneNumber == newOrderFindClient.Text || Customer.CompanyNumber == int.Parse(newOrderFindClient.Text)
                                    select new
                                    {
                                        Numer = Customer.Id,
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
                    newOrderClientData.SelectAll();
                    orderId.Text = GlobalsMainWindow.max.ToString();
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

        
        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var cellInfo = newOrderClientData.SelectedCells[0];
            var selectedClientID = (cellInfo.Column.GetCellContent(cellInfo.Item) as TextBlock).Text;
            try
            {
                App.dbContext.Orders.Add(new Order
                {
                    CustomerId = int.Parse(selectedClientID),
                    DateOfPlacingTheOrder = DateTime.Now,
                    OrderRealizationDate = null,
                    WhetherTheOrderFulfilled = false,
                    ShippingDate = null,
                    EmployeeId = 1,
                    InvoiceId = 5,
                   
                });
                App.dbContext.SaveChangesAsync();
                GlobalsMainWindow.max += 1;
                orderId.Text = GlobalsMainWindow.max.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
            finally
            {
                //App.dbContext.DisposeAsync();
            }
        }

        private void btnProductAdd_Click(object sender, RoutedEventArgs e)
        {
            NewOrder newOrder = new NewOrder();

            newOrder.Show();
        }

        private void orderData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var selectedOrderData = from Customer in App.dbContext.Customers
                                    join Order in App.dbContext.Orders on Customer.Id equals Order.CustomerId
                                    where Order.Id == Convert.ToInt32(GlobalsMainWindow.selectedOrders)
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

            var createdBy = from Order in App.dbContext.Orders
                            join Employees in App.dbContext.Employees on Order.EmployeeId equals Employees.Id
                            where Order.Id == Convert.ToInt32(GlobalsMainWindow.selectedOrders)
                            select $"{Employees.EmployeeName} {Employees.EmployeeSurname}";

            var orderValue = from OrderedProduct in App.dbContext.OrderedProducts
                             join Product in App.dbContext.Products on OrderedProduct.ProductId equals Product.Id
                             where OrderedProduct.OrderId == Convert.ToInt32(GlobalsMainWindow.selectedOrders)
                             group new { OrderedProduct, Product } by new { OrderedProduct.Quantity, Product.NetSellingPrice } into gr
                             select gr.Sum(x => x.OrderedProduct.Quantity * x.Product.NetSellingPrice);

            var productsList = from Product in App.dbContext.Products
                               join OrderedProduct in App.dbContext.OrderedProducts on Product.Id equals OrderedProduct.ProductId
                               where OrderedProduct.OrderId == Convert.ToInt32(GlobalsMainWindow.selectedOrders)
                               select new
                               {
                                   SKU = Product.Id,
                                   Nazwa = Product.ProductName,
                                   Cena = Product.NetSellingPrice,
                                   Ilość = OrderedProduct.Quantity
                               };

            GlobalsMainWindow.cellInfoOrders = orderData.SelectedCells[0];
            GlobalsMainWindow.selectedOrders = (GlobalsMainWindow.cellInfoOrders.Column.GetCellContent(GlobalsMainWindow.cellInfoOrders.Item) as TextBlock).Text;
           
            SellWindow sellWindow = new SellWindow();
            sellWindow.selectedOrderId.Text = GlobalsMainWindow.selectedOrders.ToString();
            sellWindow.orderTotalValue.Text = $"Wartść zamówienia: {orderValue.Sum()}";
            sellWindow.selectedOrderDataSellWindow.ItemsSource = selectedOrderData.ToList();
            sellWindow.selectedOrderDataSellWindow.Items.Refresh();
            sellWindow.selectedProductsSellWindow.ItemsSource = productsList.ToList();
            sellWindow.selectedProductsSellWindow.Items.Refresh();
            sellWindow.employeesData.Text = $"Stworzone przez: {createdBy.FirstOrDefault()}";
            sellWindow.Show();
        }
    }
    
}
