using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Data;
using System.Reflection;
using Homeinns.Common.Util;
using System.ComponentModel;
using System.Diagnostics.Contracts;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description  扩展IEnumerable
 * @date 2013年9月11日12:38:26
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 * @refer
 */
namespace Homeinns.Common.Ext.Extension
{
    /// <summary>
    /// 扩展 IEnumerable IList<T>  JSON等 LINQ  TO  XML, IQuery
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// IEnumerable 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pageIndex">页数</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            Contract.Requires(pageIndex >= 0, "Page index cannot be negative");
            Contract.Requires(pageSize >= 0, "Page size cannot be negative");
            int skip = pageIndex * pageSize;
            if (skip > 0)
                source = source.Skip(skip);
            source = source.Take(pageSize);
            return source;
        }

        #region EnumerableDataTableExtension Add By Irving 2013年11月22日 11:29:36
        /// <summary>
        /// 转换为一个DataTable
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult : class
        {
            return value.ToDataTable(ConstantKey.DateFormatTime);
        }
        /// <summary>
        /// 转换为一个DataTable
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value, string format) where TResult : class
        {
            if (string.IsNullOrEmpty(format))
            {
                format = ConstantKey.DateFormatTime;
            }
            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(TResult);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(), p =>
            {
                pList.Add(p);
                if (p.PropertyType.IsGenericType)
                {
                    dt.Columns.Add(p.Name);
                }
                else
                {
                    dt.Columns.Add(p.Name, p.PropertyType);
                }
            });
            if (null != value)
            {
                foreach (var item in value)
                {
                    //创建一个DataRow实例
                    DataRow row = dt.NewRow();
                    //给row 赋值
                    pList.ForEach(p => row[p.Name] = (p.GetValue(item, null) is DateTime) ? Convert.ToDateTime(p.GetValue(item, null)) : p.GetValue(item, null));
                    //加入到DataTable
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        /// <summary>
        /// IEnumerable转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(this IEnumerable enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                dataTable.Columns.Add(pd.Name, pd.PropertyType);
            }
            foreach (T item in enumerable)
            {
                var Row = dataTable.NewRow();
                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;

        }
        #endregion


        #region  Add For xhzhang By Irving 2013年11月22日 12:00:06
        /// <summary>
        /// IEnumerable转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DataTable CreateTable<T>(this IList<T> value)
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (PropertyDescriptor prop in properties)
            {
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    NullableConverter converter = new NullableConverter(prop.PropertyType);
                    table.Columns.Add(prop.Name, converter.UnderlyingType);
                }
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }

        public static T CreateItem<T>(this DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {
                        // You can log something here
                        throw;
                    }
                }
            }
            return obj;
        }
        public static IList<T> ConvertTo<T>(this DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }
            return ConvertTo<T>(rows);
        }
        public static List<T> Distinct<T>(this IList<T> list)
        {
            List<T> list1 = new List<T>();
            foreach (T obj in list)
            {
                if (!list1.Contains(obj))
                {
                    list1.Add(obj);
                }
            }
            return list1;
        }
        public static DataTable ConvertTo<T>(this IList<T> list)
        {
            DataTable table = CreateTable<T>(list);
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);
            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    object val = prop.GetValue(item);
                    if (val != null)
                        row[prop.Name] = val;
                }

                table.Rows.Add(row);
            }
            return table;
        }

        public static IList<T> ConvertTo<T>(this IList<DataRow> rows)
        {
            IList<T> list = null;
            if (rows != null)
            {
                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }
            return list;
        }
        #endregion



        public static bool In<T>(this T t, params T[] c)
        {
            return c.Any(i => i.Equals(t));
        }

        public static bool In<T>(this T o, IEnumerable<T> c)
        {
            foreach (T i in c)
            {
                if (i.Equals(o))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Join(this IEnumerable source, string separator)
        {

            return null;
        }

        public static IEnumerable<T> ToTs<T>(this IEnumerable<T> items, Func<T, object> valueField, Func<T, object> textField)
        {
            return items.ToTs(valueField, textField, null);
        }

        public static IEnumerable<T> ToTs<T>(this IEnumerable<T> items, Func<T, object> valueField, Func<T, object> textField, Func<T, bool> isSelected)
        {
            List<T> listItems = new List<T>();
            foreach (T item in items)
            {
                string value = valueField != null ? valueField(item).ToString() : item.ToString();
                string text = textField != null ? textField(item).ToString() : item.ToString();
                bool selected = isSelected != null ? isSelected(item) : false;
                // T listItem = new { Value = value, Text = text, Selected = selected };
                //   listItems.Add(listItem);
            }
            return listItems;
        }
    }
}