using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicProjectApplication.Interfaces
{
    public interface IMfaAttemptLimiter
    {
        bool IsAllowed(string mfaToken);
        void RecordFailure(string mfaToken);
    }
}
