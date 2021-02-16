using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdEntity
{
    public class HRSubmitPictureInput
    {
        public Guid Id { get; set; }
        public String VernacularName { get; set; }
        public Guid AgeType { get; set; }
        public Guid GenderType { get; set; }
        public Guid SourceType { get; set; }
        public String Credit { get; set; }
        public String Comment { get; set; }
        public String ThumbnailUrl { get; set; }
    }
}
