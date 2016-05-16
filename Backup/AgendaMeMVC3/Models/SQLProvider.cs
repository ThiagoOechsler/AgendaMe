using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace AgendaMeMVC3.Models
{
    public partial class SQLProvider
    {

        SqlConnection conn = new SqlConnection("Data Source=(local);Initial Catalog=AgendaMe;Integrated Security=SSPI");

        public SqlDataReader OpenSelect(string prSQL)
        {
            conn.Open();

            SqlCommand cmd = new SqlCommand(prSQL);
            cmd.Connection = conn;

            SqlDataReader rdr = cmd.ExecuteReader();

            return rdr;            
        }

        public void CloseSelect()
        {            
            if (conn != null)
            {
                conn.Close();
            }
        }



        public string RetornaValorString(string prSQL)
        {
            SQLProvider sql = new SQLProvider();

            String result;

            SqlDataReader rdr = sql.OpenSelect(prSQL);
            rdr.Read();
            result = Convert.ToString(rdr[0]);
            rdr.Close(); //fecha o reader
            sql.CloseSelect();

            return result;
        }

    }
}