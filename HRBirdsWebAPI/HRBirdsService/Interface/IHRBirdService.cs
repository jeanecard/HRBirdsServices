using HRBirdsModel;
using HRCommonModel;
using HRCommonModels;
using System.Threading.Tasks;

namespace HRBirdServices.Interface
{
    public interface IHRBirdService
    {
        Task<PagingParameterOutModel<HRBirdMainOutput>> GetMainRecordsAsync(
            HRBirdMainInput query,
            PagingParameterInModel pageModel,
            HRSortingParamModel orderBy);
    }
}
