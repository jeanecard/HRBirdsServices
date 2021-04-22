using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsModelDto
{
    public class HRSubmitPictureOutputDto
    {
#pragma warning disable IDE1006

        public String id { get; set; }
        public String vernacularName { get; set; }
        public String ageType { get; set; }
        public String age { get; set; }
        public Guid? genderType { get; set; }
        public String gender { get; set; }
        public Guid? sourceType { get; set; }
        public String source { get; set; }
        public String credit { get; set; }
        public String comment { get; set; }
        public String thumbnailUrl { get; set; }
        public String fullImageUrl { get; set; }
    }
}
