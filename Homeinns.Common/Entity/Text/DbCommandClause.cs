
using Homeinns.Common.UI.Common;
using Homeinns.Common.Entity.Schema;
using Homeinns.Common.Entity.Association;


namespace Homeinns.Common.Entity.Text
{
    public class DbCommandClause<T> : Clause<T> where T : new()
    {
        private readonly DatabaseSchema _DBSchema;

        public DbCommandClause(DatabaseSchema sqlDatabaseSchema)
        {
            _DBSchema = sqlDatabaseSchema;
        }


        /// <summary>
        /// 生成新增Sql
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string ToInsert(IEntity entity)
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];

            return CreateInsert(tableSchema.Columns, entity);
        }


        /// <summary>
        /// 生成更新语句 根据编辑列列表 和 实体非空属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string ToUpdate(EditData<T> editData)
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];

            return CreateUpdate(tableSchema.Columns, editData);
        }

        /// <summary>
        /// 生成更新语句 根据实体非空属性
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string ToUpdate(T entity)
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];

            return CreateUpdate(tableSchema.Columns, entity);
        }

        /// <summary>
        /// 根据主键生成Where语句
        /// </summary>
        /// <returns></returns>
        public string ToWherePrimaryKey()
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];

            return CreateWhere(tableSchema.PrimaryKeyColumns);
        }

        /// <summary>
        /// 根据有值属性生成Where语句
        /// </summary>
        /// <returns></returns>
        public string ToWherePropertysValue(IEntity entity)
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];
            return CreateWherePropertysValue(entity,tableSchema);
        }


        /// <summary>
        /// 得到非惰性列Select
        /// </summary>
        /// <returns></returns>
        public string ToSelectHardColumns()
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];

            return CreateSelectColumns(tableSchema.HardColumnsString);
        }

        /// <summary>
        /// 得到所有列Select
        /// </summary>
        /// <returns></returns>
        public string ToSelectFullColumns()
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];

            return CreateSelectColumns(tableSchema.ColumnString);
        }

        /// <summary>
        /// 生成删除条件语句
        /// </summary>
        /// <returns></returns>
        public string ToWhereIn(int conditionCount)
        {
            TableSchema tableSchema = _DBSchema.TableDict[CapitalTableName];

            return CreateWhereIn(tableSchema.PrimaryKeyColumns[0], conditionCount);
        }


        /// <summary>
        /// 生成In语句
        /// </summary>
        /// <param name="conditionCount"></param>
        /// <returns></returns>
        public string ToIn(int conditionCount)
        {
            return CreateIn(conditionCount);
        }


        /// <summary>
        /// 生成删除语句
        /// </summary>
        /// <returns></returns>
        public string ToDelete()
        {
            return CreateDelete();
        }

        ///// <summary>
        ///// 创建删除
        ///// </summary>
        ///// <returns></returns>
        //protected string CreateDelete()
        //{
        //    return "DELETE FROM " + EntityName + " ";
        //}

        /// <summary>
        /// 创建Count(*)语句
        /// </summary>
        /// <returns></returns>
        public string ToCountAll()
        {
            return CreateCountAll();
        }

        /// <summary>
        /// 创建SELECT TOP 1 1 FROM语句
        /// </summary>
        /// <returns></returns>
        public string ToExistsAll()
        {
            return CreateExists();
        }
    }
}