using System;
using System.Collections.Generic;
using System.Data.Common;
using Homeinns.Common.UI.Common;
using System.Data;
using Homeinns.Common.Entity.Text;
using Homeinns.Common.Entity.Association;

namespace Homeinns.Common.Entity.Common
{
    public abstract class DbServic<T> where T : class,IEntity, new()
    {
        private DbCommandClause<T> _clause;
        private DatabaseObject<T> _databaseObject;

        /// <summary>
        /// 数据库映射对象
        /// </summary>
        protected DatabaseObject<T> DBO
        {
            get
            {
                if (_databaseObject == null)
                {
                    throw new NullReferenceException("数据库映射对象为空！");
                }
                return _databaseObject;
            }
            set { _databaseObject = value; }
        }


        /// <summary>
        /// 语句生成
        /// </summary>
        public DbCommandClause<T> Clause
        {
            get
            {
                if (_clause == null)
                {
                    _clause = new DbCommandClause<T>(DBO.DBSchema);
                }

                return _clause;
            }
        }

        /// <summary>
        /// 返回已打开的数据库连接
        /// </summary>
        /// <returns></returns>
        public DbConnection BeginConnection()
        {
            return DBO.BeginConnection();
        }

        /// <summary>
        /// 设置数据库连接对象
        /// </summary>
        /// <param name="conn"></param>
        /// <returns>DbServic对象本身</returns>
        public DbServic<T> SetConnection(DbConnection conn)
        {
            DBO.SetConnection(conn);
            return this;
        }

        /// <summary>
        /// 设置事务对象
        /// </summary>
        /// <param name="tran"></param>
        /// <returns>DbServic对象本身</returns>
        public DbServic<T> SetTransaction(DbTransaction tran)
        {
            DBO.SetTransaction(tran);
            return this;
        }


