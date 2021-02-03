using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface IBirdsSubmissionService
    {
        Task<IEnumerable<String>> GetMatchingVernacularNamesAsync(string pattern);
    }
}
