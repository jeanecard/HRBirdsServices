using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsModelDto
{
    public class HRSubmitPictureListItemDto
    {
        public Guid Id { get; set; }
        public String VernacularName { get; set; }
        public String Age { get; set; }
        public String Gender { get; set; }
        public String Source { get; set; }
        public String Credit { get; set; }
        public String ThumbnailUrl { get; set; }
        public String Comment { get; set; }
    }
}
