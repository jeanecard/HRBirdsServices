using HRBirdRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdRepository
{
    public class HRBirdSubmissionRepository : IHRBirdSubmissionRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetMatchingVernacularNamesAsync(string pattern)
        {
            return new List<String>() { "oiseau bouchon 1", "oiseau bouchon 2"};
        }
    }
}
