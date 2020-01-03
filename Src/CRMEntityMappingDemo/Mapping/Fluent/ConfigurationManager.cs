using CRMEntityMapping.Infrastructure.Mapping.CRM.Configuration;

namespace CRMEntityMapping.Infrastructure.CRM.Mapping
{
    /// <summary>
    /// 数据实体配置管理器类
    /// 
    /// 目的：
    ///     1.获取已注册的业务实体的映射配置。
    ///     2.注册映射配置文件。
    /// 
    /// 使用规范：
    ///     
    ///     
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// 配置注册器
        /// </summary>
        public ConfigurationRegistrar Configurations
        {
            get
            {
                return ConfigurationRegistrar.Instance;
            }
        }

        /// <summary>
        /// 获取业务实体的映射配置
        /// </summary>
        /// <typeparam name="TBizEntity">业务实体</typeparam>
        /// <returns></returns>
        public EntityConfiguration<TBizEntity> GetEntityConfiguration<TBizEntity>()
        {
            return ConfigurationRegistrar.Instance.GetEntityConfiguration(typeof(TBizEntity).FullName.ToLower());
        }
    }
}
