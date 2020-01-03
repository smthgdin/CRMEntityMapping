using System;
using System.Linq;
using System.Reflection;

using CRMEntityMapping.Infrastructure.Mapping.CRM.Converter;
using CRMEntityMapping.Domain;

using Microsoft.Xrm.Sdk;

namespace CRMEntityMapping.Infrastructure.Mapping.CRM.Convention
{
    public static class EntityMapper
    {
        /// <summary>
        /// 从模型到Entity的映射
        /// </summary>
        /// <typeparam name="TSource">模型的类型</typeparam>
        /// <param name="source">模型实例</param>
        /// <returns></returns>
        public static Entity MapFrom<TSource>(TSource source)
           where TSource : class, new()
        {
            var entity = CreateEntity(source);

            //2.给Entity的属性赋值
            var properties = source.GetType().GetProperties();
            foreach(var property in properties)
            {
                if (property.IsDefined(typeof(IgnoreAttribute), false))
                    continue;

                if (property.IsDefined(typeof(DataTypeAttribute), false))
                {
                    //获取DataType特性
                    var attribute = property
                        .GetCustomAttributes()
                        .Where(a => a.GetType() == typeof(DataTypeAttribute))
                        .FirstOrDefault();

                    //获取DataType属性值
                    var tmp = (DataTypeAttribute)attribute;                   
                    var attrPropertyValue = tmp.DataType;  
                    if (string.IsNullOrWhiteSpace(attrPropertyValue))
                        continue;

                    //给Entity属性赋值
                    var propertyValue = property.GetValue(source);
                    if (propertyValue != null)
                    {
                        if (attrPropertyValue.ToLower() == "OptionSetValue".ToLower())
                        {
                            entity[property.Name.ToLower()] = new OptionSetValue(int.Parse(propertyValue.ToString()));
                            continue;
                        }

                        if (attrPropertyValue.ToLower() == "Money".ToLower())
                        {
                            entity[property.Name.ToLower()] = new Money(decimal.Parse(propertyValue.ToString()));
                            continue;
                        }
                    }
                }

                if (property.IsDefined(typeof(ReferenceAttribute), false))
                {
                    //获取Reference特性
                    var attribute = property
                        .GetCustomAttributes()
                        .Where(a => a.GetType() == typeof(ReferenceAttribute))
                        .FirstOrDefault() as ReferenceAttribute;
                    if(attribute == null)
                        continue;

                    //获取实体名
                    var attrEntityNameValue = attribute.EntityName;
                    if (string.IsNullOrWhiteSpace(attrEntityNameValue))
                        continue;

                    //获取模型实例的属性值
                    var propertyValue = property.GetValue(source);
                    if (propertyValue == null)
                        continue;

                    //给EntityReference赋值
                    EntityReference refEntity = 
                        new EntityReference(attrEntityNameValue, new Guid(propertyValue.ToString()));

                    entity[property.Name.ToLower()] = refEntity;

                    continue;
                }

                entity[property.Name.ToLower()] = property.GetValue(source);
            }

            return entity;
        }

        /// <summary>
        /// 从Entity到模型的映射
        /// </summary>
        /// <typeparam name="Destination">模型类型</typeparam>
        /// <param name="source">Entity</param>
        /// <returns>模型实例</returns>
        public static Destination MapTo<Destination>(Entity source)
         where Destination : class, new()
        {
            Destination model = new Destination();
            var properties = model.GetType().GetProperties();

            foreach(var property in properties)
            {
                if (property.IsDefined(typeof(IgnoreAttribute), false))
                    continue;

                var propertyName = property.Name.ToLower();
                if (!source.Contains(propertyName))
                    continue;

                //获取Entity特性值
                var attr = source.Attributes[propertyName];
                if (attr == null)
                    continue;

                //获取模型属性的类型，用于类型转换
                Type type = property.PropertyType;

                if (attr is EntityReference)
                {
                    //获取模型实例属性的特性
                    var attribute = property
                          .GetCustomAttributes()
                          .Where(a => a.GetType() == typeof(ReferenceAttribute))
                          .FirstOrDefault() as ReferenceAttribute;

                    if (attribute == null)
                        continue;

                    //给实例属性赋值
                    var tmp = (EntityReference)attr;
                    var attrName = attribute.GetType().GetProperty("Name");
                    var attrId = attribute.GetType().GetProperty("Id");

                    attrName.SetValue(attribute, tmp.Name);
                    attrId.SetValue(attribute, tmp.Id.ToString());

                    if(!attribute.IsBindName)
                        property.SetValue(model, tmp.Id.ToString());
                    if (attribute.IsBindName)
                        property.SetValue(model, tmp.Name);

                    continue;
                }

                if (attr is OptionSetValue)
                {
                    var attribute = property
                          .GetCustomAttributes()
                          .Where(a => a.GetType() == typeof(DataTypeAttribute))
                          .FirstOrDefault() as DataTypeAttribute;

                    if (attribute == null)
                        continue;

                    var v = ((OptionSetValue)attr).Value;

                    if (!attribute.IsBindValue)
                        property.SetValue(model, v.ConvertTo(type));
                    //if (attribute.IsBindValue)
                    //    property.SetValue(model, CRMHelper.GetPicklistDisplayName());

                    continue;
                }

                if (attr is Money)
                {
                    var v = ((Money)attr).Value;
                    property.SetValue(model, v.ConvertTo(type));

                    continue;
                }

                if (type == typeof(string))
                    property.SetValue(model, attr.ToString());
                if (type == typeof(int))
                    property.SetValue(model, (int)attr);
                if (type == typeof(float))
                    property.SetValue(model, (float)attr);
                if (type == typeof(decimal))
                    property.SetValue(model, (decimal)attr);
                if (type == typeof(bool))
                    property.SetValue(model, (bool)attr);
            }

            return model;
        }

        public static Destination MapFrom<Destination>(this Destination dest, Entity source)
            where Destination : class, new()
        {
            var rlt = MapTo<Destination>(source);

            var properties = dest.GetType().GetProperties();
            if(properties != null && properties.Length > 0)
            {
                foreach(var property in properties)
                {
                    if(source.Contains(property.Name.ToLower()))
                    {
                        var value = property.GetValue(rlt);
                        property.SetValue(dest, value);
                    }
                }
            }

            return dest;
        }

        private static Entity CreateEntity<TSource>(TSource source)
           where TSource : class, new()
        {
            if (!source.GetType().IsDefined(typeof(EntityAttribute), false))
                throw new Exception($"模型{source.GetType().Name},需要定义Entity特性");

            var attrEntity = source
                .GetType()
                .GetCustomAttributes()
                .Where(a => a.GetType() == typeof(EntityAttribute))
                .FirstOrDefault();

            var attrEntityValue = attrEntity
                .GetType()
                .GetProperty("EntityName")
                .GetValue(attrEntity);

                if(attrEntityValue == null)
                    throw new Exception($"模型{source.GetType().Name},需要定义映射的Entity名");

            var entityName = attrEntityValue.ToString().ToLower();

            return new Entity(entityName);
        }
    }
}
