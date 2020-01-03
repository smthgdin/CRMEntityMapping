using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntityMapping.Domain
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ReferenceAttribute : Attribute
    {
        public string EntityName { get; }

        /// <summary>
        /// 引用对象的ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 引用对象的名（值）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 绑定
        /// 数据映射时，打上Reference特性的属性，默认绑定Id，可改为Name
        /// 只有entity转model时有效
        /// </summary>
        public bool IsBindName { get; set; }

        public ReferenceAttribute(string entity)
        {
            EntityName = entity;
            IsBindName = false;
        }
    }
}
