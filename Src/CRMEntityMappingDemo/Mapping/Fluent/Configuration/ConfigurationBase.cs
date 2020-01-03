using System;

namespace CRMEntityMapping.Infrastructure.Mapping.CRM.Configuration
{
    /// <summary>
    /// 配置基类
    /// 
    /// 目的：
    ///     1.配置相关类的基类，有需要可以改写一些通用方法
    /// 
    /// 使用规范：
    ///     1.所有配置相关的类都直接或者间接继承此类。
    ///     2.所有对象都直接间接继承Object，Object提供了几个通用方法，有需要可以在此重写。
    /// </summary>
    public abstract class ConfigurationBase
    {
        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public new Type GetType()
        {
            return base.GetType();
        }
    }
}
