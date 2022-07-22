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
using static PShop.MainWindow;

namespace PShop.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {
        public SellWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// event handler changing order status for closed in database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSellOrder_Click(object sender, RoutedEventArgs e)
        {
            (from Order in App.dbContext.Orders
             where Order.Id == Convert.ToInt32(GlobalsMainWindow.selectedOrders)
             select Order).ToList().ForEach(x => x.WhetherTheOrderFulfilled = true);
            App.dbContext.SaveChanges();

            this.Close();

        }
    }
}
