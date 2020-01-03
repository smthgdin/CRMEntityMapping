using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace CRMEntityMapping.Infrastructure.Mapping.CRM.Configuration
{
    /// <summary>
    /// 对象配置类
    /// 
    /// 目的：
    ///     1.定义了实体和数据库表映射关系。
    /// 
    /// 使用规范：
    ///     
    ///     
    /// </summary>
    public class EntityConfiguration<TBizEntity> : ConfigurationBase
    {
        public  EntityConfiguration()
        {
            Key = typeof(TBizEntity).FullName.ToLower();
            Columns = new Dictionary<string, PropertyConfiguration<TBizEntity>>();
        }

        /// <summary>
        /// 业务实体配置类的主键
        /// 主键=类的全名的小写字母字符串
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 业务实体映射的数据实体的表名
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// 业务实体映射的数据实体的表的主键
        /// </summary>
        public string PK { get; private set; }

        /// <summary>
        /// 业务实体映射的数据实体的表的架构
        /// </summary>
        public string TableSchemaName { get; private set; }

        /// <summary>
        /// 业务实体映射的数据实体的列集合
        /// </summary>
        public IDictionary<string, PropertyConfiguration<TBizEntity>> Columns { get; }


        /// <summary>
        /// 设置业务实体和数据实体映射
        /// 由于数据实体和表一一对应，所以等同于业务实体和表的映射
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public EntityConfiguration<TBizEntity> ToTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException("参数tableName为空");

            TableName = tableName;

            return this;
        }

        /// <summary>
        /// 设置业务实体和数据实体映射
        /// 由于数据实体和表一一对应，所以等同于业务实体和表的映射 
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="schemaName">数据库中表的架构</param>
        /// <returns></returns>
        public EntityConfiguration<TBizEntity> ToTable(string tableName, string schemaName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentNullException("参数tableName为空");

            if (string.IsNullOrWhiteSpace(schemaName))
                throw new ArgumentNullException("参数tableName为空");

            TableName = tableName;
            TableSchemaName = schemaName;

            return this;
        }

        /// <summary>
        /// 设置表主键名
        /// </summary>
        /// <param name="keyName">表的主键名</param>
        /// <returns></returns>
        public EntityConfiguration<TBizEntity> HasKey(string keyName)
        {
            if(string.IsNullOrWhiteSpace(keyName))
                throw new ArgumentNullException("参数keyName为空");

            PK = keyName;

            return this;
        }

        /// <summary>
        /// 根据属性的Lambda表达式获得属性的配置类
        /// </summary>
        /// <typeparam name="V">被配置的属性的类型</typeparam>
        /// <param name="propertyExpression">属性的Lambda表达式</param>
        /// <returns>属性配置类，用于对属性进行具体配置</returns>
        public virtual PropertyConfiguration<TBizEntity> Property<V>(Expression<Func<TBizEntity, V>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException("参数propertyExpression为空");

            if (!(propertyExpression.Body is MemberExpression))
                throw new ArgumentException("参数propertyExpression不是成员表达式");

            string propertyName = ((MemberExpression)propertyExpression.Body).Member.Name;     //属性名
            PropertyConfiguration<TBizEntity> propertyConfig = new PropertyConfiguration<TBizEntity>();
            Columns.Add(propertyName, propertyConfig);

            return propertyConfig;
        }
    }
}
