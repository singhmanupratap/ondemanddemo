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
    }
}