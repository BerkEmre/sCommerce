using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace sCommerce.Helper
{
    public static class SQL
    {
        #if DEBUG
            static string text = ConfigurationManager.ConnectionStrings["debug"].ConnectionString;
        #else
            static string text = ConfigurationManager.ConnectionStrings["relase"].ConnectionString;
        #endif

        public static bool baglanti_test()
        {
            using (SqlConnection con = new SqlConnection(@text))
            {
                try
                {
                    SQL.get("SELECT * FROM kullanicilar"); return true;
                }
                catch
                {
                    con.Close();
                    return false;
                }
            }
        }

        public static DataTable get(string query)
        {
            using (SqlConnection con = new SqlConnection(@text))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataSet ds = new DataSet();
                    con.Open();
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    con.Close();
                    return dt;
                }
                catch
                {
                con.Close();
                return null;
                }
            }
        }

        public static void set(string query)
        {
            using (SqlConnection con = new SqlConnection(@text))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch
                {
                    con.Close();
                }
            }
        }

        public static string MD5(string sifrelenecekMetin)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            StringBuilder sb = new StringBuilder();
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);

            dizi = md5.ComputeHash(dizi);

            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            return sb.ToString();
        }

    }
}