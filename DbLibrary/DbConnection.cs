using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace DbLibrary
{
    public abstract class DbConnection
    {
        protected MySqlConnection connection;
        protected string server;
        protected string database;
        protected string uid;
        protected string password;
        public DbConnection()
        {
            server = "37.187.107.7";
            database = "projekt_kacper_tip";
            uid = "user_io";
            password = "pyQjlJmoEADJZq0i";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

    }
}
