using System;
using System.Data;
//using System.Data.SqlClient;

namespace Database
{
    public class LanguageTable
    {
        /*const string conString =
            "Data Source=(LocalDB)\\MSSQLLocalDB;" +
            "AttachDbFilename=D:\\Users\\Artur\\Documents\\SnakeTestDB.mdf;" +
            "Integrated Security=True;" +
            "Connect Timeout=30";

        static void Insert(string name, string genesF, string genesB)
        {
            SqlConnection con = new SqlConnection(conString);

            con.Open();

            if (con.State == System.Data.ConnectionState.Open)
            {
                string q = "insert into Genes(snakeName, genesF, genesB)" +
                    " values (\'" + name + "\', \'" + genesF + "\', \'" + genesB + "\') ";
                SqlCommand cmd = new SqlCommand(q, con);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Insertion was successful!");
            }
            else
                Console.WriteLine("Connection failed!");

            con.Close();
        }*/
    }

}