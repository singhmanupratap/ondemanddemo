using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Provision
    {
        public string SolutionName { get; set; }
        public string DeploymentName { get; set; }
        public string LocationName { get; set; }
        public string VmAdminPassword { get; set; }
        public string AzureAccountOwnerName { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
        public string SessionId { get; set; }
    }
}