using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Diagnostics;
using Homeinns.Common.Entity.Content;
using Homeinns.Common.Entity.Schema;
using Homeinns.Common.Entity.Text;
using Homeinns.Common.Entity.Association;

namespace Homeinns.Common.Entity.Common
{

    public delegate DbConnection CreateConnectionDelegate();
    /// <summary>
    /// 数据访问层数据自动传递
    /// </summary>
    public class DatabaseObject<T> where T : class, new()
    {
        /// <summary>
        /// Sql分析处理
        /// </summary>
        private readonly CommandTextAnalyser _CommandStringAnalyser = new CommandTextAnalyser();

        /// <summary>
        /// 数据库参数数据 (包含有本数据库数据所有表所有列的表名/列名/数据类型/数据长度/是否为空的的数据)
        /// </summary>
        private readonly DatabaseSchema _dbSchema;

        private DbConnection _conn;

        /// <summary>
        /// 是否是内部创建连接
        /// </summary>
        private bool _innerConnection;

        private DbTransaction _tran;

        public DatabaseObject(DatabaseSchema dbSchema)
        {
            _dbSchema = dbSchema;
            SetCreateConnectionFun(CreateConnectionFun);
        }

        /// <summary>
        /// 数据库结构
        /// </summary>
        public DatabaseSchema DBSchema
        {
            get { return _dbSchema; }
        }

        #region 当前表事务及连接操作

        /// <summary>
        /// 可用事务
        /// </summary>
        private DbTransaction Transaction
        {
            get
            {
                if (!_innerConnection && _tran != null && _tran.Connection != null &&
                    _tran.Connection.State != ConnectionState.Closed)
                {
                    return _tran;
                }
                return null;
            }
        }



        /// <summary>
        /// 返回已打开的数据库连接
        /// </summary>
        /// <returns></returns>
        public DbConnection BeginConnection()
        {
            StartOperation();
            _innerConnection = false;
            return _conn;
        }

        /// <summary>
        /// 设置数据库连接对象
        /// </summary>
        /// <param name="conn"></param>
        public void SetConnection(DbConnection conn)
        {
            _conn = conn;
            _innerConnection = false;
        }

        /// <summary>
        /// 设置事务对象
        /// </summary>
        /// <param name="tran"></param>
        public void SetTransaction(DbTransaction tran)
        {
            SetConnection(tran.Connection);
            _tran = tran;
        }

        private CreateConnectionDelegate createConnDel;

        public void SetCreateConnectionFun(CreateConnectionDelegate fun)
        {
            createConnDel = fun;
        }

        private DbConnection CreateConnectionFun()
        {
            return DBSchema.ProviderFactory.CreateConnection();
        }

        /// <summary>
        /// 数据操作开始
        /// </summary>
        private void StartOperation()
        {
            if (_conn == null || _conn.State == ConnectionState.Closed)
            {
                //_conn = DBSchema.ProviderFactory.CreateConnection();
                if (createConnDel == null)
                {
                    throw new Exception("委托实例中没有创建DbConnection的方法!");
                }
                _conn = createConnDel();
                _conn.ConnectionString = DBSchema.ConnectionString;
                _innerConnection = true;
                _conn.Open();
            }
        }

        /// <summary>
        /// 数据操作结束
        /// </summary>
        private void EndOperation()
        {
            if (_innerConnection && _conn != null && _conn.State != ConnectionState.Closed)
            {
                _conn.Close();
                _conn = null;
            }
        }

        /// <summary>
        /// 给DbCommand绑定Connection/Transaction
        /// </summary>
        /// <param name="command"></param>
        private void BindDbCommandConnAndTran(DbCommand command)
        {
            command.Connection = _conn;
            if (Transaction != null)
            {
                command.Transaction = _tran;
            }
        }

        /// <summary>
        /// 回滚可回滚的事务
        /// </summary>
        private void RollbackTransaction()
        {
            if (Transaction != null)
            {
                _tran.Rollback();
            }
        }

        #endregion

