using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdRepository.Interface
{
    public interface IHRBirdSubmissionRepository
    {
        Task<IEnumerable<String>> GetMatchingVernacularNamesAsync(string pattern);

    }
}
