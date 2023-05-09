using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Services.Description;

namespace AccessSystem.Repository
{
    public class LogRepository
    {
        public static string LOGDATAIN(string NameID, string RFIDCODE)
        {
            string OUTPUT = string.Empty;
            string connstring = ConfigurationManager.ConnectionStrings["AccessSystemDBEntities"].ConnectionString;
            string providerString = new EntityConnectionStringBuilder(connstring).ProviderConnectionString;
            SqlConnection conn = new SqlConnection(providerString);
            try
            {
                using (conn)
                {
                    using (SqlCommand cmd = new SqlCommand("SP_INSERT_LOG", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NAMEID", NameID);
                        cmd.Parameters.AddWithValue("@RFIDCODE", RFIDCODE);
                        cmd.Parameters.Add("@OUTPUT", SqlDbType.NVarChar, 200);
                        cmd.Parameters["@OUTPUT"].Direction = ParameterDirection.Output;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        OUTPUT = (string)cmd.Parameters["@OUTPUT"].Value;
                        conn.Close();
                    }
                }
                return OUTPUT;
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
        }

        public static string LOGDATAOUT(string NameID)
        {
            string OUTPUT = string.Empty;
            string connstring = ConfigurationManager.ConnectionStrings["AccessSystemDBEntities"].ConnectionString;
            string providerString = new EntityConnectionStringBuilder(connstring).ProviderConnectionString;
            SqlConnection conn = new SqlConnection(providerString);
            try
            {
                using (conn)
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UPDATE_LOG", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NAMEID", NameID);
                        cmd.Parameters.Add("@OUTPUT", SqlDbType.NVarChar, 200);
                        cmd.Parameters["@OUTPUT"].Direction = ParameterDirection.Output;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        OUTPUT = (string)cmd.Parameters["@OUTPUT"].Value;
                        conn.Close();
                    }
                }
                return OUTPUT;
            }
            catch (SqlException ex)
            {
                return ex.Message;
            }
        }

    }
}