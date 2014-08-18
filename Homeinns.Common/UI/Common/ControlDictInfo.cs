using System;
using System.Reflection;

namespace Homeinns.Common.UI.Common
{
    public class ControlDictInfo
    {
        protected ControlGroupEnum _controlGroup;
        protected Type _controlType;
        protected string _controlTypeName;
        protected string _prefix;
        protected PropertyInfo _propertyInfo;
        protected string _propertyName;
        //protected Type _propertyUnderlyingType;

        public ControlDictInfo(ControlGroupEnum controlGroupEnum, string prefix, Type controlType, string propertyName)
        {
            _controlGroup = controlGroupEnum;
            _prefix = prefix;
            _controlTypeName = controlType.FullName;
            _controlType = controlType;
            _propertyName = propertyName;
            if (_propertyName != null)
            {
                _propertyInfo = _controlType.GetProperty(_propertyName);
                //this._propertyUnderlyingType = this._propertyInfo.PropertyType;
            }
        }

        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value; }
        }

        public string ControlTypeName
        {
            get { return _controlTypeName; }
            set { _controlTypeName = value; }
        }

        public Type ControlType
        {
            get { return _controlType; }
            set { _controlType = value; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
            set { _propertyInfo = value; }
        }

        //public Type PropertyUnderlyingType
        //{
        //    get { return this._propertyUnderlyingType; }
        //    set { this._propertyUnderlyingType = value; }
        //}

        public ControlGroupEnum ControlGroup
        {
            get { return _controlGroup; }
            set { _controlGroup = value; }
        }
    }

    /// <summary>
    /// 控件分组
    /// </summary>
    public enum ControlGroupEnum
    {
        DataGroup = 1,
        ValidateGroup = 2,
        ContainerGroup = 3
    }
}