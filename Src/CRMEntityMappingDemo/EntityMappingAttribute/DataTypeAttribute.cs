using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntityMapping.Domain
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class DataTypeAttribute : Attribute
    {
        public string DataType { get; }

        /// <summary>
        ///  如果是键值对类型时，可以用于保存值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 如果是键值对类型时，可以用于保存键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 绑定
        /// 数据映射时，打上DataType特性的属性，默认绑定选项集的value，如果需要绑定中文，需要设置为true
        /// 只有entity转model时有效
        /// </summary>
        public bool IsBindValue { get; set; }

        public DataTypeAttribute(string datatype)
        {
            DataType = datatype;
            IsBindValue = false;
        }
    }
}
