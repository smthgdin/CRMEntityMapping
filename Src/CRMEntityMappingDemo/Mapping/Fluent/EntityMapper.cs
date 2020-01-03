using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xrm.Sdk;

using CRMEntityMapping.Infrastructure.Mapping.CRM.Converter;
using CRMEntityMapping.Infrastructure.CRM.Mapping;

namespace CRMEntityMapping.Infrastructure.Mapping.CRM
{
    public class EntityMapper
    {
        public static Entity MapFrom<TSource>(TSource source)
            where TSource : class, new()
        {
            var sourceType = source.GetType();
            var entityConfig = new ConfigurationManager().GetEntityConfiguration<TSource>();
            var dbEntity = new Entity(entityConfig.TableName);

            foreach (var column in entityConfig.Columns)
            {
                var propertyValue = sourceType.GetProperty(column.Key).GetValue(source);
                
                if (propertyValue == null && column.Value.IsRequiredColumn == false)
                    continue;

                if (propertyValue == null && column.Value.IsRequiredColumn == true)
                    throw new Exception($"属性{propertyValue}必须赋值");

                //mapper配置文件中如果不指定映射的数据类型，则不做转换
                if (column.Value.ColumnType != null)
                {
                    var v = propertyValue as IConvertible;

                    if (propertyValue == null)
                        throw new InvalidCastException("无效转换，属性值不是IConvertible的实现。");

                    if(column.Value.IsOptionSet)
                        dbEntity[column.Value.ColumnName] = new OptionSetValue(v.ConvertTo(column.Value.ColumnType));
                    else
                        dbEntity[column.Value.ColumnName] = v.ConvertTo(column.Value.ColumnType);
                }
                else
                    dbEntity[column.Value.ColumnName] = propertyValue;
            }

            return dbEntity;
        }

        public static TDest MapTo<TDest>(Entity source)
             where TDest : class, new()
        {
            var model = new TDest();
            var entityConfig = new ConfigurationManager().GetEntityConfiguration<TDest>();
            foreach (var column in entityConfig.Columns)
            {
                foreach(var property in model.GetType().GetProperties())
                {
                    int index = column.Key.LastIndexOf('.');
                    if (property.Name.ToLower() == column.Key.Substring(index + 1).ToLower())
                    {
                        if(source[column.Value.ColumnName].GetType() == typeof(OptionSetValue))
                            property.SetValue(model, (int)source[column.Value.ColumnName]);
                        else
                            property.SetValue(model, source[column.Value.ColumnName]);
                    }
                }
            }

            return model;
        }
    }
}
