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
        Task<IEnumerable<HRSubmitPicture>> GetSubmittedPicturesAsync(String vernacularName);
        Task AddPictureAsync(HRSubmitPicture picture);
        Task<HRSubmitPicture> UpdatePictureAsync(HRSubmitPicture picture);
        Task DeletePictureAsync(Guid id);
        Task<IEnumerable<HRSubmitGender>> GetGenderTypesAsync();
        Task<IEnumerable<HRSubmitAge>> GetAgeTypesAsync();
        Task<IEnumerable<HRSubmitSource>> GetSourcesAsync();
    }
}
