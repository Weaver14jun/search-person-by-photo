using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web_service.Services
{
    public class InitializeDB
    {
        public void InitializeeDB()
        {
            string dbPath = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (!File.Exists(dbPath))
            {
                //SQLiteConnection.CreateFile(dbPath);
            }
        }
    }
}