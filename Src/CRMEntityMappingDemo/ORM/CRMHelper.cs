using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CRMEntityMapping.Infrastructure.ORM.CRM
{
    public class CRMHelper
    {
        public static string GetPicklistDisplayName(IOrganizationService organizationService, string entityName, string attributeName, int picklistValue)
        {
            var request = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };

            var response = (RetrieveAttributeResponse)organizationService.Execute(request);

            var metadata = (PicklistAttributeMetadata)response.AttributeMetadata;
            var optionList = metadata.OptionSet.Options.ToArray();

            string selectedOptionLabel = string.Empty;
            foreach (OptionMetadata option in optionList)
            {
                if (option.Value == picklistValue)
                {
                    selectedOptionLabel = option.Label.UserLocalizedLabel.Label;

                }
            }
            return selectedOptionLabel;
        }
    }
}