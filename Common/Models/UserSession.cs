using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Models;

namespace Common.Models
{
    public class UserSession: TableEntity
    {
        public UserSession()
        {
            ExpireTime = DateTime.Now.AddMinutes(5);
        }
        public string UserId { get; set; }
        public string SolutionId { get; set; }
        public DateTime ExpireTime { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
    }
}
