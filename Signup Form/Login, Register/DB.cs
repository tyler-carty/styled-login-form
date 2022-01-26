using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Book.Login__Register
{
    class DB
    {
        private MySqlConnection connection = new MySqlConnection("server=localhost; database=pocketbookdb ;username=root; password=;SslMode=none");

        //function to open the connection to DB
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        //function to close the connection to DB
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        //function to return the connection
        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}