        #region 根据Sql语句生成DbCommand对象

        public DbCommand CreateDbCommand(string commandText)
        {
            DbCommand command = DBSchema.ProviderFactory.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// 根据Sql语句与相应顺序排列的变量生成DbCommand对象
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="variables">变量</param>
        /// <returns>DbCommand对象</returns>
        public DbCommand CreateDbCommandTypedParams(string commandText, params object[] variables)
        {
            return CreateDbCommandTypedParamArray(commandText, variables);
        }

        /// <summary>
        /// 根据DbCommand字符串与实体类对象生成dbCommand
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DbCommand CreateDbCommandTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = DBSchema.ProviderFactory.CreateCommand();
            command.CommandText = commandText;

            if (entity == null)
                return command;
            string[] parameterNames = _CommandStringAnalyser.GetDbParameterNames(commandText);
            if (parameterNames == null)
                return command;
            List<string> tableNames = _CommandStringAnalyser.GetTableNames(commandText);
            Type entityType = entity.GetType();

            for (int i = 0; i < parameterNames.Length; ++i)
            {
                ColumnSchema columnSchema;
                DbParameter parameter = CreateDbParameter(tableNames, parameterNames[i], out columnSchema);
                PropertyInfo property = entityType.GetProperty(parameterNames[i]);
                ColumnAssociationAttribute ca = (ColumnAssociationAttribute)Attribute.GetCustomAttribute(property, typeof(ColumnAssociationAttribute));
                object parameterValue = null;
                if (ca == null || ca.IsColumn)
                {
                    parameterValue = property.GetValue(entity, null);
                }
                parameter.Value = parameterValue ?? DBNull.Value;
                // 参数绑定后事件激发
                DbParameterBoundEventFunction(columnSchema, parameter);
                command.Parameters.Add(parameter);
            }

            return command;
        }


        /// <summary>
        /// 根据Sql语句字符串与变量列表生成相应的DbCommand对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public DbCommand CreateDbCommandTypedParamArray(string commandText, object[] variables)
        {
            DbCommand command = DBSchema.ProviderFactory.CreateCommand();
            command.CommandText = commandText;

            string[] parameterNames = _CommandStringAnalyser.GetDbParameterNames(commandText);
            if (parameterNames == null)
                return command;
            List<string> tableNames = _CommandStringAnalyser.GetTableNames(commandText);


            for (int i = 0; i < parameterNames.Length; ++i)
            {
                ColumnSchema columnSchema;
                DbParameter parameter = CreateDbParameter(tableNames, parameterNames[i], out columnSchema);
                object variableValue = i < variables.Length ? variables[i] : null;
                parameter.Value = variableValue ?? DBNull.Value;
                // 参数绑定后事件激发
                DbParameterBoundEventFunction(columnSchema, parameter);
                command.Parameters.Add(parameter);
            }

            return command;
        }


        /// <summary>
        /// 为delete in 语句生成Command对象
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="columnName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public DbCommand CreateCommandForInClause(string commandText, string columnName, object[] variables)
        {
            DbCommand command = DBSchema.ProviderFactory.CreateCommand();
            command.CommandText = commandText;

            string[] parameterNames = _CommandStringAnalyser.GetDbParameterNames(commandText);
            if (parameterNames == null)
                return command;
            List<string> tableNames = _CommandStringAnalyser.GetTableNames(commandText);

            for (int i = 0; i < parameterNames.Length; ++i)
            {
                ColumnSchema columnSchema;
                DbParameter parameter = CreateDbParameter(tableNames, columnName, out columnSchema);
                object variableValue = i < variables.Length ? variables[i] : null;
                parameter.Value = variableValue ?? DBNull.Value;
                // 绑定后事件激发
                DbParameterBoundEventFunction(columnSchema, parameter);
                command.Parameters.Add(parameter);
            }

            return command;
        }

