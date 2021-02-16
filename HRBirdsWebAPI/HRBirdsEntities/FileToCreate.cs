using HRBirdEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsEntities
{
    public class FileToCreate
    {
        public HRSubmitPictureInput SubmittedPicture { get; set; }
        public string FileType { get; set; }
        public string FileAsBase64 { get; set; }
        public byte[] FileAsByteArray { get; set; }
        public short Part { get; set; }
        public short PartCount { get; set; }
    }
}
