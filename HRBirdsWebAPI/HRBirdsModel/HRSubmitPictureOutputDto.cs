using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsModelDto
{
    public class HRSubmitPictureOutputDto
    {
        public String id { get; set; }
        public String vernacularName { get; set; }
        public String ageType { get; set; }
        public Guid? genderType { get; set; }
        public Guid? sourceType { get; set; }
        public String credit { get; set; }
        public String comment { get; set; }
        public String thumbnailUrl { get; set; }
        public String fullImageUrl { get; set; }
    }
}
