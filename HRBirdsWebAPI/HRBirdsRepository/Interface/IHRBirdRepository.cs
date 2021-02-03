using HRBirdsModel;
using HRCommonModel;
using HRCommonModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdsRepository.Interface
{
    public interface IHRBirdRepository
    {
        Task<PagingParameterOutModel<HRBirdMainOutput>> GetMainRecordsAsync(HRBirdMainInput query, PagingParameterInModel pageModel, HRSortingParamModel orderBy);
        Task<IEnumerable<String>> GetMatchingVernacularNamesAsync(string pattern);
    }
}
