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
        /// <summary>
        /// global varibles for mainwindow
        /// </summary>
        public static class GlobalsMainWindow
        {
            static public int max { get; set; }

            public static DataGridCellInfo cellInfoOrders { get; set; }
            public static string selectedOrders { get; set; }
        }
        /// <summary>
        /// event handler for searching new orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
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
                                 Wartość = @String.Format("{0:C}", gr.Sum(x => x.OrderedProduts.Quantity * x.Products.NetSellingPrice))
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

        /// <summary>
        /// event handler for searching clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (findClient.Text != "")
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

        /// <summary>
        /// event handler for searching closed orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnbtnFindFulfilledOrder_Click(object sender, RoutedEventArgs e)
        {
            int number;
            if (findFulfilledOrder.Text != "")
            {
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
                                          Wartość = @String.Format("{0:C}", gr.Sum(x => x.OrderedProduts.Quantity * x.Products.NetSellingPrice))
                                      };

                fulfilledOrder.ItemsSource = fulfilledOrders.ToList();
                fulfilledOrder.Items.Refresh();
            }
            else
            {
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
                                          Wartość = @String.Format("{0:C}", gr.Sum(x => x.OrderedProduts.Quantity * x.Products.NetSellingPrice))
                                      };
                fulfilledOrder.ItemsSource = fulfilledOrders.ToList();
                fulfilledOrder.Items.Refresh();
            }
            if (fulfilledOrder.Items.Count == 0)
            {
                MessageBox.Show("Brak zamówienia lub w realizacji");
            }

        }

        /// <summary>
        /// event handler opening window wiht adding new client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddClient_Click(object sender, RoutedEventArgs e)
        {
            AddCustomer addCustomer = new AddCustomer();

            addCustomer.Show();
        }

        /// <summary>
        /// event handler for searching client and last created order number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                                    where Customer.PhoneNumber == newOrderFindClient.Text || Customer.CompanyNumber == Convert.ToInt64(newOrderFindClient.Text)
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
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        /// <summary>
        /// event handler selecting client id from data grid and adding new order with this client to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            var cellInfo = newOrderClientData.SelectedCells[0];
            var selectedClientID = ((TextBlock)cellInfo.Column.GetCellContent(cellInfo.Item)).Text;
            try
            {
                App.dbContext.Orders.Add(new Order
                {
                    CustomerId = int.Parse(selectedClientID),
                    DateOfPlacingTheOrder = DateTime.Now,
                    OrderRealizationDate = null,
                    WhetherTheOrderFulfilled = false,
                    ShippingDate = null,
                    EmployeeId = loginWindowGlobalVariables.employeeId,
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
        }

        /// <summary>
        /// event handler opening window wiht adding product to order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProductAdd_Click(object sender, RoutedEventArgs e)
        {
            NewOrder newOrder = new NewOrder();

            newOrder.Show();
        }

        /// <summary>
        /// event handler selecting new order and printing this order data to sellwindow 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                                   Cena = @String.Format("{0:C}", Product.NetSellingPrice),
                                   Ilość = OrderedProduct.Quantity
                               };

            GlobalsMainWindow.cellInfoOrders = orderData.SelectedCells[0];
            GlobalsMainWindow.selectedOrders = (GlobalsMainWindow.cellInfoOrders.Column.GetCellContent(GlobalsMainWindow.cellInfoOrders.Item) as TextBlock).Text;

            SellWindow sellWindow = new SellWindow();
            sellWindow.selectedOrderId.Text = GlobalsMainWindow.selectedOrders.ToString();
            sellWindow.orderTotalValue.Text = $"Wartść zamówienia: {@String.Format("{0:C}", orderValue.Sum())}";
            sellWindow.selectedOrderDataSellWindow.ItemsSource = selectedOrderData.ToList();
            sellWindow.selectedOrderDataSellWindow.Items.Refresh();
            sellWindow.selectedProductsSellWindow.ItemsSource = productsList.ToList();
            sellWindow.selectedProductsSellWindow.Items.Refresh();
            sellWindow.employeesData.Text = $"Stworzone przez: {createdBy.FirstOrDefault()}";

            sellWindow.Show();
        }

        /// <summary>
        /// event handler for logout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loginWindowGlobalVariables.employeeId = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            this.Hide();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }

}
