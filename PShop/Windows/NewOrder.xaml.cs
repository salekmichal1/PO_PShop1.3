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
            public static string selecteProductId = "";

            public static DataGridCellInfo cellInfoNewOrder { get; set; }
            public static string selectedNewOrderId = "";

        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void newOrderFindClient1_GotFocus(object sender, RoutedEventArgs e)
        {
            var empnamesEnum = from Product in App.dbContext.Products
                               select Product.ProductName;

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

            if (newOrderVariables.selectedNewOrderId != "" && productQuantity.Text != "" && newOrderVariables.selecteProductId != "")
            {
                try
                {
                    var selectedProductExist = from OrderedProduct in App.dbContext.OrderedProducts
                                               where OrderedProduct.OrderId == Convert.ToInt32(newOrderVariables.selectedNewOrderId) && OrderedProduct.ProductId == Convert.ToInt32(newOrderVariables.selecteProductId)
                                               select OrderedProduct.Id;

                    if (selectedProductExist.Any())
                    {
                        (from OrderedProduct in App.dbContext.OrderedProducts
                         where OrderedProduct.OrderId == Convert.ToInt32(newOrderVariables.selectedNewOrderId) && OrderedProduct.ProductId == Convert.ToInt32(newOrderVariables.selecteProductId)
                         select OrderedProduct).ToList().ForEach(x => x.Quantity += int.Parse(productQuantity.Text));

                        App.dbContext.SaveChanges();
                    }
                    else
                    {
                        App.dbContext.OrderedProducts.Add(new OrderedProduct
                        {
                            OrderId = int.Parse(newOrderVariables.selectedNewOrderId),
                            ProductId = int.Parse(newOrderVariables.selecteProductId),
                            Quantity = int.Parse(productQuantity.Text)
                        });
                        App.dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message);
                }

                newOrderVariables.selectedNewOrderId = "";
                newOrderVariables.selecteProductId = "";
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
                             group new { Order, Customers } by new { Order.Id, Customers.CustomerName, Customers.Surname, Customers.Mail } into gr
                             select new
                             {
                                 NumerZamówienia = gr.Key.Id,
                                 Imie = gr.Key.CustomerName,
                                 Nazwisko = gr.Key.Surname,
                                 Mail = gr.Key.Mail
                             };

                addFindOraderData.ItemsSource = orders.ToList();
                addFindOraderData.Items.Refresh();
            }
            else
            {
                var orders = from Order in App.dbContext.Orders
                             join Customers in App.dbContext.Customers on Order.CustomerId equals Customers.Id
                             where Order.WhetherTheOrderFulfilled == false
                             group new { Order, Customers } by new { Order.Id, Customers.CustomerName, Customers.Surname, Customers.Mail } into gr
                             select new
                             {
                                 NumerZamówienia = gr.Key.Id,
                                 Imie = gr.Key.CustomerName,
                                 Nazwisko = gr.Key.Surname,
                                 Mail = gr.Key.Mail
                             };

                addFindOraderData.ItemsSource = orders.ToList();
                addFindOraderData.Items.Refresh();
            }
            if (addFindOraderData.Items.Count == 0)
            {
                MessageBox.Show("Brak zamówienia lub zrealizowane");
            }
        }

    }
}