        /// <summary>
        /// 生成DbParameter对象
        /// </summary>
        /// <param name="tableNames"></param>
        /// <param name="columnName"></param>
        /// <param name="columnSchema"></param>
        /// <returns></returns>
        private DbParameter CreateDbParameter(List<string> tableNames, string columnName,
                                              out ColumnSchema columnSchema)
        {
            DbParameter parameter = DBSchema.ProviderFactory.CreateParameter();

            parameter.ParameterName = "@" + columnName;
            columnSchema = _dbSchema.GetColumnSchema(tableNames, columnName);
            if (columnSchema != null)
            {
                parameter.DbType = columnSchema.ColumnType;
                if (columnSchema.ColumnLength != null)
                    parameter.Size = columnSchema.ColumnLength.Value;
            }
            else
            {
                columnSchema = new ColumnSchema { ColumnName = columnName };
            }

            return parameter;
        }

        /// <summary>
        /// 创建参数列表
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public DbParameter[] CreateParameterArrayValue(string commandText, object[] values)
        {
            string[] parameterNames = _CommandStringAnalyser.GetDbParameterNames(commandText);
            var parameters = new DbParameter[parameterNames.Length];
            DbParameter parameter;
            for (int i = 0; i < parameters.Length; ++i)
            {
                parameter = DBSchema.ProviderFactory.CreateParameter();
                parameter.ParameterName = "@" + parameterNames[i];
                parameter.Value = values[i];

                parameters[i] = parameter;
            }

            return parameters;
        }


        /// <summary>
        /// Parameter绑定后事件激发
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="parameter"></param>
        private void DbParameterBoundEventFunction(ColumnSchema schema, DbParameter parameter)
        {
            if (DbParameterBoundEvent != null)
            {
                DbParameterBoundEvent(parameter, new DbParameterBoundEventArgs(schema, parameter));
            }
        }

        #endregion

        #region 执行DbCommand

