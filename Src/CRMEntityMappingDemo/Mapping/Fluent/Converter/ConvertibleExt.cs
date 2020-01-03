using System;

namespace CRMEntityMapping.Infrastructure.Mapping.CRM.Converter
{
    /// <summary>
    /// 数据转换扩展类类
    /// 
    /// 目的：
    ///     1.支持原生类型和可空类型的转换；
    /// 
    /// 使用规范：
    ///     例如：DateTime? dt = "1981-08-24".ConvertTo&lt;DateTime?&gt;();
    /// </summary>
    public static class ConvertibleExt
    {
        /// <summary>
        /// 数据类型转换扩展方法，针对泛型
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="source">实现IConvertible接口的类型</param>
        /// <returns>T指定的类型</returns>
        public static T ConvertTo<T>(this IConvertible source)
        {
            if (null == source)
                return default(T);

            if (typeof(T).IsGenericType)
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(Nullable<>))
                    return (T)Convert.ChangeType(source, Nullable.GetUnderlyingType(typeof(T)));
            }
            else
                return (T)Convert.ChangeType(source, typeof(T));

            throw
                new InvalidCastException(string.Format($"无效转换。不能从类型 {source.GetType().FullName} 转成类型{typeof(T).FullName}."));
        }

        /// <summary>
        /// 数据类型转换扩展方法，针对非泛型
        /// </summary>
        /// <param name="convertibleValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static dynamic ConvertTo(this IConvertible convertibleValue, Type type)
        {
            if (null == type)
                return null;

            if (type.IsGenericType)
            {
                Type genericType = type.GetGenericTypeDefinition();

                if (genericType == typeof(Nullable<>))
                    return Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(type));
            }
            else
                return Convert.ChangeType(convertibleValue, type);

            throw 
                new InvalidCastException(string.Format($"无效转换。不能从类型 {convertibleValue.GetType().FullName} 转成类型{type.FullName}."));
        }

    }
}
