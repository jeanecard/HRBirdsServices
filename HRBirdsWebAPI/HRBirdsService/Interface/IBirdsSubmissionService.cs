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
        Task<IEnumerable<String>> GetMatchingVernacularNamesAsync(string pattern);
        Task AddPictureDataAsync(HRSubmitPictureInput picture);
        Task DeletePictureDataAsync(Guid id);
        Task<IEnumerable<HRSubmitGenderDto>> GetGenderTypesAsync();
        Task<IEnumerable<HRSubmitAgeDto>> GetAgeTypesAsync();
        Task<IEnumerable<HRSubmitSourceDto>> GetSourcesAsync();
    }
}
