using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdEntity
{
    public class HRSubmitPicture
    {
        public Guid id { get; set; }
        public String vernacularName { get; set; }
        public Guid ageType { get; set; }
        public Guid genderType { get; set; }
        public Guid sourceType { get; set; }
        public String credit { get; set; }
        public String comment { get; set; }
    }
}
