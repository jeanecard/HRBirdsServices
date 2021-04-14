using HRBirdEntity;
using HRBirdsModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface IBirdsSubmissionService
    {
        Task<IEnumerable<HRSubmitPictureListItemDto>> GetSubmittedPicturesAsync(String vernacularName);
        Task<HRSubmitPictureListItemDto> GetSubmittedPictureAsync(String id);

        Task<IEnumerable<String>> GetMatchingVernacularNamesAsync(string pattern);
        Task<HRSubmitPictureOutputDto> AddPictureDataAsync(HRSubmitPictureInputDto picture);
        Task<HRSubmitPictureOutputDto> UpdatePictureDataAsync(HRSubmitPictureInputDto test);
        Task DeletePictureDataAsync(Guid id);
        Task<IEnumerable<HRSubmitGenderDto>> GetGenderTypesAsync();
        Task<IEnumerable<HRSubmitAgeDto>> GetAgeTypesAsync();
        Task<IEnumerable<HRSubmitSourceDto>> GetSourcesAsync();
    }
}
