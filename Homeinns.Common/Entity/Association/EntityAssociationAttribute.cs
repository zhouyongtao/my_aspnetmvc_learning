using System;
using System.Collections.Generic;
using System.Text;

namespace Homeinns.Common.Entity.Association
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityAssociationAttribute : Attribute
    {
        public string TableName { get; set; }
    }
}
