using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HRBirdEntity;
using HRBirdsEntities;


namespace HRBirdRepository.Interface
{
    public interface IHRBirdSubmissionRepository
    {
        Task<IEnumerable<String>> GetMatchingVernacularNamesAsync(string pattern);
        Task<IEnumerable<HRSubmitPictureListItem>> GetSubmittedPicturesAsync(String vernacularName);
        Task<HRSubmitPictureListItem> GetSubmittedPicturesByID(String id);
        Task<HRSubmitPictureOutput> AddPictureAsync(HRSubmitPictureInput picture);
        Task<HRSubmitPictureInput> UpdatePictureAsync(HRSubmitPictureInput picture);
        Task UpdateThumbnailAsync(Guid id, String thumbnailPath);
        Task DeletePictureAsync(Guid id);
        Task<IEnumerable<HRSubmitGender>> GetGenderTypesAsync();
        Task<IEnumerable<HRSubmitAge>> GetAgeTypesAsync();
        Task<IEnumerable<HRSubmitSource>> GetSourcesAsync();

        Task<HRSubmitGender> GetGenderTypeAsync(Guid id);
        Task<HRSubmitAge> GetAgeTypeAsync(Guid id);
        Task<HRSubmitSource> GetSourceAsync(Guid id);
    }
}
