using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdEntity
{
    public class HRSubmitPicture
    {
        public Guid Id { get; set; }
        public String Vernacular_name { get; set; }
        public short Type_age { get; set; }
        public short Type_gender { get; set; }
        public short Id_source { get; set; }
        public String Credit { get; set; }
        public String Image_data { get; set; }
        public String Url_fullsize { get; set; }
        public String Url_thumbnail { get; set; }
        public String Comment { get; set; }


    }
}
