using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
//using System.Data.SQLite;

namespace Homeinns.Common.Entity.Schema
{
    public class NRDbProviderFactories
    {
        public static DbProviderFactory GetFactory(string providerName)
        {
            if (providerName == null)
                throw new ArgumentNullException("providerName");
         //   DbProviderFactory dbFactory;
            switch (providerName)
            {
                //case "System.Data.SQLite":
                //    return new SQLiteFactory();
                default:
                    return DbProviderFactories.GetFactory(providerName);
            }
        }
        public static string GetParameterMarkerFormat(DbConnection connect)
        {
            if (connect == null)
                throw new ArgumentNullException("connect");
            Type type = connect.GetType();
            //if (type == typeof(System.Data.SQLite.SQLiteConnection))
            //    return "${0}";
            if (type == typeof(System.Data.SqlClient.SqlConnection))
                return "@{0}";//ms bug
            connect.Open();
            string result = connect.GetSchema("DataSourceInformation").Rows[0]["ParameterMarkerFormat"].ToString();
            connect.Close();
            return result;
        }

    }
}
