using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
   
        public interface ICacheableQuery
        {
            string CacheKey { get; }
            TimeSpan? Expiration { get; }
            bool BypassCache { get; } // optional, useful for both
    }
    
}
