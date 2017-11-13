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
    }
}
