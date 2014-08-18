using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;
namespace Homeinns.Common.Data.Dapper
{
    /// <summary>
    /// 自定义扩展Dapper
    /// </summary>
    public class SqlExtension
    {
        /// <summary>
        /// 批量事物提交
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="entities"></param>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        public static int InsertMultiple<T>(string sql, IEnumerable<T> entities, string connectionName = null) where T : class, new()
        {
            using (var conn = DbService.OpenConnection(connectionName))
            {
                int records;
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {
                        records = conn.Execute(sql, entities, trans, 30, CommandType.Text);
                    }
                    catch (DataException ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                    trans.Commit();
                }
                return records;
            }
        }

        /*
         private void SaveServiceSnapshots(IEnumerable<ServiceCounterSnapshot> snapshots)
         {
             using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlDiagnosticsDb"].ConnectionString))
             {
                 conn.Open();
                 foreach (var snapshot in snapshots)
                 {
                     // insert new snapshot to the database
                    conn.Execute(
     @"insert into service_counter_snapshots(ServiceCounterId,SnapshotMachineName,CreationTimeUtc,ServiceCounterValue) values (
         @ServiceCounterId,@SnapshotMachineName,@CreationTimeUtc,@ServiceCounterValue)", snapshot);
                 }
             }
         }
         */
    }
}
