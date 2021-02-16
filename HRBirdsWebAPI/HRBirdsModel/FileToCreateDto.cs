using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsModelDto
{
    public class FileToCreateDto
    {
        public  HRSubmitPictureInputDto SubmittedPicture { get; set; }
        public string FileType { get; set; }
        public string FileAsBase64 { get; set; }
        // public byte[]? FileAsByteArray { get; set; }
        public short Part { get; set; }
        public short PartCount { get; set; }
    }
}
