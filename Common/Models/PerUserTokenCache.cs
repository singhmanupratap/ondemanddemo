using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Utilities.Models
{
    public class UserTokenCache : TableEntity
    {
        public string WebUserUniqueId { get; set; }
        public byte[] CacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }
}