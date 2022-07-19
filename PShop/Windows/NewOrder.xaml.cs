using PShop.Tables;
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
        public class newOrderVariables
        {
            public static DataGridCellInfo cellInfoProduct { get; set; }
            public static string selecteProductId { get; set; }

            public static DataGridCellInfo cellInfoNewOrder { get; set; }
            public static string selectedNewOrderId { get; set; }
           
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
        private void newOrderProductData_Selected(object sender, RoutedEventArgs e)
        {
            newOrderVariables.cellInfoProduct = newOrderProductData.SelectedCells[0];
            newOrderVariables.selecteProductId = (newOrderVariables.cellInfoProduct.Column.GetCellContent(newOrderVariables.cellInfoProduct.Item) as TextBlock).Text;

        }

        private void addFindOraderData_Selected(object sender, RoutedEventArgs e)
        {
            newOrderVariables.cellInfoNewOrder = addFindOraderData.SelectedCells[0];
            newOrderVariables.selectedNewOrderId = (newOrderVariables.cellInfoNewOrder.Column.GetCellContent(newOrderVariables.cellInfoNewOrder.Item) as TextBlock).Text;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            int number;
            string quantity = productQuantity.Text;

            var idSelectProduct = from Product in App.dbContext.Products
                     where Product.ProductName == newOrderFindProduct.Text || Product.Id == (int.TryParse(newOrderFindProduct.Text, out number) ? number : 0)
                     select Product.Id;

            int idConvertInt = Convert.ToInt32(idSelectProduct.FirstOrDefault());
            MainWindow.GlobalsMainWindow.selectedProductId.Add(idConvertInt);

            MainWindow.GlobalsMainWindow.productQunatity.Add(quantity);

            var productsList = from Product in App.dbContext.Products
                               where MainWindow.GlobalsMainWindow.selectedProductId.Contains(Product.Id)
                               select new
                               {
                                   SKU = Product.Id,
                                   Nazwa = Product.ProductName,
                                   Cena = Product.NetSellingPrice,
                                   Ilość = MainWindow.GlobalsMainWindow.productQunatity.LastOrDefault()
                               };

            // lista w mainwindow nowe zamówienie
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(window => window is MainWindow);

            mainWindow.newOrderAddedProducts.ItemsSource = productsList.ToList();
            mainWindow.newOrderAddedProducts.Items.Refresh();

            ///////////////////////////////////////////////////////////////////////////


            if (newOrderVariables.selectedNewOrderId != null && productQuantity.Text != "" && newOrderVariables.selecteProductId != null)
            {
                try
                {
                    App.dbContext.OrderedProducts.Add(new OrderedProduct
                    {
                        OrderId = int.Parse(newOrderVariables.selectedNewOrderId),
                        ProductId = int.Parse(newOrderVariables.selecteProductId),
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
            else
            {
                MessageBox.Show("Ilość, produkt, lub zamówienie nie zostały wybrane");
            }
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
