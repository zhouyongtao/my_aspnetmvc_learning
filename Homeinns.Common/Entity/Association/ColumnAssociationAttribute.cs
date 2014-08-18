using System;
using System.Collections.Generic;
using System.Text;

namespace Homeinns.Common.Entity.Association
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAssociationAttribute : Attribute
    {
        private readonly string _columnName;
        public bool IsColumn { get; set; }
        public ColumnAssociationAttribute()
        { }

        public ColumnAssociationAttribute(string columnName)
        {
            _columnName = columnName;
            IsColumn = true;
        }
    }
}
