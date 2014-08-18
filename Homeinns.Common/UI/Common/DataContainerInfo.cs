using System;
using System.Collections.Generic;
using Homeinns.Common.Entity.Schema;

namespace Homeinns.Common.UI.Common
{
    /// <summary>
    /// 界面/实体值类型转换委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TypeChangeHandler(object sender, PropertyValueSettingEventArgs e);

    /// <summary>
    /// 界面/实体值类型转换事件数据类
    /// </summary>
    public class PropertyValueSettingEventArgs : EventArgs
    {
        public PropertyValueSettingEventArgs(Type controlValueType, Type entityPropertyValueType,
                                             object entityPropertyValue, object controlValue)
        {
            ControlValueType = controlValueType;
            EntityPropertyValueType = entityPropertyValueType;
            ControlValue = controlValue;
            EntityPropertyValue = entityPropertyValue;
            Cancel = false;
        }

        #region 属性

        /// <summary>
        /// 控件值类型
        /// </summary>
        public Type ControlValueType { get; set; }

        /// <summary>
        /// 实体值类型
        /// </summary>
        public Type EntityPropertyValueType { get; set; }

        /// <summary>
        /// 控件值
        /// </summary>
        public object ControlValue { get; set; }

        /// <summary>
        /// 实体值
        /// </summary>
        public object EntityPropertyValue { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool Cancel { get; set; }

        #endregion
    }

    public delegate void ValidateControlValueHandler(object sender, ValidateControlValueEventArgs e);

    public class ValidateControlValueEventArgs : EventArgs
    {
        public ValidateControlValueEventArgs(object dataControlValue, string[] validateClassArray,
                                             ColumnSchema sqlColumnSchema, string title)
        {
            DataControlValue = dataControlValue;
            ValidateClassArray = validateClassArray;
            ColumnSchema = sqlColumnSchema;
            Title = title;
        }

        #region 属性

        /// <summary>
        /// 控件值
        /// </summary>
        public object DataControlValue { get; set; }

        /// <summary>
        /// 验证种类名
        /// </summary>
        public string[] ValidateClassArray { get; set; }

        /// <summary>
        /// 字段结构
        /// </summary>
        public ColumnSchema ColumnSchema { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否验证合格
        /// </summary>
        public bool Pass { get; set; }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EditData<T> where T : new()
    {
        private List<string> _editPropertys;

        public EditData()
        {
            Entity = new T();
        }

        public EditData(T t)
        {
            Entity = t;
        }

        /// <summary>
        /// 需修改值的字段名列表
        /// </summary>
        public List<string> EditPropertys
        {
            get
            {
                if(_editPropertys == null)
                {
                    _editPropertys = new List<string>();
                }
                return _editPropertys;
            }
            set { _editPropertys = value; }
        }

        /// <summary>
        /// 实体
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// 追加强制更新字段名称(需要把属性中的Null更新到数据库才需手动追加列名)
        /// </summary>
        /// <param name="fieldNames"></param>
        public void AppField(params string[] fieldNames)
        {
            if (_editPropertys == null)
            {
                _editPropertys = new List<string>();
            }

            _editPropertys.AddRange(fieldNames);
        }

        /// <summary>
        /// 移除编辑字段名称
        /// </summary>
        /// <param name="fieldNames"></param>
        public void RemoveField(params string[] fieldNames)
        {
            if (_editPropertys != null)
            {
                foreach (string fieldName in fieldNames)
                {
                    _editPropertys.Remove(fieldName);
                }
            }
        }
    }
}