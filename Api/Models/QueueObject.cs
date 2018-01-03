using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class QueueObject
    {
        public string Id { get; set; }
        public string SolutionName { get; set; }
        public string BuildTemplate { get; set; }
        public int Status { get; set; }
        public string BuildId { get; set; }
        public string PkgURL { get; set; }
        public string DeploymentName { get; set; }
        public string PresetAzureLocationName { get; set; }
        public string PresetAzureAccountName { get; set; }
        public string PresetAzureSubscriptionName { get; set; }
        public string PresetAzureDirectoryName { get; set; }
        public string VmAdminPassword { get; set; }
        public string Command { get; set; }
        public string Configuration { get; set; }
        public string AzureEnvironmentName { get; set; }
        public int IsLowCost { get; set; }
        public int IsForce { get; set; }
        public string BuildRepositoryLocalPath { get; set; }
        public string PresetAzureAccountPassword { get; set; }
        public string TemplateUri { get; set; }
        public string TemplateParameterUri { get; set; }
    }
}