using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMEntityMapping.Domain
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EntityAttribute : Attribute
    {
        public string EntityName { get; set; }

        public EntityAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
