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
        public Guid AgeType { get; set; }

        public String Gender { get; set; }
        public Guid GenderType { get; set; }
        public String Source { get; set; }
        public String SourceType { get; set; }
        public String Credit { get; set; }
        public String ThumbnailUrl { get; set; }
        public String Comment { get; set; }
        public String FullImageUrl { get; set; }
    }

    public class HRSubmitPictureListItemJsonDto
    {
        public HRSubmitPictureListItemJsonDto(HRSubmitPictureListItemDto data)
        {
            if(data != null)
            {
                id = data.Id;
                vernacularName = data.VernacularName;
                age = data.Age;
                ageType = data.AgeType;
                gender = data.Gender;
                genderType = data.GenderType;
                source = data.Source;
                sourceType = data.SourceType;
                credit = data.Credit;
                thumbnailUrl = data.ThumbnailUrl;
                comment = data.Comment;
                fullImageUrl = data.FullImageUrl;
            }
        }
        public Guid id { get; set; }
        public String vernacularName { get; set; }
        public String age { get; set; }
        public Guid ageType { get; set; }

        public String gender { get; set; }
        public Guid genderType { get; set; }
        public String source { get; set; }
        public String sourceType { get; set; }
        public String credit { get; set; }
        public String thumbnailUrl { get; set; }
        public String comment { get; set; }
        public String fullImageUrl { get; set; }
    }
}
