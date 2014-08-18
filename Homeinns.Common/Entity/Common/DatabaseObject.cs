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
    /// ���ݷ��ʲ������Զ�����
    /// </summary>
    public class DatabaseObject<T> where T : class, new()
    {
        /// <summary>
        /// Sql��������
        /// </summary>
        private readonly CommandTextAnalyser _CommandStringAnalyser = new CommandTextAnalyser();

        /// <summary>
        /// ���ݿ�������� (�����б����ݿ��������б������еı���/����/��������/���ݳ���/�Ƿ�Ϊ�յĵ�����)
        /// </summary>
        private readonly DatabaseSchema _dbSchema;

        private DbConnection _conn;

        /// <summary>
        /// �Ƿ����ڲ���������
        /// </summary>
        private bool _innerConnection;

        private DbTransaction _tran;

        public DatabaseObject(DatabaseSchema dbSchema)
        {
            _dbSchema = dbSchema;
            SetCreateConnectionFun(CreateConnectionFun);
        }

        /// <summary>
        /// ���ݿ�ṹ
        /// </summary>
        public DatabaseSchema DBSchema
        {
            get { return _dbSchema; }
        }

        #region ��ǰ���������Ӳ���

        /// <summary>
        /// ��������
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
        /// �����Ѵ򿪵����ݿ�����
        /// </summary>
        /// <returns></returns>
        public DbConnection BeginConnection()
        {
            StartOperation();
            _innerConnection = false;
            return _conn;
        }

        /// <summary>
        /// �������ݿ����Ӷ���
        /// </summary>
        /// <param name="conn"></param>
        public void SetConnection(DbConnection conn)
        {
            _conn = conn;
            _innerConnection = false;
        }

        /// <summary>
        /// �����������
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
        /// ���ݲ�����ʼ
        /// </summary>
        private void StartOperation()
        {
            if (_conn == null || _conn.State == ConnectionState.Closed)
            {
                //_conn = DBSchema.ProviderFactory.CreateConnection();
                if (createConnDel == null)
                {
                    throw new Exception("ί��ʵ����û�д���DbConnection�ķ���!");
                }
                _conn = createConnDel();
                _conn.ConnectionString = DBSchema.ConnectionString;
                _innerConnection = true;
                _conn.Open();
            }
        }

        /// <summary>
        /// ���ݲ�������
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
        /// ��DbCommand��Connection/Transaction
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
        /// �ع��ɻع�������
        /// </summary>
        private void RollbackTransaction()
        {
            if (Transaction != null)
            {
                _tran.Rollback();
            }
        }

        #endregion

        #region ����Sql�������DbCommand����

        public DbCommand CreateDbCommand(string commandText)
        {
            DbCommand command = DBSchema.ProviderFactory.CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// ����Sql�������Ӧ˳�����еı�������DbCommand����
        /// </summary>
        /// <param name="commandText">Sql���</param>
        /// <param name="variables">����</param>
        /// <returns>DbCommand����</returns>
        public DbCommand CreateDbCommandTypedParams(string commandText, params object[] variables)
        {
            return CreateDbCommandTypedParamArray(commandText, variables);
        }

        /// <summary>
        /// ����DbCommand�ַ�����ʵ�����������dbCommand
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
                // �����󶨺��¼�����
                DbParameterBoundEventFunction(columnSchema, parameter);
                command.Parameters.Add(parameter);
            }

            return command;
        }


        /// <summary>
        /// ����Sql����ַ���������б�������Ӧ��DbCommand����
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
                // �����󶨺��¼�����
                DbParameterBoundEventFunction(columnSchema, parameter);
                command.Parameters.Add(parameter);
            }

            return command;
        }


        /// <summary>
        /// Ϊdelete in �������Command����
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
                // �󶨺��¼�����
                DbParameterBoundEventFunction(columnSchema, parameter);
                command.Parameters.Add(parameter);
            }

            return command;
        }

        /// <summary>
        /// ����DbParameter����
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
        /// ���������б�
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
        /// Parameter�󶨺��¼�����
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

        #region ִ��DbCommand

        /// <summary>
        /// �������DataSet
        /// </summary>
        /// <param name="command">DbCommand����</param>
        /// <param name="ds">DataSet����</param>
        /// <param name="tableName">����(�ɿ�,����ֻ��һ��DataTable)</param>
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
        /// ������ִ��DbCommand������DataSet
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
        /// ִ��DbCommand����Ӱ������
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
        /// ִ��DbCommand����DbReader
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
        /// ִ��DbCommand�����ص�һ�е�һ������
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
        /// ִ��Sql��䲢�����Ƿ�ִ�гɹ�
        /// </summary>
        /// <param name="command">Sql����ַ���</param>
        /// <returns>�Ƿ�ִ�гɹ�</returns>
        public bool ExecuteBool(DbCommand command)
        {
            return ExecuteNonQuery(command) != 0;
        }

        /// <summary>
        /// ִ��DbCommand���󲢷�����Ӧ��ʵ���෺�ͼ���
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="command">DbCommand����</param>
        /// <returns>ʵ���෺�ͼ���</returns>
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
        /// (��ҳ)ִ��DbCommand���󲢷�����Ӧ��ʵ���෺�ͼ���
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="command">DbCommand����</param>
        /// <param name="indicator"></param>
        /// <returns>ʵ���෺�ͼ���</returns>
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
        /// ����DbCommand���ػ����������ͷ�������
        /// </summary>
        /// <typeparam name="T">������������</typeparam>
        /// <param name="command">DbCommand</param>
        /// <returns>�����������ͷ�������</returns>
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
        /// (��ҳ)ִ��DbCommand���󲢷�����Ӧ��DataTable
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
        /// ����DbCommand��������ʵ�������
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="command">DbCommand����</param>
        /// <returns>ʵ�������</returns>
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

        #region ִ��Sql��䲢������Ӱ������

        /// <summary>
        /// ִ��Sql��䲢������Ӱ�������
        /// </summary>
        /// <param name="commandText">Sql����ַ���</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecuteInt(string commandText)
        {
            DbCommand command = CreateDbCommand(commandText);
            return ExecuteNonQuery(command);
        }

        /// <summary>
        /// �����ִ��Sql����Ӱ������
        /// </summary>
        /// <param name="commandText">Sql���</param>
        /// <param name="entity">�����������ݵ�ʵ�������</param>
        /// <returns>Ӱ������</returns>
        public int ExecuteIntTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return ExecuteNonQuery(command);
        }

        /// <summary>
        /// ִ��Sql��䲢������Ӱ�������
        /// </summary>
        /// <param name="commandText">Sql����ַ���</param>
        /// <param name="variables">����ֵ</param>
        /// <returns>��Ӱ�������</returns>
        public int ExecuteIntTypedParams(string commandText, object[] variables)
        {
            DbCommand command = variables == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedParamArray(commandText, variables);

            return ExecuteNonQuery(command);
        }

        #endregion

        #region ִ��Sq��䲢�����Ƿ�����Ӱ������

        /// <summary>
        /// ִ��Sql��䲢�����Ƿ�ִ�гɹ�
        /// </summary>
        /// <returns>�Ƿ�ִ�гɹ�</returns>
        public bool ExecuteBool(string commandText)
        {
            DbCommand command = CreateDbCommand(commandText);

            return ExecuteBool(command);
        }

        /// <summary>
        /// ִ��Sql��䲢�����Ƿ�����Ӱ������
        /// </summary>
        /// <param name="commandText">Sql����ַ���</param>
        /// <param name="varibles">���������б�</param>
        /// <returns>�����Ƿ�����Ӱ������</returns>
        public bool ExecuteBoolTypedArray(string commandText, object[] varibles)
        {
            return ExecuteIntTypedParams(commandText, varibles) != 0;
        }

        /// <summary>
        /// ִ��Sql��䲢�����Ƿ�����Ӱ������
        /// </summary>
        /// <param name="commandText">Sql����ַ���</param>
        /// <param name="varibles">���������б�</param>
        /// <returns>�����Ƿ�����Ӱ������</returns>
        public bool ExecuteBoolTypedParams(string commandText, params object[] varibles)
        {
            return ExecuteBoolTypedArray(commandText, varibles);
        }


        /// <summary>
        /// ִ��Sql��䲢�����Ƿ�ִ�гɹ�
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="entity">�������ݵ�ʵ�������</param>
        /// <returns>�Ƿ�ִ�гɹ�</returns>
        public bool ExecuteBoolTypedEntity(string commandText, IEntity entity)
        {
            return ExecuteIntTypedEntity(commandText, entity) != 0;
        }

        #endregion

        #region ����Sql����DbDataReader���ɷ��ͼ���

        /// <summary>
        /// ����û�в�����DbCommand�ַ�������ʵ���෺�ͼ���
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="commandText">DbCommand�ַ���</param>
        /// <returns>ʵ���෺�ͼ���</returns>
        public List<T> BuildEntityList(string commandText)
        {
            return BuildEntityListTypedEntity(commandText, null);
        }

        /// <summary>
        /// ����DbCommand�ַ�����ʵ�����������ʵ���෺�ͼ���
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="commandText">DbCommand���</param>
        /// <param name="entity">ʵ�������</param>
        /// <returns>ʵ���෺�ͼ���</returns>
        public List<T> BuildEntityListTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return BuildEntityList(command);
        }

        /// <summary>
        /// ����DbCommand�ַ�����ʵ�����������ʵ���෺�ͼ���
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="commandText">DbCommand���</param>
        /// <param name="variables">���������б�</param>
        /// <returns>ʵ���෺�ͼ���</returns>
        public List<T> BuildEntityListTypedParams(string commandText, params object[] variables)
        {
            DbCommand command = variables == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedParamArray(commandText, variables);

            return BuildEntityList(command);
        }


        /// <summary>
        /// ��DbDataReader�ж�ȡ�������뷺���༯����
        /// </summary>
        /// <typeparam name="T">Object����</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns>ʵ���༯��</returns>
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
        /// (��ҳ)����DbCommand�ַ�����ʵ�����������ʵ���෺�ͼ���
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="commandText">DbCommand���</param>
        /// <param name="entity">ʵ�������</param>
        /// <param name="indicator">��ҳ����</param>
        /// <returns>ʵ���෺�ͼ���</returns>
        public List<T> BuildEntityListPageTypedEntity(string commandText, IEntity entity, PageIndicatorInfo indicator)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return BuildEntityListPage(command, indicator);
        }


        /// <summary>
        /// (��ҳ)��DbDataReader�ж�ȡ�������뷺���༯����
        /// </summary>
        /// <typeparam name="T">Object����</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <param name="indicator"></param>
        /// <returns>ʵ���༯��</returns>
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

        #region ���ػ������ͼ���

        /// <summary>
        /// ��DbDataReader�ж�ȡ��������ʵ���༯����
        /// </summary>
        /// <typeparam name="T">������</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns>ʵ���༯��</returns>
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

        #region ִ��Sql��DbCommand������DataTable

        public DataTable BuildDataTablePageTypedEntity(string commandText, IEntity entity, PageIndicatorInfo indicator)
        {
            DbCommand command = entity == null
                                    ? CreateDbCommand(commandText)
                                    : CreateDbCommandTypedEntity(commandText, entity);

            return BuildDataTablePage(command, indicator);
        }

        /// <summary>
        /// (��ҳ)��DbDataReader�ж�ȡ��������DataTable��
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
        /// ִ��Sql������DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet BuildDataSet(string commandText)
        {
            return BuildDataSetTypedEntity(commandText, null);
        }

        /// <summary>
        /// ִ��Sql������DataSet
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
        /// ִ��Sql�������׸�DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable BuildDataTableTypedEntity(string commandText, IEntity entity)
        {
            return BuildDataSetTypedEntity(commandText, entity).Tables[0];
        }

        /// <summary>
        /// ִ��Sql������DataTable
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

        #region ����Sql���/DbDataReader����ʵ�������

        /// <summary>
        /// �����ִ��Sql��䲢����ʵ�������
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="commandText">Sql���</param>
        /// <param name="variables">����ֵ����(��Ϊnull)</param>
        /// <returns>ʵ�������</returns>
        public T MakeEntityTypedParamArray(string commandText, object[] variables)
        {
            DbCommand command = CreateDbCommandTypedParamArray(commandText, variables);
            return MakeEntity(command);
        }

        /// <summary>
        /// ����Sql�������Ӧ˳��ı����������ɰ������ݵ�ʵ�������
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="commandText">Sql���</param>
        /// <param name="variables">����</param>
        /// <returns>�������ݵ�ʵ�������</returns>
        public T MakeEntityTypedParams(string commandText, params object[] variables)
        {
            return MakeEntityTypedParamArray(commandText, variables);
        }

        /// <summary>
        /// ��DbDataReader�ж�ȡ��������ʵ������
        /// </summary>
        /// <typeparam name="T">ʵ����</typeparam>
        /// <param name="reader">DbDataReader</param>
        /// <returns>ʵ��������</returns>
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
        /// ����Sql�������ʵ����������ɰ������ݵ�ʵ�������
        /// </summary>
        /// <typeparam name="T">ʵ��������</typeparam>
        /// <param name="commandText">Sql���</param>
        /// <param name="entity">�����������ݵ�ʵ�������</param>
        /// <returns>�������ݵ�ʵ�������</returns>
        public T MakeEntityTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = CreateDbCommandTypedEntity(commandText, entity);
            return MakeEntity(command);
        }

        #endregion

        #region ִ��Sql���ص�һ�е�һ������

        /// <summary>
        /// ���ص�һ�е�һ�е�����
        /// </summary>
        /// <param name="commandText">sql���</param>
        /// <param name="entity">ʵ�������</param>
        /// <returns>��һ�е�һ�����ݶ���</returns>
        public object ExecuteScalarTypedEntity(string commandText, IEntity entity)
        {
            DbCommand command = CreateDbCommandTypedEntity(commandText, entity);
            return ExecuteScalar(command);
        }

        /// <summary>
        /// ���ݲ����б��ص�һ�е�һ�е�����
        /// </summary>
        /// <param name="commandText">sql���</param>
        /// <returns>��һ�е�һ�����ݶ���</returns>
        public object ExecuteScalar(string commandText)
        {
            DbCommand command = CreateDbCommand(commandText);

            return ExecuteScalar(command);
        }

        /// <summary>
        /// ���ݲ����б��ص�һ�е�һ�е�����
        /// </summary>
        /// <param name="commandText">sql���</param>
        /// <param name="parameters"></param>
        /// <returns>��һ�е�һ�����ݶ���</returns>
        public object ExecuteScalarTypedParams(string commandText, params object[] parameters)
        {
            DbCommand command = CreateDbCommandTypedParamArray(commandText, parameters);

            return ExecuteScalar(command);
        }

        #endregion

        public event DbParameterBoundDelegate DbParameterBoundEvent;

        #region ������������

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