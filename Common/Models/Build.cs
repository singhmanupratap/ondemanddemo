
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Build : TableEntity
    {
        public string SolutionId { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string VSTSBuildId { get; set; }
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
        public string AzureAccountOwnerName { get; set; }
        public string AzureSubscriptionId { get; set; }
        public string AzureTenantId { get; set; }
        public string ServicePrincipalId { get; set; }
        public string ServicePrincipalPassword { get; set; }
    }

    public class VSTSBuild
    {
        public PostDefinition definition { get; set; }
        public string parameters { get; set; }
    }

    public class VSTSBuildResult
    {
        public string uri { get; set; }
        public Queue queue { get; set; }
        public Definition definition { get; set; }
        public object[] builds { get; set; }
        public string customGetVersion { get; set; }
        public string priority { get; set; }
        public int queuePosition { get; set; }
        public DateTime queueTime { get; set; }
        public string reason { get; set; }
        public Requestedby requestedBy { get; set; }
        public string status { get; set; }
        public Project project { get; set; }
        public int id { get; set; }
        public string url { get; set; }
        public Requestedfor requestedFor { get; set; }
        
    }

    public class Queue
    {
        public string queueType { get; set; }
        public int id { get; set; }
        public string url { get; set; }
    }

    public class Definition
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Requestedby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string state { get; set; }
    }

    public class Requestedfor
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class PostDefinition
    {
        public string id { get; set; }
    }
}
