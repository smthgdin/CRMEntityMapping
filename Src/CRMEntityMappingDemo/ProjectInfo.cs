using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CRMEntityMapping.Domain;

namespace CRMEntityMappingDemo
{
    [Entity("comba_project")]
    public class ProjectInfo
    {
        /// <summary>
        /// CRM项目名称
        /// </summary>
        public string comba_name { get; set; }

        /// <summary>
        /// CRM项目编号
        /// </summary>
        public string comba_projectcode { get; set; }

        /// <summary>
        /// SAP项目编号
        /// </summary>
        public string comba_sapprojectcode { get; set; }

        /// <summary>
        /// 主导事业部
        /// </summary>
        [Reference("businessunit", IsBindName = true)]
        public string comba_businessunit { get; set; }

        /// <summary>
        /// 项目类别
        /// </summary>
        [DataType("OptionSetValue")]
        public string comba_projecttype { get; set; }

        /// <summary>
        /// 项目类别二级
        /// </summary>
        [Reference("comba_projecttype", IsBindName = true)]
        public string comba_projecttypesecond { get; set; }

        /// <summary>
        /// 采购类型
        /// </summary>
        [DataType("OptionSetValue")]
        public string comba_purchasetype { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [Reference("comba_province", IsBindName = true)]
        public string comba_province { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        [Reference("account", IsBindName = true)]
        public string comba_account { get; set; }

        /// <summary>
        /// 中标份额
        /// </summary>
        //[Reference("comba_bidrecords", IsBindName = true)]
        public string comba_bidshare { get; set; }

        /// <summary>
        /// 含税中标金额
        /// </summary>
        public decimal comba_acceptedamountaftertax { get; set; }

        /// <summary>
        /// 未税中标金额
        /// </summary>
        public decimal comba_acceptedamount { get; set; }

        /// <summary>
        ///负责人
        /// </summary>
        [Reference("ownerid", IsBindName = true)]
        public string ownerid { get; set; }

        /// <summary>
        /// 投标公示日期
        /// </summary>
        public string comba_opendate { get; set; }
    }
}
