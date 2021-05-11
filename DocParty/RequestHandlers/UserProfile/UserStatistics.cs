using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.UserProfile
{
    public class UserStatistics
    {
        public IEnumerable<(string metricName, int metricValue)> Data { set; get; }
    }
}
