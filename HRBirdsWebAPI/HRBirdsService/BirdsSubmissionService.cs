using HRBirdRepository.Interface;
using HRBirdService.Interface;
using HRBirdsRepository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRBirdService
{
    public class BirdsSubmissionService : IBirdsSubmissionService
    {
        private IHRBirdRepository _birdsRepo = null;
        private IHRBirdSubmissionRepository _birdsSubmissionrepo = null;
        private BirdsSubmissionService()
        {
            // Dummy for DI.
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="bRepo"></param>
        /// <param name="bSubRepo"></param>
        public BirdsSubmissionService(IHRBirdRepository bRepo, IHRBirdSubmissionRepository bSubRepo)
        {
            _birdsRepo = bRepo;
            _birdsSubmissionrepo = bSubRepo;

        }

        /// <summary>
        /// Merge of birds Repo and Birds Submission repo
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetMatchingVernacularNamesAsync(string pattern)
        {
            HashSet<string> vernacularNames = new HashSet<string>();
            //1- Get from existing birds
            using var birdsTask = _birdsRepo.GetMatchingVernacularNamesAsync(pattern);
            await birdsTask;
            if(birdsTask.IsCompletedSuccessfully)
            {
                foreach(String iter in birdsTask.Result)
                {
                    if(!vernacularNames.Contains(iter))
                    {
                        vernacularNames.Add(iter);
                    }
                }
            }
            else
            {
                throw new Exception("_birdsRepo.GetMatchingVernacularNamesAsync error");
            }
            //2- Get from birsSubmissionRepo
            using var birdsSubmissionTask = _birdsSubmissionrepo.GetMatchingVernacularNamesAsync(pattern);
            await birdsSubmissionTask;
            if(birdsSubmissionTask.IsCompletedSuccessfully)
            {
                foreach (String iter in birdsSubmissionTask.Result)
                {
                    if (!vernacularNames.Contains(iter))
                    {
                        vernacularNames.Add(iter);
                    }
                }
            }
            else
            {
                throw new Exception("_birdsSubmissionrepo.GetMatchingVernacularNamesAsync error");
            }
            //3- sort and return 
            return vernacularNames;

        }
    }
}
