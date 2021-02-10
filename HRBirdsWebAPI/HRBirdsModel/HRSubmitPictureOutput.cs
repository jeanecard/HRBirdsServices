using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsModelDto
{
    public class HRSubmitPictureOutput
    {
        public String id { get; set; }
        public String vernacularName { get; set; }
        public char[] url { get; set; }
        public String typeAge { get; set; }
        public bool isMale { get; set; }
        public String source { get; set; }
        public String credit { get; set; }
    }
}
