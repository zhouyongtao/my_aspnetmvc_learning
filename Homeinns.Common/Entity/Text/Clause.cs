using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Homeinns.Common.UI.Common;
using Homeinns.Common.Entity.Schema;
using Homeinns.Common.Entity.Association;

namespace Homeinns.Common.Entity.Text
{
    public abstract class Clause<T> where T : new()
    {
        private string _capitalTableName;
        protected CommandTextAnalyser _CommandTextAnalyser = new CommandTextAnalyser();
        private string _entityName;
        private PropertyInfo[] _entityPropertis;

        private Type _entityType;

        /// <summary>
        /// 实体类类型
        /// </summary>
        public Type EntityType
        {
            get
            {
                if (_entityType == null)
                {
                    _entityType = typeof(T);
                }

                return _entityType;
            }
        }

        /// <summary>
        /// 实体类名
        /// </summary>
        public string EntityName
        {
            get
            {
                if (_entityName == null)
                {
                    EntityAssociationAttribute attribute = Attribute.GetCustomAttribute(EntityType, typeof(EntityAssociationAttribute)) as EntityAssociationAttribute;
                    if (attribute != null)
                    { 
                        _entityName = attribute.TableName; 
                    }
                    else
                    {
                        _entityName = EntityType.Name;
                    }
                }

                return _entityName;
            }
        }

        /// <summary>
        /// 大写表名
        /// </summary>
        public string CapitalTableName
        {
            get
            {
                if (_capitalTableName == null)
                {
                    _capitalTableName = EntityName.ToUpper();
                }

                return _capitalTableName;
            }
        }

        /// <summary>
        /// 实体属性集合
        /// </summary>
        public PropertyInfo[] EntityProperties
        {
            get
            {
                if (_entityPropertis == null)
                {
                    _entityPropertis = EntityType.GetProperties();
                }

                return _entityPropertis;
            }
        }

        /// <summary>
        /// 创建insert
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected string CreateInsert(List<string> columns, IEntity entity)
        {
            string addCmdStr = "INSERT INTO [{0}] ({1}) VALUES({2})";
            var paramList = new List<string>();
            PropertyInfo property;

            foreach (string column in columns)
            {
                property = _entityType.GetProperty(column);

                if (property == null)
                    throw new NullReferenceException("实体类" + EntityName + "中缺少" + column + "字段");

                if (property.GetValue(entity, null) != null)
                    paramList.Add(column);
            }

            string[] paramArray = paramList.ToArray();
            string columnsString = "[" + string.Join("],[", paramArray) + "]";
            string paramsString = "@" + string.Join(",@", paramArray);

            return string.Format(addCmdStr, EntityName, columnsString, paramsString);
        }

        /// <summary>
        /// 创建Update 根据列列表
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="editPropertys"></param>
        /// <returns></returns>
        protected string CreateUpdate(List<string> columns, List<string> editPropertys)
        {
            string updateClause = "UPDATE [{0}] SET {1} ";
            var columnSB = new StringBuilder();
            foreach (string columnName in columns)
            {
                if (editPropertys.Contains(columnName))
                {
                    columnSB.Append("[")
                        .Append(columnName)
                        .Append("] = @")
                        .Append(columnName)
                        .Append(" , ");
                }
            }

            if (columnSB.Length > 3)
                columnSB = columnSB.Remove(columnSB.Length - 3, 3);

            return string.Format(updateClause, EntityName, columnSB);
        }

        /// <summary>
        /// 创建Update 根据非空属性
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected string CreateUpdate(List<string> columns, T entity)
        {
            string updateClause = "UPDATE [{0}] SET {1} ";
            PropertyInfo proerty;
            var columnSB = new StringBuilder();
            foreach (string columnName in columns)
            {
                proerty = _entityType.GetProperty(columnName);
                if (proerty != null && proerty.GetValue(entity, null) != null)
                {
                    columnSB.Append("[")
                        .Append(columnName)
                        .Append("] = @")
                        .Append(columnName)
                        .Append(" , ");
                }
            }

            if (columnSB.Length > 3)
                columnSB = columnSB.Remove(columnSB.Length - 3, 3);

            return string.Format(updateClause, EntityName, columnSB);
        }

        /// <summary>
        /// 创建Update 根据非空属性
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="editData"></param>
        /// <returns></returns>
        protected string CreateUpdate(List<string> columns, EditData<T> editData)
        {
            string updateClause = "UPDATE [{0}] SET {1} ";
            PropertyInfo proerty;
            var columnSB = new StringBuilder();
            foreach (string columnName in columns)
            {
                if (!editData.EditPropertys.Contains(columnName))
                {
                    proerty = _entityType.GetProperty(columnName);
                    if (proerty == null || proerty.GetValue(editData.Entity, null) == null)
                        continue;
                }

                columnSB.Append("[")
                    .Append(columnName)
                    .Append("] = @")
                    .Append(columnName)
                    .Append(" , ");
            }

            if (columnSB.Length > 3)
                columnSB = columnSB.Remove(columnSB.Length - 3, 3);

            return string.Format(updateClause, EntityName, columnSB);
        }

        /// <summary>
        /// 创建Count(*)语句
        /// </summary>
        /// <returns></returns>
        protected string CreateCountAll()
        {
            string countClause = " SELECT COUNT(*) FROM [{0}] ";
            return string.Format(countClause, EntityName);
        }