        /// <summary>
        /// 填充已有DataSet
        /// </summary>
        /// <param name="command">DbCommand对象</param>
        /// <param name="ds">DataSet对象</param>
        /// <param name="tableName">表名(可空,空则只有一个DataTable)</param>
        public void FillDataSet(DbCommand command, DataSet ds, string tableName)
        {
            try
            {
                PrintCommand(command);
                StartOperation();
                BindDbCommandConnAndTran(command);
                DbDataAdapter da = DBSchema.ProviderFactory.CreateDataAdapter();
                da.SelectCommand = command;
                if (tableName == null)
                    da.Fill(ds);
                else
                    da.Fill(ds, tableName);
                EndOperation();
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// 基础的执行DbCommand并返回DataSet
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbCommand command)
        {
            try
            {
                PrintCommand(command);
                StartOperation();
                BindDbCommandConnAndTran(command);
                DbDataAdapter da = DBSchema.ProviderFactory.CreateDataAdapter();
                da.SelectCommand = command;
                var ds = new DataSet();
                da.Fill(ds);
                EndOperation();

                return ds;
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }


        /// <summary>
        /// 执行DbCommand返回影响行数
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbCommand command)
        {
            try
            {
                PrintCommand(command);
                StartOperation();
                BindDbCommandConnAndTran(command);
                int count = command.ExecuteNonQuery();
                EndOperation();

                return count;
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// 执行DbCommand返回DbReader
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(DbCommand command)
        {
            //try
            //{
            PrintCommand(command);
            StartOperation();
            BindDbCommandConnAndTran(command);
            DbDataReader reader = command.ExecuteReader();
            //EndOperation();

            return reader;
            //}
            //catch (Exception)
            //{
            //    RollbackTransaction();
            //    throw;
            //}
        }

        /// <summary>
        /// 执行DbCommand并返回第一行第一列数据
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbCommand command)
        {
            try
            {
                PrintCommand(command);
                StartOperation();
                BindDbCommandConnAndTran(command);
                object obj = command.ExecuteScalar();
                EndOperation();
                return obj;
            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// 执行Sql语句并返回是否执行成功
        /// </summary>
        /// <param name="command">Sql语句字符串</param>
        /// <returns>是否执行成功</returns>
        public bool ExecuteBool(DbCommand command)
        {
            return ExecuteNonQuery(command) != 0;
        }

        /// <summary>
        /// 执行DbCommand对象并返回相应的实体类泛型集合
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="command">DbCommand对象</param>
        /// <returns>实体类泛型集合</returns>
        public List<T> BuildEntityList(DbCommand command)
        {
            List<T> entityList;
            using (DbDataReader reader = ExecuteReader(command))
            {
                entityList = BuildEntityList(reader);
            }

            EndOperation();
            return entityList;
        }

        /// <summary>
        /// (分页)执行DbCommand对象并返回相应的实体类泛型集合
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="command">DbCommand对象</param>
        /// <param name="indicator"></param>
        /// <returns>实体类泛型集合</returns>
        public List<T> BuildEntityListPage(DbCommand command, PageIndicatorInfo indicator)
        {
            List<T> entityList;
            using (DbDataReader reader = ExecuteReader(command))
            {
                entityList = BuildEntityListPage(reader, indicator);
            }

            EndOperation();
            return entityList;
        }

        /// <summary>
        /// 根据DbCommand返回基本数据类型泛型数组
        /// </summary>
        /// <typeparam name="T">基本数据类型</typeparam>
        /// <param name="command">DbCommand</param>
        /// <returns>基本数据类型泛型数组</returns>
        public List<BaseT> BuildObjectList<BaseT>(DbCommand command)
        {
            List<BaseT> objList;
            using (DbDataReader reader = ExecuteReader(command))
            {
                objList = BuildObjectList<BaseT>(reader);
            }

            EndOperation();
            return objList;
        }

        /// <summary>
        /// (分页)执行DbCommand对象并返回相应的DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public DataTable BuildDataTablePage(DbCommand command, PageIndicatorInfo indicator)
        {
            DataTable dataTable;
            using (DbDataReader reader = ExecuteReader(command))
            {
                dataTable = BuildDataTablePage(reader, indicator);
            }

            EndOperation();
            return dataTable;
        }

        /// <summary>
        /// 根据DbCommand对象生成实体类对象
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="command">DbCommand对象</param>
        /// <returns>实体类对象</returns>
        public T MakeEntity(DbCommand command)
        {
            T entity;
            using (DbDataReader reader = ExecuteReader(command))
            {
                entity = MakeEntity(reader);
            }

            EndOperation();
            return entity;
        }

        #endregion

        #region 执行Sql语句并返回受影响行数

        /// <summary>
        /// 执行Sql语句并返回受影响的行数
        /// </summary>
        /// <param name="commandText">Sql语句字符串</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteInt(string commandText)
        {
            DbCommand command = CreateDbCommand(commandText);
            return ExecuteNonQuery(command);
        }

        /// <summary>
        /// 事务的执行Sql返回影响行数
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="entity">包含参数数据的实体类对象</param>
        /// <returns>影响行数</returns>
        public int ExecuteIntTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return ExecuteNonQuery(command);
        }

        /// <summary>
        /// 执行Sql语句并返回受影响的行数
        /// </summary>
        /// <param name="commandText">Sql语句字符串</param>
        /// <param name="variables">参数值</param>
        /// <returns>受影响的行数</returns>
        public int ExecuteIntTypedParams(string commandText, object[] variables)
        {
            DbCommand command = variables == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedParamArray(commandText, variables);

            return ExecuteNonQuery(command);
        }

        #endregion

        #region 执行Sq语句并返回是否有受影响数据

        /// <summary>
        /// 执行Sql语句并返回是否执行成功
        /// </summary>
        /// <returns>是否执行成功</returns>
        public bool ExecuteBool(string commandText)
        {
            DbCommand command = CreateDbCommand(commandText);

            return ExecuteBool(command);
        }

        /// <summary>
        /// 执行Sql语句并返回是否有受影响数据
        /// </summary>
        /// <param name="commandText">Sql语句字符串</param>
        /// <param name="varibles">参数变量列表</param>
        /// <returns>返回是否有受影响数据</returns>
        public bool ExecuteBoolTypedArray(string commandText, object[] varibles)
        {
            return ExecuteIntTypedParams(commandText, varibles) != 0;
        }

        /// <summary>
        /// 执行Sql语句并返回是否有受影响数据
        /// </summary>
        /// <param name="commandText">Sql语句字符串</param>
        /// <param name="varibles">参数变量列表</param>
        /// <returns>返回是否有受影响数据</returns>
        public bool ExecuteBoolTypedParams(string commandText, params object[] varibles)
        {
            return ExecuteBoolTypedArray(commandText, varibles);
        }


        /// <summary>
        /// 执行Sql语句并返回是否执行成功
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="entity">包含数据的实体类对象</param>
        /// <returns>是否执行成功</returns>
        public bool ExecuteBoolTypedEntity(string commandText, IEntity entity)
        {
            return ExecuteIntTypedEntity(commandText, entity) != 0;
        }

        #endregion

        #region 根据Sql语句或DbDataReader生成泛型集合

        /// <summary>
        /// 根据没有参数的DbCommand字符串生成实体类泛型集合
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">DbCommand字符串</param>
        /// <returns>实体类泛型集合</returns>
        public List<T> BuildEntityList(string commandText)
        {
            return BuildEntityListTypedEntity(commandText, null);
        }

        /// <summary>
        /// 根据DbCommand字符串和实体类对象生成实体类泛型集合
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">DbCommand语句</param>
        /// <param name="entity">实体类对象</param>
        /// <returns>实体类泛型集合</returns>
        public List<T> BuildEntityListTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return BuildEntityList(command);
        }

        /// <summary>
        /// 根据DbCommand字符串和实体类对象生成实体类泛型集合
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">DbCommand语句</param>
        /// <param name="variables">参数变量列表</param>
        /// <returns>实体类泛型集合</returns>
        public List<T> BuildEntityListTypedParams(string commandText, params object[] variables)
        {
            DbCommand command = variables == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedParamArray(commandText, variables);

            return BuildEntityList(command);
        }


        /// <summary>
        /// 从DbDataReader中读取数据置入泛型类集合中
        /// </summary>
        /// <typeparam name="T">Object类型</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns>实体类集合</returns>
        public List<T> BuildEntityList(DbDataReader reader)
        {
            var entityList = new List<T>();

            Type entityType = typeof(T);

            while (reader.Read())
            {
                var entity = new T();
                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    if (reader.IsDBNull(i))
                        continue;
                    string propertyName = reader.GetName(i);
                    PropertyInfo property = entityType.GetProperty(propertyName);

                    property.SetValue(entity, reader.GetValue(i), null);
                } // endFor

                entityList.Add(entity);
            } // endWhile

            return entityList;
        } // endBindDbDataReaderToObject


        /// <summary>
        /// (分页)根据DbCommand字符串和实体类对象生成实体类泛型集合
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">DbCommand语句</param>
        /// <param name="entity">实体类对象</param>
        /// <param name="indicator">分页导航</param>
        /// <returns>实体类泛型集合</returns>
        public List<T> BuildEntityListPageTypedEntity(string commandText, IEntity entity, PageIndicatorInfo indicator)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return BuildEntityListPage(command, indicator);
        }


