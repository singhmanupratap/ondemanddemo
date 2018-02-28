using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilities.Models
{
    public class Subscription : TableEntity
    {
        public string Id { get; set; }
        public string DirectoryId { get; set; }
        public DateTime ConnectedOn { get; set; }
        public string ConnectedBy { get; set; }
        public bool AzureAccessNeedsToBeRepaired { get; set; }
    }
}