        /// <summary>
        /// (废弃)创建select count(*) from  (SELECT TOP 1 * FROM [{0}]) as [TempTable] 语句
        /// </summary>
        /// <returns></returns>
        protected string CreateExists()
        {
            const string existsClause = " select count(*) from  (SELECT TOP 1 * FROM [{0}]) as [TempTable] ";
            //string existsClause = " SELECT COUNT(*) FROM [{0}] ";
            return string.Format(existsClause, EntityName);
        }

        /// <summary>
        /// 创建删除
        /// </summary>
        /// <returns></returns>
        protected string CreateDelete()
        {
            return String.Format("DELETE FROM [{0}] ", EntityName);
        }

        /// <summary>
        /// 创建Where
        /// </summary>
        /// <param name="primaryKeyColumns"></param>
        /// <returns></returns>
        protected string CreateWhere(List<string> primaryKeyColumns)
        {
            if (primaryKeyColumns == null || primaryKeyColumns.Count == 0)
            {
                throw new NullReferenceException("此表主键为空或者主键名不包含PK_,无法使用此方法.");
            }

            var pkColumnNamesSB = new StringBuilder(" WHERE ");

            foreach (string pkColumnNames in primaryKeyColumns)
            {
                pkColumnNamesSB.Append("[")
                    .Append(pkColumnNames)
                    .Append("] = @")
                    .Append(pkColumnNames)
                    .Append(" AND ");
            }

            if (pkColumnNamesSB.Length >= 5)
                pkColumnNamesSB = pkColumnNamesSB.Remove(pkColumnNamesSB.Length - 5, 5);

            return pkColumnNamesSB.ToString();
        }

        /// <summary>
        /// 根据属性值创建Where
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected string CreateWherePropertysValue(IEntity entity, TableSchema tableSchema)
        {
            var sb = new StringBuilder(" WHERE ");
            string keyName;

            foreach (PropertyInfo property in EntityProperties)
            {
                if (tableSchema.ColumnDict.ContainsKey(property.Name.ToUpper()))// 2010-12-24
                {
                    if (property.GetValue(entity, null) != null)
                    {
                        keyName = property.Name;
                        sb.Append("[")
                            .Append(keyName)
                            .Append("] = @")
                            .Append(keyName)
                            .Append(" AND ");
                    }
                }
            }

            if (sb.Length > 12)
                sb.Remove(sb.Length - 4, 4);
            else
                //throw new NullReferenceException("没有任何值以生成查询条件.");
                sb = new StringBuilder("");

            return sb.ToString();
        }

        /// <summary>
        /// 创建非惰性列的Select
        /// </summary>
        /// <returns></returns>
        protected string CreateSelectColumns(string columnsString)
        {
            return "SELECT " + columnsString + " FROM " + EntityName;
        }


        /// <summary>
        /// 创建删除条件语句
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="conditionCount"></param>
        /// <returns></returns>
        public string CreateWhereIn(string columnName, int conditionCount)
        {
            var inSB = new StringBuilder();
            inSB.Append(" WHERE [")
                .Append(columnName)
                .Append("] IN (");

            for (int i = 0; i < conditionCount; ++i)
            {
                if (i != 0)
                    inSB.Append(",");
                inSB.Append("@").Append("TempConditionParameter").Append(i);
            }

            return inSB.Append(")").ToString();
        }


        /// <summary>
        /// 创建In语句
        /// </summary>
        /// <param name="conditionCount"></param>
        /// <returns></returns>
        protected string CreateIn(int conditionCount)
        {
            var inSB = new StringBuilder();
            inSB.Append(" IN (");

            for (int i = 0; i < conditionCount; ++i)
            {
                if (i != 0)
                    inSB.Append(",");
                inSB.Append("@").Append("TempConditionParameter").Append(i);
            }

            return inSB.Append(")").ToString();
        }


        /// <summary>
        /// 拼接有值的And子句
        /// </summary>
        /// <param name="originClause"></param>
        /// <param name="entity">实体类对象</param>
        /// <returns>拼接完成的And子句</returns>
        public string ProcessAndClause(string originClause, IEntity entity)
        {
            PropertyInfo property;
            string[] parameters = _CommandTextAnalyser.GetSqlParameterNamesForWhere(originClause);
            string[] whereClauses = originClause.Split(new[] { " and ", " AND " }, StringSplitOptions.None);
            string whereClauseResult = string.Empty;
            object value;

            for (int i = 0; i < whereClauses.Length; ++i)
            {
                if (whereClauses[i].IndexOf('@') != -1)
                {
                    property = EntityType.GetProperty(parameters[i]);
                    if (property == null)
                        continue;

                    value = property.GetValue(entity, null);
                    if (value == null || value.ToString().Length == 0)
                        continue;
                }

                whereClauseResult = whereClauseResult + " AND " + whereClauses[i];
            }

            return whereClauseResult;
        }


        /// <summary>
        /// 创建Where语句
        /// </summary>
        /// <param name="originClause"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string ProcessWhereClause(string originClause, IEntity entity)
        {
            string whereClause = ProcessAndClause(originClause, entity);

            if (!string.IsNullOrEmpty(whereClause))
            {
                whereClause = " WHERE " + whereClause.Remove(0, 4);
            }

            return whereClause;
        }
    }
}