        /// <summary>
        /// (分页)从DbDataReader中读取数据置入泛型类集合中
        /// </summary>
        /// <typeparam name="T">Object类型</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <param name="indicator"></param>
        /// <returns>实体类集合</returns>
        public List<T> BuildEntityListPage(DbDataReader reader, PageIndicatorInfo indicator)
        {
            var entityList = new List<T>();

            Type entityType = typeof(T);

            int showPageFirstIndex = (indicator.PageIndex - 1) * indicator.PageSize;
            int showPageLastIndex = indicator.PageIndex * indicator.PageSize;
            int dataIndex = 0;

            while (reader.Read())
            {
                if (dataIndex >= showPageFirstIndex && dataIndex <= showPageLastIndex)
                {
                    var entity = new T();
                    for (int i = 0; i < reader.FieldCount; ++i)
                    {
                        if (reader.IsDBNull(i))
                            continue;
                        string propertyName = reader.GetName(i);
                        PropertyInfo property = entityType.GetProperty(propertyName);
                        property.SetValue(entity, reader.GetValue(i), null);
                    } // endFor

                    entityList.Add(entity);
                }

                if (indicator.CacheData && dataIndex == showPageLastIndex)
                {
                    break;
                }
                ++dataIndex;
            } // endWhile
            if (!indicator.CacheData)
            {
                indicator.DataCount = dataIndex;
                indicator.PageCount =
                    Convert.ToInt32(Math.Ceiling(Convert.ToDouble(indicator.DataCount) / indicator.PageSize));
            }

            return entityList;
        } // endBindDbDataReaderToObject

