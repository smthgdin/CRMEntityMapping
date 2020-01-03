using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMEntityMapping.Infrastructure.Mapping.CRM.Convention;
using CRMEntityMapping.Infrastructure.ORM.CRM;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace CRMEntityMappingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CrmServiceClient connection = new CrmServiceClient(
                  ConfigurationManager.ConnectionStrings["CRMConnectionString"].ConnectionString);

            //构建查询信息
            QueryExpression query = new QueryExpression()
            {
                EntityName = "comba_project",
                ColumnSet = new ColumnSet(new string[] {
                    "comba_name",
                    "comba_projectcode",
                    "comba_sapprojectcode",
                    "ownerid",
                    "comba_projecttypesecond",
                    "comba_account",
                    "comba_businessunit",
                    "comba_province",
                    "comba_projecttype",
                    "comba_purchasetype",
                    "comba_crmmainproject",
                    "comba_projectlevel"
                })
            };
            query.Criteria.AddCondition("comba_projectstatus", ConditionOperator.In, new object[] { 151220003, 151220000 });
            query.Criteria.AddCondition("comba_projectcode", ConditionOperator.Equal, new object[] { "11190020 - 11" });

            var crmProxy = new OrganizationService(connection);
            var entities = crmProxy.RetrieveMultiple(query);
            if (entities != null && entities.Entities.Count > 0)
            {
                var entity = entities[0];
                var projectInfo = new ProjectInfo();

                //使用映射
                projectInfo.MapFrom(entity);

                var mainprojectID = entity.GetAttributeValue<EntityReference>("comba_crmmainproject").Id.ToString();
                var bidrecord = GetBidrecord(mainprojectID, crmProxy);
                if (bidrecord != null)
                    projectInfo.MapFrom(bidrecord);   //多个CRMEntity映射到一个DTO

                projectInfo.comba_projecttype = 
                    CRMHelper.GetPicklistDisplayName(crmProxy, "comba_project", "comba_projecttype", int.Parse(projectInfo.comba_projecttype));
                projectInfo.comba_purchasetype = 
                    CRMHelper.GetPicklistDisplayName(crmProxy, "comba_project", "comba_purchasetype", int.Parse(projectInfo.comba_purchasetype));
                //end

                //不使用映射的代码
                //if (entity.Contains("comba_name"))
                //    projectInfo.comba_name = entity["comba_name"].ToString();
                //if (entity.Contains("comba_projectcode"))
                //    projectInfo.comba_projectcode = entity["comba_projectcode"].ToString();
                //if (entity.Contains("comba_sapprojectcode"))
                //    projectInfo.comba_sapprojectcode = entity["comba_sapprojectcode"].ToString();

                //if (entity.Contains("comba_projecttype"))
                //{
                //    var pt = entity.GetAttributeValue<OptionSetValue>("comba_projecttype").Value;
                //    var value = CRMHelper.GetPicklistDisplayName(crmProxy, "comba_project", "comba_projecttype", pt);
                //    projectInfo.comba_projecttype = value;
                //}

                //if (entity.Contains("comba_purchasetype"))
                //{
                //    var pt = entity.GetAttributeValue<OptionSetValue>("comba_purchasetype").Value;
                //    var value = CRMHelper.GetPicklistDisplayName(crmProxy, "comba_project", "comba_purchasetype", pt);
                //    projectInfo.comba_purchasetype = value;
                //}

                //if (entity.Contains("ownerid"))
                //{
                //    var owner = entity.GetAttributeValue<EntityReference>("ownerid");
                //    projectInfo.ownerid = owner == null ? "" : owner.Name;
                //}
                //if (entity.Contains("comba_projecttypesecond"))
                //{
                //    var ps = entity.GetAttributeValue<EntityReference>("comba_projecttypesecond");
                //    projectInfo.comba_projecttypesecond = ps == null ? "" : ps.Name;
                //}
                //if (entity.Contains("comba_account"))
                //{
                //    var account = entity.GetAttributeValue<EntityReference>("comba_account");
                //    projectInfo.comba_account = account == null ? "" : account.Name;
                //}
                //if (entity.Contains("comba_businessunit"))
                //{
                //    var bizUnit = entity.GetAttributeValue<EntityReference>("comba_businessunit");
                //    projectInfo.comba_businessunit = bizUnit == null ? "" : bizUnit.Name;
                //}
                //if (entity.Contains("comba_province"))
                //{
                //    var province = entity.GetAttributeValue<EntityReference>("comba_province");
                //    projectInfo.comba_province = province == null ? "" : province.Name;
                //}

                //var mainprojectID = entity.GetAttributeValue<EntityReference>("comba_crmmainproject").Id.ToString();
                //var bidrecord = GetBidrecord(mainprojectID);
                //if(bidrecord != null)
                //{
                //    if(bidrecord.Contains("comba_acceptedamount"))
                //        projectInfo.comba_acceptedamount = bidrecord.GetAttributeValue<Money>("comba_acceptedamount").Value;
                //    if (bidrecord.Contains("comba_acceptedamountaftertax"))
                //        projectInfo.comba_acceptedamountaftertax = bidrecord.GetAttributeValue<Money>("comba_acceptedamountaftertax").Value;
                //    if (bidrecord.Contains("comba_bidshare"))
                //        projectInfo.comba_bidshare = bidrecord["comba_bidshare"].ToString();
                //    if (bidrecord.Contains("comba_opendate"))
                //        projectInfo.comba_opendate = bidrecord["comba_opendate"].ToString();
                //}
                //end
            }
        }

        private static Entity GetBidrecord(string mainProjectID, OrganizationService crmProxy)
        {
            QueryExpression query = new QueryExpression()
            {
                EntityName = "comba_bidrecords",
                ColumnSet = new ColumnSet(
                    new string[] {
                    "comba_bidshare",
                    "comba_acceptedamountaftertax",
                    "comba_acceptedamount",
                    "comba_opendate" })
            };

            query.Criteria.AddCondition("comba_project", ConditionOperator.Equal, new object[] { mainProjectID });

            var entities = crmProxy.RetrieveMultiple(query);
            if (entities != null && entities.Entities.Count > 0)
                return entities.Entities[0];

            return null;
        }
    }
}
