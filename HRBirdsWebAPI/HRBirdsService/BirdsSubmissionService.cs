﻿using AutoMapper;
using HRBirdEntity;
using HRBirdRepository.Interface;
using HRBirdsEntities;
using HRBirdService.Interface;
using HRBirdsModelDto;
using HRBirdsRepository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRBirdService
{
    public class BirdsSubmissionService : IBirdsSubmissionService
    {
        private readonly IHRBirdRepository _birdsRepo = null;
        private readonly  IHRBirdSubmissionRepository _birdsSubmissionrepo = null;
        private readonly IHRPictureConverterRepository _birdsPictureConverter = null;
        private readonly IMapper _mapper;

        private BirdsSubmissionService()
        {
            // Dummy for DI.
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="bRepo"></param>
        /// <param name="bSubRepo"></param>
        public BirdsSubmissionService(
            IHRBirdRepository bRepo, 
            IHRBirdSubmissionRepository bSubRepo,
            IHRPictureConverterRepository picConverter,
            IMapper mapper)
        {
            _birdsRepo = bRepo;
            _birdsSubmissionrepo = bSubRepo;
            _birdsPictureConverter = picConverter;
            _mapper = mapper;

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
            return vernacularNames.OrderBy(name => name);

        }
        /// <summary>
        /// 1- Submit picture to image Repository
        /// 
        /// </summary>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task AddPictureDataAsync(HRSubmitPictureInput picture)
        {
            if(picture != null)
            {
                //1- 
                using var taskPicture = _birdsSubmissionrepo.AddPictureAsync(_mapper.Map<HRSubmitPicture>(picture));
                await taskPicture;
                if(!taskPicture.IsCompletedSuccessfully)
                {
                    throw new Exception("_birdsPictureConverter.AddPictureAsync fail.");
                }
            }

        }
        /// <summary>
        /// TODO DAPPER
        /// </summary>
        /// <param name="vernacularName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitPictureListItemDto>> GetSubmittedPicturesAsync(string vernacularName)
        {
            using var birdsSubmissionTask = _birdsSubmissionrepo.GetSubmittedPicturesAsync(vernacularName);
            await birdsSubmissionTask;
            if (birdsSubmissionTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<IEnumerable<HRSubmitPictureListItemDto>>(birdsSubmissionTask.Result);
            }
            else
            {
                throw new Exception("_birdsSubmissionrepo.GetSubmittedPicturesAsync error");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitGenderDto>> GetGenderTypesAsync()
        {
            using Task<IEnumerable<HRSubmitGender>> getTask = _birdsSubmissionrepo.GetGenderTypesAsync();
            await getTask;
            if(getTask.IsCompletedSuccessfully)
            {
                return _mapper.Map< IEnumerable<HRSubmitGenderDto>>(getTask.Result);
            }
            throw new Exception("Error onasync call : GetGenderTypesAsync");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitAgeDto>> GetAgeTypesAsync()
        {
            using Task<IEnumerable<HRSubmitAge>> getTask = _birdsSubmissionrepo.GetAgeTypesAsync();
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<IEnumerable<HRSubmitAgeDto>>(getTask.Result);
            }
            throw new Exception("Error onasync call : GetAgeTypesAsync");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<HRSubmitSourceDto>> GetSourcesAsync()
        {
            using Task<IEnumerable<HRSubmitSource>> getTask = _birdsSubmissionrepo.GetSourcesAsync();
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return _mapper.Map<IEnumerable<HRSubmitSourceDto>>(getTask.Result);
            }
            throw new Exception("Error onasync call : GetAgeTypesAsync");
        }
    }
}