        #endregion

        #region 返回基础类型集合

        /// <summary>
        /// 从DbDataReader中读取数据置入实体类集合中
        /// </summary>
        /// <typeparam name="T">类类型</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns>实体类集合</returns>
        public List<BaseT> BuildObjectList<BaseT>(DbDataReader reader)
        {
            var objlList = new List<BaseT>();
            using (reader)
            {
                while (reader.Read())
                {
                    objlList.Add((BaseT)reader.GetValue(0));
                }
            }

            return objlList;
        }

        #endregion

        #region 执行Sql或DbCommand并返回DataTable

        public DataTable BuildDataTablePageTypedEntity(string commandText, IEntity entity, PageIndicatorInfo indicator)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return BuildDataTablePage(command, indicator);
        }

        /// <summary>
        /// (分页)从DbDataReader中读取数据置入DataTable中
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public DataTable BuildDataTablePage(DbDataReader reader, PageIndicatorInfo indicator)
        {
            DataTable dt = CreateTableBySchemaTable(reader.GetSchemaTable());
            var values = new object[reader.FieldCount];

            int showPageFirstIndex = (indicator.PageIndex - 1) * indicator.PageSize;
            int showPageLastIndex = indicator.PageIndex * indicator.PageSize;
            int dataIndex = 0;

            while (reader.Read())
            {
                if (dataIndex >= showPageFirstIndex && dataIndex <= showPageLastIndex)
                {
                    reader.GetValues(values);
                    dt.LoadDataRow(values, true);
                }

                if (indicator.CacheData && dataIndex == showPageLastIndex)
                {
                    break;
                }
                ++dataIndex;
            } // endWhile
            if (!indicator.CacheData)
            {
                indicator.DataCount = dataIndex;
                indicator.PageCount =
                    Convert.ToInt32(Math.Ceiling(Convert.ToDouble(indicator.DataCount) / indicator.PageSize));
            }

            return dt;
        } // endBindDbDataReaderToObject

        protected DataTable CreateTableBySchemaTable(DataTable schemaTable)
        {
            var dt = new DataTable();

            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                DataRow dr = schemaTable.Rows[i];
                var dc = new DataColumn(dr["ColumnName"].ToString(), (Type)dr["DataType"]);
                dt.Columns.Add(dc);
            }

