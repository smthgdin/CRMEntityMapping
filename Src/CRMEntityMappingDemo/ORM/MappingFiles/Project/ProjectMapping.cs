using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRMEntityMapping.Infrastructure.Mapping.CRM.Configuration;

namespace CRMEntityMappingDemo.ORM.MappingFiles
{
    public class ProjectMapping : EntityConfiguration<ProjectInfo>
    {
        //如果crm实体字段和ProjectInfo字段名字、类型不同时才适合使用，这里只是演示用
        public ProjectMapping()
        {
            this.ToTable("comba_project");
            //this.HasKey("myId");
            this.Property(c => c.comba_name).HasColumnName("comba_name").HasColumnType(typeof(string));
            this.Property(c => c.comba_projectcode).HasColumnName("comba_projectcode").HasColumnType(typeof(string));
            this.Property(c => c.comba_sapprojectcode).HasColumnName("comba_sapprojectcode").HasColumnType(typeof(string));
            this.Property(c => c.comba_businessunit).HasColumnName("comba_businessunit").HasColumnType(typeof(string));  
            this.Property(c => c.comba_projecttype).HasColumnName("comba_projecttype").HasColumnType(typeof(string));
        }
    }
}
