﻿using PShop.Tables;
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


namespace PShop.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy NewOrder.xaml
    /// </summary>
    public partial class NewOrder : Window
    {
        public NewOrder()
        {
            InitializeComponent();
        }
        public class Globals
        {
           public static DataGridCellInfo cellInfoProduct;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void newOrderFindClient1_GotFocus(object sender, RoutedEventArgs e)
        {
            //customers = (from customer in App.dbContext.Customers
            //             select new
            //             {
            //                 customer.Surname
            //             }).ToDictionary();

            var empnamesEnum = from Product in App.dbContext.Products
                               select $"{(Product.Id).ToString()} {Product.ProductName} {Product.NetSellingPrice}";

            List<string> empnames = empnamesEnum.ToList();
            newOrderFindProduct.ItemsSource = empnames;

            //foreach (string cus in empnames)
            //{
            //    MessageBox.Show(cus);
            //}

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
                    newOrderProductData.SelectAll();
                    Globals.cellInfoProduct = newOrderProductData.SelectedCells[0];
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

        //public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        //{
        //    var itemsSource = grid.ItemsSource as IEnumerable;
        //    if (null == itemsSource) yield return null;
        //    foreach (var item in itemsSource)
        //    {
        //        var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
        //        if (null != row) yield return row;
        //    }
        //}

        //List<int> chuj = new List<int>();

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            int number;
            string quantity = productQuantity.Text;

            //var productsList = App.dbContext.Products.Where(t => chuj.Contains(t.Id)).Select();
            var idSelectProduct = from Product in App.dbContext.Products
                     where Product.ProductName == newOrderFindProduct.Text || Product.Id == (int.TryParse(newOrderFindProduct.Text, out number) ? number : 0)
                     select Product.Id;

            int idConvertInt = Convert.ToInt32(idSelectProduct.FirstOrDefault());
            MainWindow.GlobalsList.selectedProductId.Add(idConvertInt);

            MainWindow.GlobalsList.productQunatity.Add(quantity);

            var productsList = from Product in App.dbContext.Products
                               where MainWindow.GlobalsList.selectedProductId.Contains(Product.Id)
                               select new
                               {
                                   SKU = Product.Id,
                                   Nazwa = Product.ProductName,
                                   Cena = Product.NetSellingPrice,
                                   Ilość = MainWindow.GlobalsList.productQunatity.LastOrDefault()
                               };

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault(window => window is MainWindow);

            mainWindow.newOrderAddedProducts.ItemsSource = productsList.ToList();
            mainWindow.newOrderAddedProducts.Items.Refresh();

            var selecteProductId = (Globals.cellInfoProduct.Column.GetCellContent(Globals.cellInfoProduct.Item) as TextBlock).Text;

            var cellInfoOrder = addFindOraderData.SelectedCells[0];
            var selectedOrderId = (cellInfoOrder.Column.GetCellContent(cellInfoOrder.Item) as TextBlock).Text;

            try
            {
                App.dbContext.OrderedProducts.Add(new OrderedProduct
                {
                    OrderId = int.Parse(selectedOrderId),
                    ProductId = int.Parse(selecteProductId),
                    Quantity = int.Parse(productQuantity.Text)
                });
                App.dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

            this.Close();
        }

        private void btnAddFindedOrder_Click(object sender, RoutedEventArgs e)
        {
            if (addFindOrder.Text != "")
            {
                int number;
                var orders = from Order in App.dbContext.Orders
                             join Customers in App.dbContext.Customers on Order.CustomerId equals Customers.Id
                             where Order.WhetherTheOrderFulfilled == false && Order.Id == (int.TryParse(addFindOrder.Text, out number) ? number : 0) || Order.WhetherTheOrderFulfilled == false && Customers.Surname == addFindOrder.Text
                             group new { Order, Customers } by new { Order.Id, Customers.CustomerName, Customers.Surname } into gr
                             select new
                             {
                                 NumerZamówienia = gr.Key.Id,
                                 Imie = gr.Key.CustomerName,
                                 Nazwisko = gr.Key.Surname,
                             };

                addFindOraderData.ItemsSource = orders.ToList();
                addFindOraderData.Items.Refresh();
                //string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '0' AND orders.id='{(int.TryParse(findOrder.Text, out int number) ? number : 0)}' OR customers.surname='{findOrder.Text}' GROUP BY orders.id, customers.customer_name, customers.surname";
                //downloadData(query, orderData);
            }
            else
            {
                var orders = from Order in App.dbContext.Orders
                             join Customers in App.dbContext.Customers on Order.CustomerId equals Customers.Id
                             where Order.WhetherTheOrderFulfilled == false
                             group new { Order, Customers } by new { Order.Id, Customers.CustomerName, Customers.Surname } into gr
                             select new
                             {
                                 NumerZamówienia = gr.Key.Id,
                                 Imie = gr.Key.CustomerName,
                                 Nazwisko = gr.Key.Surname,
                             };

                addFindOraderData.ItemsSource = orders.ToList();
                addFindOraderData.Items.Refresh();
                //string query = $"SELECT orders.id AS [Numer zamówienia], customers.customer_name AS Imie, customers.surname AS Nazwisko, SUM(ordered_products.quantity * products.net_selling_price) AS Wartość FROM orders JOIN ordered_products ON orders.id = ordered_products.order_id JOIN products ON products.id = ordered_products.product_id JOIN customers ON customers.id = orders.customer_id WHERE orders.whether_the_order_fulfilled = '0' GROUP BY orders.id, customers.customer_name, customers.surname";
                //downloadData(query, orderData);
            }
            if (addFindOraderData.Items.Count == 0)
            {
                MessageBox.Show("Brak zamówienia lub zrealizowane");
            }
        }
    }
}