            return dt;
        }

        /// <summary>
        /// 执行Sql并返回DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet BuildDataSet(string commandText)
        {
            return BuildDataSetTypedEntity(commandText, null);
        }

        /// <summary>
        /// 执行Sql并返回DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataSet BuildDataSetTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public DataSet BuildDataSetTypedParams(string commandText, params object[] variables)
        {
            DbCommand command = variables == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedParamArray(commandText, variables);

            return ExecuteDataSet(command);
        }

        /// <summary>
        /// 执行Sql并返回首个DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable BuildDataTableTypedEntity(string commandText, IEntity entity)
        {
            return BuildDataSetTypedEntity(commandText, entity).Tables[0];
        }

        /// <summary>
        /// 执行Sql并返回DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public DataTable BuildDataTableTypedParams(string commandText, params object[] variables)
        {
            DbCommand command = variables == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedParamArray(commandText, variables);

            return ExecuteDataSet(command).Tables[0];
        }

        #endregion

        #region 根据Sql语句/DbDataReader生成实体类对象

        /// <summary>
        /// 事务的执行Sql语句并返回实体类对象
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">Sql语句</param>
        /// <param name="variables">参数值变量(可为null)</param>
        /// <returns>实体类对象</returns>
        public T MakeEntityTypedParamArray(string commandText, object[] variables)
        {
            DbCommand command = CreateDbCommandTypedParamArray(commandText, variables);
            return MakeEntity(command);
        }

        /// <summary>
        /// 根据Sql语句与相应顺序的变量参数生成包含数据的实体类对象
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">Sql语句</param>
        /// <param name="variables">变量</param>
        /// <returns>包含数据的实体类对象</returns>
        public T MakeEntityTypedParams(string commandText, params object[] variables)
        {
            return MakeEntityTypedParamArray(commandText, variables);
        }

        /// <summary>
        /// 从DbDataReader中读取数据置入实体类中
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns>实体类数据</returns>
        public T MakeEntity(DbDataReader reader)
        {
            if (reader.Read())
            {
                Type entityType = typeof(T);
                var entity = new T();

                for (int i = 0; i < reader.FieldCount; ++i)
                {
                    if (reader.IsDBNull(i))
                        continue;

                    string propertyName = reader.GetName(i);
                    PropertyInfo property = entityType.GetProperty(propertyName);

                    if (property != null)
                    {
                        property.SetValue(entity, reader.GetValue(i), null);
                    }
                } // endFor

                return entity;
            } // endWhile

            return null;
        }

        /// <summary>
        /// 根据Sql语句与相实体类对象生成包含数据的实体类对象
        /// </summary>
        /// <typeparam name="T">实体类类型</typeparam>
        /// <param name="commandText">Sql语句</param>
        /// <param name="entity">包含参数数据的实体类对象</param>
        /// <returns>包含数据的实体类对象</returns>
        public T MakeEntityTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = CreateDbCommandTypedEntity(commandText, entity);
            return MakeEntity(command);
        }

        #endregion

        #region 执行Sql返回第一行第一列数据

        /// <summary>
        /// 返回第一行第一列的数据
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="entity">实体类对象</param>
        /// <returns>第一行第一列数据对象</returns>
        public object ExecuteScalarTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = CreateDbCommandTypedEntity(commandText, entity);
            return ExecuteScalar(command);
        }

        /// <summary>
        /// 根据参数列表返回第一行第一列的数据
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <returns>第一行第一列数据对象</returns>
        public object ExecuteScalar(string commandText)
        {
            DbCommand command = CreateDbCommand(commandText);

            return ExecuteScalar(command);
        }

        /// <summary>
        /// 根据参数列表返回第一行第一列的数据
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="parameters"></param>
        /// <returns>第一行第一列数据对象</returns>
        public object ExecuteScalarTypedParams(string commandText, params object[] parameters)
        {
            DbCommand command = CreateDbCommandTypedParamArray(commandText, parameters);

            return ExecuteScalar(command);
        }

        #endregion

        public event DbParameterBoundDelegate DbParameterBoundEvent;

        #region 开发辅助方法

        [System.Diagnostics.Conditional("DEBUG")]
        private void PrintCommand(DbCommand command)
        {
            Debug.WriteLine("-----------NRainel-Print-Start-----------");
            Debug.WriteLine("DbCommand:");
            Debug.WriteLine(command.CommandText);
            Debug.WriteLine("CommandParams:");
            for (int i = 0; i < command.Parameters.Count; i++)
            {
                Debug.WriteLine(command.Parameters[i].ParameterName + ":" + command.Parameters[i].Value);
            }
            Debug.WriteLine("-----------NRainel-Print-End-------------");
        }

        #endregion
    }
}