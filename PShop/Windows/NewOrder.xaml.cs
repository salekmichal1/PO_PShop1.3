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
        //public static class Globals
        //{
        //    public static int quantity { get; set; }
        //}

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
            int quantity = int.Parse(productQuantity.Text);

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
                                   Ilość = quantity
                               };

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault(window => window is MainWindow);

            mainWindow.newOrderAddedProducts.ItemsSource = productsList.ToList();
            mainWindow.newOrderAddedProducts.Items.Refresh();
            this.Close();
        }
    }
}
