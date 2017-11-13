using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
namespace Common.Models
{
    public class Solution: TableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public SolutionType SolutionType { get; set; }
        public SourceControlType SourceControlType { get; set; }
        public string TemplateFolder { get; set; }
        public string Image { get; set; }
    }
}