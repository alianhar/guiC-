using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace toko_laptop_tugas
{
    internal class DBConnection
    {
        public static string ConnectionString = @"Data Source=DESKTOP-PDGNVP8\SQLEXPRESS;Initial Catalog=db_toko_laptop_tugas;Integrated Security=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
