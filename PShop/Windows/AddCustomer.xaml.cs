using Microsoft.Data.SqlClient;
using PShop.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logika interakcji dla klasy AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// event handler adding new client to database and cheing if mail exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddClientData_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //string query = $"SET IDENTITY_INSERT Customers ON INSERT INTO Customers(id, customer_name, surname, company_name, company_number, street, street_number, flat_number, post_code, city, phone_number, mail) VALUES ({id+1}, '{clientName.Text}', '{clientSurname.Text}', {((clientCompanyName.Text != "") ? $"'{clientCompanyName.Text}'" : "NULL")}, {(int.TryParse(clientCompanyNubmer.Text, out int companyNumber) ? companyNumber : "NULL")}, '{clientStreet.Text}', {int.Parse(clientStreetNumber.Text)}, {(int.TryParse(clientFlatNumber.Text, out int flatNumber) ? flatNumber : "NULL")}, '{clientPostCode.Text}', '{clientCity.Text}', '{clientPhone.Text}', '{clientMail.Text}') SET IDENTITY_INSERT Customers OFF";
                //insertData(query);
                var email = App.dbContext.Customers.FirstOrDefault(u => u.Mail.ToLower() == clientMail.Text.ToLower());

                if (email == null)
                {
                    if (clientName.Text != "" && clientSurname.Text != "" && clientMail.Text != "" && clientStreet.Text != "" && clientPhone.Text != "" && clientCity.Text != "" && clientPostCode.Text != "" && clientStreetNumber.Text != "")
                    {

                        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        Match match = regex.Match(clientMail.Text);
                        if (match.Success)
                        {
                            App.dbContext.Customers.AddAsync(new Customer
                            {
                                CustomerName = clientName.Text,
                                Surname = clientSurname.Text,
                                CompanyName = ((clientCompanyName.Text != "") ? clientCompanyName.Text : null),
                                CompanyNumber = (int.TryParse(clientCompanyNubmer.Text, out int companyNumber) ? companyNumber : null),
                                Street = clientStreet.Text,
                                StreetNumber = int.Parse(clientStreetNumber.Text),
                                FlatNumber = (int.TryParse(clientFlatNumber.Text, out int flatNumber) ? flatNumber : null),
                                PostCode = clientPostCode.Text,
                                City = clientCity.Text,
                                PhoneNumber = clientPhone.Text,
                                Mail = clientMail.Text
                            });
                            App.dbContext.SaveChangesAsync();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Nieprawidłowy email");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Brakuje danych");
                    }
                }
                else
                {
                    MessageBox.Show("Email już istnieje");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }
    }
}
