using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class SolutionQueue : TableEntity
    {
        public string Id { get; set; }
        public string SolutionId { get; set; }

        public BuildStatus BuildStatus { get; set; }
    }
}