        /// <summary>
        /// command加parameters
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameters"></param>
        private DbCommand AddCommandParameters(DbCommand command, IEnumerable<DbParameter> parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        #region 新增

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">包含数据的实体对象</param>
        /// <returns>是否插入成功</returns>
        public bool Add(T entity)
        {
            string commandText = Clause.ToInsert(entity);

            return DBO.ExecuteBoolTypedEntity(commandText, entity);
        }

        #endregion

        #region 修改

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="editData"></param>
        /// <param name="clip"></param>
        /// <returns></returns>
        public bool Modify(EditData<T> editData, T clip)
        {
            string commandText = Clause.ToUpdate(editData);
            string whereCommandText = Clause.ToWherePropertysValue(clip);
            DbCommand cmd = DBO.CreateDbCommandTypedEntity(commandText, editData.Entity);
            DbCommand cmdWhere = DBO.CreateDbCommandTypedEntity(whereCommandText, clip);
            cmd.CommandText += whereCommandText;

            foreach (DbParameter param in cmdWhere.Parameters)
            {
                DbParameter newParam = DBO.DBSchema.ProviderFactory.CreateParameter();
                newParam.ParameterName = param.ParameterName;
                newParam.Value = param.Value;
                cmd.Parameters.Add(newParam);
            }

            return DBO.ExecuteBool(cmd);
        }

        /// <summary>
        /// 根据主键修改
        /// </summary>
        /// <param name="editData"></param>
        /// <param name="primaryKeyValues"></param>
        /// <returns></returns>
        public bool ModifyByPrimaryKey(EditData<T> editData, params object[] primaryKeyValues)
        {
            return ModifyByPrimaryKeyArray(editData, primaryKeyValues);
        }

        /// <summary>
        /// 根据主键修改
        /// </summary>
        /// <param name="editData"></param>
        /// <param name="primaryKeyValues"></param>
        /// <returns></returns>
        public bool ModifyByPrimaryKeyArray(EditData<T> editData, object[] primaryKeyValues)
        {
            string commandText = Clause.ToUpdate(editData);
            string whereCommandText = Clause.ToWherePrimaryKey();
            DbCommand cmd = DBO.CreateDbCommandTypedEntity(commandText, editData.Entity);
            DbCommand cmdWhere = DBO.CreateDbCommandTypedParamArray(whereCommandText, primaryKeyValues);
            cmd.CommandText += whereCommandText;

            foreach (DbParameter param in cmdWhere.Parameters)
            {
                DbParameter newParam = DBO.DBSchema.ProviderFactory.CreateParameter();
                newParam.ParameterName = param.ParameterName;
                newParam.Value = param.Value;
                cmd.Parameters.Add(newParam);
            }

            return DBO.ExecuteBool(cmd);
        }

        /// <summary>
        /// 无条件修改全部
        /// </summary>
        /// <param name="editData"></param>
        /// <param name="clip"></param>
        /// <returns></returns>
        public bool ModifyALL(EditData<T> editData)
        {
            string commandText = Clause.ToUpdate(editData);
            DbCommand cmd = DBO.CreateDbCommandTypedEntity(commandText, editData.Entity);

            return DBO.ExecuteBool(cmd);
        }


        #endregion

        #region 得到数据对象

        /// <summary>
        /// 得到数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Get(T entity)
        {
            string commandText = Clause.ToSelectHardColumns();
            commandText += Clause.ToWherePropertysValue(entity);

            return _databaseObject.MakeEntityTypedEntity(commandText, entity);
        }

        /// <summary>
        /// 得到所有列数据包括惰性列
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T GetFull(T entity)
        {
            string commandText = Clause.ToSelectFullColumns();
            commandText += Clause.ToWherePropertysValue(entity);

            return _databaseObject.MakeEntityTypedEntity(commandText, entity);
        }

        /// <summary>
        /// 根据主键得到数据
        /// </summary>
        /// <returns></returns>
        public T GetByPrimaryKey(params object[] primaryKeyValues)
        {
            return GetByPrimaryKeyArray(primaryKeyValues);
        }


        /// <summary>
        /// 根据主键列表得到数据
        /// </summary>
        /// <param name="primaryKeyValues"></param>
        /// <returns></returns>
        public T GetByPrimaryKeyArray(object[] primaryKeyValues)
        {
            string commandText = Clause.ToSelectHardColumns();
            commandText += Clause.ToWherePrimaryKey();

            return _databaseObject.MakeEntityTypedParamArray(commandText, primaryKeyValues);
        }

        /// <summary>
        /// 根据主键得到所有数据包括惰性列
        /// </summary>
        /// <param name="primaryKeyValues"></param>
        /// <returns></returns>
        public T GetFullByPrimaryKey(params object[] primaryKeyValues)
        {
            return GetFullByPrimaryKeyArray(primaryKeyValues);
        }

        /// <summary>
        /// 根据主键得到所有数据包括惰性列
        /// </summary>
        /// <param name="primaryKeyValues"></param>
        /// <returns></returns>
        public T GetFullByPrimaryKeyArray(object[] primaryKeyValues)
        {
            string commandText = Clause.ToSelectFullColumns();
            commandText += Clause.ToWherePrimaryKey();

            return _databaseObject.MakeEntityTypedParamArray(commandText, primaryKeyValues);
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            string deleteClause = Clause.ToDelete();
            if (entity != null)
                deleteClause += Clause.ToWherePropertysValue(entity);


            return DBO.ExecuteBoolTypedEntity(deleteClause, entity);
        }

        /// <summary>
        /// 根据复合主键删除
        /// </summary>
        /// <returns></returns>
        public bool DeleteByPrimaryKey(params object[] primaryKeyValues)
        {
            return DeleteByPrimaryKeyArray(primaryKeyValues);
        }

        /// <summary>
        /// 根据复合主键列表删除
        /// </summary>
        /// <param name="primaryKeyValues"></param>
        /// <returns></returns>
        public bool DeleteByPrimaryKeyArray(object[] primaryKeyValues)
        {
            string deleteClause = Clause.ToDelete();
            deleteClause += Clause.ToWherePrimaryKey();

            return _databaseObject.ExecuteBoolTypedArray(deleteClause, primaryKeyValues);
        }

        /// <summary>
        /// 删除单主键In多个主键值
        /// </summary>
        /// <returns></returns>
        public bool DeleteIn(List<object> primaryKeyValueList)
        {
            string commandText = Clause.ToDelete();
            commandText += Clause.ToWhereIn(primaryKeyValueList.Count);

            return DBO.ExecuteBoolTypedParams(commandText, primaryKeyValueList.ToArray());
        }

        #endregion

        #region 得到数据对象组

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<T> GetList(T entity)
        {
            string commandText = Clause.ToSelectHardColumns();
            if (entity != null)
                commandText += Clause.ToWherePropertysValue(entity);

            return DBO.BuildEntityListTypedEntity(commandText, entity);
        }

        /// <summary>
        /// 得到所有列数据列表包括惰性列
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<T> GetFullList(T entity)
        {
            string commandText = Clause.ToSelectFullColumns();
            commandText += Clause.ToWherePropertysValue(entity);

            return DBO.BuildEntityListTypedEntity(commandText, entity);
        }

        #endregion

        #region 得到数据表对象

        /// <summary>
        /// 查询表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataSet GetGetDataSet(T entity)
        {
            string commandText = Clause.ToSelectHardColumns();
            if (entity != null)
                commandText += Clause.ToWherePropertysValue(entity);

            return DBO.BuildDataSetTypedEntity(commandText, entity);
        }

        /// <summary>
        /// 得到所有列数据表包括惰性列
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataSet GetFullDataSet(T entity)
        {
            string commandText = Clause.ToSelectFullColumns();
            commandText += Clause.ToWherePropertysValue(entity);

            return DBO.BuildDataSetTypedEntity(commandText, entity);
        }

        /// <summary>
        /// 查询表
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable GetTable(T entity)
        {
            string commandText = Clause.ToSelectHardColumns();
            if (entity != null)
                commandText += Clause.ToWherePropertysValue(entity);

            return DBO.BuildDataTableTypedEntity(commandText, entity);
        }

        /// <summary>
        /// 得到所有列数据表包括惰性列
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public DataTable GetFullTable(T entity)
        {
            string commandText = Clause.ToSelectFullColumns();
            commandText += Clause.ToWherePropertysValue(entity);

            return DBO.BuildDataTableTypedEntity(commandText, entity);
        }
        #endregion

        #region 统计数据

        /// <summary>
        /// 统计符合条件数据数量
        /// </summary>
        /// <returns></returns>
        public int Count(T entity)
        {
            string commandText = Clause.ToCountAll();
            commandText += Clause.ToWherePropertysValue(entity);
            object obj = DBO.ExecuteScalarTypedEntity(commandText, entity);
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 是否有符合条件数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists(T entity)
        {
            return Count(entity) != 0;
            //string commandText = Clause.ToExistsAll();
            //commandText += Clause.ToWherePropertysValue(entity);

            //return DBO.ExecuteScalarTypedEntity(commandText, entity) != null;
        }

        #endregion
    }
}