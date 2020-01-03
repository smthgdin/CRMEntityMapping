using System;

namespace CRMEntityMapping.Infrastructure.Mapping.CRM.Configuration
{
    /// <summary>
    /// 属性配置类
    /// 
    /// 目的：
    ///     1.定义实体属性和表字段的映射。
    /// 
    /// 使用规范：
    ///     
    ///     
    /// </summary>
    public class PropertyConfiguration<TBizEntity> : ConfigurationBase
    {
        /// <summary>
        ///  业务实体映射的数据实体的列名
        /// </summary>
        public string ColumnName { get; private set; }

        /// <summary>
        /// 业务实体映射的数据实体列的数据类型
        /// </summary>
        public Type ColumnType { get; private set; }

        /// <summary>
        /// 业务实体映射的数据实体列的默认值
        /// </summary>
        public dynamic ColumnDefaultValue { get; private set; }

        /// <summary>
        /// 是否可空字段
        /// </summary>
        public bool IsRequiredColumn { get; private set; }

        /// <summary>
        /// 是否选项集
        /// </summary>
        public bool IsOptionSet { get; private set; }

        /// <summary>
        /// 设置业务实体属性对应的表字段
        /// </summary>
        /// <param name="columnName">字段名</param>
        /// <returns></returns>
        public PropertyConfiguration<TBizEntity> HasColumnName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentNullException("参数columnName为空");

            ColumnName = columnName;

            return this;
        }

        /// <summary>
        ///  设置业务实体属性对应的表字段的数据类型
        /// </summary>
        /// <param name="type">数据库的数据类型</param>
        /// <returns></returns>
        public PropertyConfiguration<TBizEntity> HasColumnType(Type type)
        {
            ColumnType = type ?? throw new ArgumentNullException("参数type为空");

            return this;
        }

        /// <summary>
        /// 设置业务实体属性对应的表字段是否为空
        /// </summary>
        /// <returns></returns>
        public PropertyConfiguration<TBizEntity> IsRequired(bool required = false)
        {
            IsRequiredColumn = required;

            return this;
        }

        /// <summary>
        /// 指定默认值
        /// </summary>
        /// <typeparam name="V">默认值的数据类型</typeparam>
        /// <param name="value">默认值</param>
        /// <returns></returns>
        public PropertyConfiguration<TBizEntity> HasDefaultValue<V>(V value)
        {
            ColumnDefaultValue = value;

            return this;
        }

        public PropertyConfiguration<TBizEntity> IsOptionSetValue(bool isOptionSet = true)
        {
            IsOptionSet = isOptionSet;

            return this;
        }
    }
}
