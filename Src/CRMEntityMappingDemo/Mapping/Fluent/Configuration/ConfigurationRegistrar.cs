using System;
using System.Collections.Generic;
using System.Linq;

namespace CRMEntityMapping.Infrastructure.Mapping.CRM.Configuration
{
    /// <summary>
    /// 配置注册类
    /// 
    /// 目的：
    ///     1.用于管理业务实体和数据实体的映射配置。
    /// 
    /// 使用规范：
    ///     
    ///     
    /// </summary>
    public sealed class ConfigurationRegistrar
    {
        private static readonly Lazy<ConfigurationRegistrar> instance 
            = new Lazy<ConfigurationRegistrar>(() => new ConfigurationRegistrar());

        //业务实体和数据实体之间的映射配置字典
        private static readonly Lazy<IDictionary<string, dynamic>> entityConfigurations
            = new Lazy<IDictionary<string, dynamic>>(() => new Dictionary<string, dynamic>());

        private ConfigurationRegistrar()
        {

        }

        public static ConfigurationRegistrar Instance
        {
            get
            {
                return instance.Value;
            }
        }

        /// <summary>
        /// 新增一个业务实体的映射配置
        /// </summary>
        /// <typeparam name="TBizEntity">业务实体</typeparam>
        /// <param name="entityConfiguration"></param>
        /// <returns></returns>
        public ConfigurationRegistrar Add<TBizEntity>(EntityConfiguration<TBizEntity> entityConfiguration)
        {
            if(entityConfiguration == null)
                throw new ArgumentNullException("参数entityConfiguration为空");

            var entityKey = typeof(TBizEntity).FullName.ToLower();
            if (!entityConfigurations.Value.ContainsKey(entityKey))
                entityConfigurations.Value.Add(entityKey, entityConfiguration);

            return this;
        }

        /// <summary>
        /// 移除一个业务实体的映射配置
        /// </summary>
        /// <typeparam name="TBizEntity">业务实体</typeparam>
        /// <returns></returns>
        public ConfigurationRegistrar Remove<TBizEntity>()
        {
            var entityKey = typeof(TBizEntity).FullName.ToLower();

            if (entityConfigurations.Value.ContainsKey(entityKey))
                entityConfigurations.Value.Remove(entityKey);

            return this;
        }

        /// <summary>
        /// 获取业务实体的映射配置
        /// </summary>
        /// <param name="bizEntityFullName">业务实体类全名</param>
        /// <returns></returns>
        public dynamic GetEntityConfiguration(string bizEntityFullName)
        {
            if (string.IsNullOrWhiteSpace(bizEntityFullName))
                throw new ArgumentNullException("参数entityFullName为空");

            return entityConfigurations.Value.FirstOrDefault(s => s.Key == bizEntityFullName).Value;
        }
    }
}
