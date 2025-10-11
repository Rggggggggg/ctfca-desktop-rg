using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFCA_ADMIN
{
    internal class Database
    {
        // ✅ Your MySQL connection string
        private static string connectionString = "server=localhost;user=root;password=;database=ctfcai_enrollment;";

        //private static string connectionString = "server=bg8mskfc3w6xmuvmrhra-mysql.services.clever-cloud.com;user=ugilfrfmeunzv7kb;password=usX8s9ZERknJXS5Lu0uz;database=bg8mskfc3w6xmuvmrhra;";

        // ✅ Method to return a new connection object
        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
