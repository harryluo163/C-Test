using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Dapper的基本使用
{
    public class dbConfig
    {
        //连接数据库字符串。
        private readonly string sqlconnection =  ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString;
//public readonly string mysqlconnectionString = @"server=127.0.0.1;database=test;uid=renfb;pwd=123456;charset='gbk'";


        public SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(sqlconnection);
            connection.Open();
            return connection;
        }

        //获取MySql的连接数据库对象。MySqlConnection
        //public MySqlConnection OpenConnection()
        //{
        //     MySqlConnection connection = new MySqlConnection(mysqlconnectionString);
        //     connection.Open();
        //     return connection;
        //}
    }
}
