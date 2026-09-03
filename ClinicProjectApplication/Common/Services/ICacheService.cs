using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Common.Services
{
    public interface ICacheService
    {
        void Set<T>(string key, string prefix, T value, TimeSpan expiration);
        void RemoveByPrefix(string prefix);
    }
}
