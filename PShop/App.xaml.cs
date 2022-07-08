﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PShop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string connectionString = @"Data Source=LAPTOP-9A79R96U;Initial Catalog=rtvDatabase;Integrated Security=True";
        public static ApplicationDbContext dbContext;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            dbContext = new ApplicationDbContext(connectionString);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            dbContext.Dispose();
        }
    }
}
