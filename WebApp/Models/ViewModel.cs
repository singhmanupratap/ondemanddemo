using System.Collections.Generic;
using Utilities.Models;

namespace WebApp.Models
{
    public class ViewModel
    {
        public ICollection<Subscription> ConnectedSubscriptions { get; set; }
    }
}