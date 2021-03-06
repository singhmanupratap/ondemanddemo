﻿using Common.Models;
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
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public SolutionType SolutionType { get; set; }
        public SourceControlType SourceControlType { get; set; }
        public string BuildTemplate { get; set; }
        public string Image { get; set; }
        public string Id { get; set; }
        public string BuildDefinitionId { get; set; }
        public string BuildDefinitionUrl { get; set; }
        public string BuildDefinitionUserName { get; set; }
        public string BuildDefinitionPassword { get; set; }
    }
}