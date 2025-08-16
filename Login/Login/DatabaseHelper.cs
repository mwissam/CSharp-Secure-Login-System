using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login
{
    internal class DatabaseHelper
    {
        public static SqlConnection CreateConnection() 
        { 
            string serverIp = File.Exists("settings.txt") ? File.ReadAllText("settings.txt").Trim() : "localhost"; 
            string connectionString = $"Data Source={serverIp};Initial Catalog=Account;User Id=sa;Password=123;Connect Timeout=5;"; 
            return new SqlConnection(connectionString); 
        }
    }
}
