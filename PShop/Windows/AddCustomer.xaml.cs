﻿using Microsoft.Data.SqlClient;
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
    /// Logika interakcji dla klasy AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        public AddCustomer()
        {
            InitializeComponent();
        }

        public void insertData(string query)
        {
            SqlConnection conection = new SqlConnection(@"Data Source=LAPTOP-9A79R96U;Initial Catalog=rtvDatabase;Integrated Security=True");

            conection.Open();
            SqlCommand command = new SqlCommand(query, conection);
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.InsertCommand = new SqlCommand(query, conection);
            adapter.InsertCommand.ExecuteNonQuery();

            command.Dispose();
            conection.Close();
        }

        private void btnAddClientData_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conection = new SqlConnection(@"Data Source=LAPTOP-9A79R96U;Initial Catalog=rtvDatabase;Integrated Security=True");
            string queryId = $"SELECT MAX(id) FROM Customers";
            conection.Open();
            SqlCommand commandId = new SqlCommand(queryId, conection);
            int id = Convert.ToInt32(commandId.ExecuteScalar());

            commandId.Dispose();
            conection.Close();

            try
            {             
                string query = $"SET IDENTITY_INSERT Customers ON INSERT INTO Customers(id, customer_name, surname, company_name, company_number, street, street_number, flat_number, post_code, city, phone_number, mail) VALUES ({id+1}, '{clientName.Text}', '{clientSurname.Text}', {((clientCompanyName.Text != "") ? $"'{clientCompanyName.Text}'" : "NULL")}, {(int.TryParse(clientCompanyNubmer.Text, out int companyNumber) ? companyNumber : "NULL")}, '{clientStreet.Text}', {int.Parse(clientStreetNumber.Text)}, {(int.TryParse(clientFlatNumber.Text, out int flatNumber) ? flatNumber : "NULL")}, '{clientPostCode.Text}', '{clientCity.Text}', '{clientPhone.Text}', '{clientMail.Text}') SET IDENTITY_INSERT Customers OFF";
                insertData(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błędne dane. Sprawdź poprawność");
            }
            this.Close();
        }
    }
}