using HRBirdRepository.Interface;
using HRBirdsEntities;
using HRBirdService.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Business
{
    internal class PictureDataFormatter : IPictureDataFormatter
    {
        private IHRBirdSubmissionRepository _repo = null;
        private const  String UNKNOWN_VERNACULAR_NAME = "";

        private PictureDataFormatter()
        {
            //Dummy for DI.
        }

        public PictureDataFormatter(IHRBirdSubmissionRepository repo)
        {
            _repo = repo;
        }
        /// <summary>
        /// TO DO
        /// </summary>
        /// <param name="fileBase64Data"></param>
        /// <returns></returns>
        public String CleanData(String fileBase64Data)
        {
            if (!String.IsNullOrEmpty(fileBase64Data) && fileBase64Data.Contains(","))
            {
                return fileBase64Data.Substring(fileBase64Data.IndexOf(",") + 1);
            }
            return String.Empty;
        }
        /// <summary>
        ///  vernacularname/Sex/Age
        /// </summary>
        /// <param name="fileToCreate"></param>
        /// <returns></returns>
        public async Task<String> GetPathAsync(FileToCreate fileToCreate)
        {
            if (fileToCreate == null)
            {
                throw new ArgumentNullException();
            }
            StringBuilder sb = new StringBuilder();
            if (fileToCreate.SubmittedPicture == null
                || String.IsNullOrEmpty(fileToCreate.SubmittedPicture.VernacularName)
                 || String.IsNullOrWhiteSpace(fileToCreate.SubmittedPicture.VernacularName))
            {
                sb.Append(UNKNOWN_VERNACULAR_NAME);
            }
            else
            {
                //1- vernacular Name
                String vernacularName = fileToCreate.SubmittedPicture.VernacularName;
                String age = String.Empty; 
                String gender = String.Empty; 
                sb.Append(vernacularName.Replace(" ", "_"));
                sb.Append("/");
                //2- Sex and age
                var ageTask = _repo.GetAgeTypeAsync(fileToCreate.SubmittedPicture.AgeType);
                var genderTask = _repo.GetGenderTypeAsync(fileToCreate.SubmittedPicture.GenderType);
                List<Task> allTasks = new List<Task> { ageTask, genderTask };
                while (allTasks.Count != 0)
                {
                    //Await any task to finish
                    Task justFinishedTask = await Task.WhenAny(allTasks);
                    if (justFinishedTask == ageTask)
                    {
                        if(justFinishedTask.IsCompletedSuccessfully)
                        {
                            allTasks.Remove(ageTask);
                            age = ageTask.Result.Age.Replace(" ", "_");
                        }
                        else
                        {
                            throw new Exception("_repo.GetAgeTypeAsync fail");
                        }
                    }
                    else if (justFinishedTask == genderTask)
                    {
                        if (justFinishedTask.IsCompletedSuccessfully)
                        {
                            allTasks.Remove(genderTask);
                            gender = genderTask.Result.SubmitGender.Replace(" ", "_");
                        }
                        else
                        {
                            throw new Exception("_repo.GetGenderTypeAsync fail");
                        }
                    }
                    else
                    {
                        //Strange .. must have missed something...
                        allTasks.Remove(justFinishedTask);
                    }
                }
                sb.Append(gender.Trim());
                sb.Append("/");
                sb.Append(age.Trim());
                sb.Append("/");
            }
            //3- return
            return sb.ToString();
        }
    }
}
