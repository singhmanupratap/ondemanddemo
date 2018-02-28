using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Models;

namespace Businesseses
{
    public interface IBusinessRepository
    {
        Task<List<Subscription>> GetSubscriptionsByUser(string userId);
    }
}