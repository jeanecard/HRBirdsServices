using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsModelDto
{
    public class HRSubmitPictureInput
    {
        public String Id { get; set; }
        public String VernacularName { get; set; }
        public String ImageData { get; set; }
        public int TypeAge { get; set; }
        public int TypeGender { get; set; }
        public int SourceID { get; set; }
        public String Credit { get; set; }
    }
}
