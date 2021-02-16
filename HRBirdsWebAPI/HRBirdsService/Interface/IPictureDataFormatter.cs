using HRBirdsEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRBirdService.Interface
{
    public interface IPictureDataFormatter
    {
        String CleanData(String fileBase64Data);
        Task<String> GetPathAsync(FileToCreate fileToCreate);

    }
}
