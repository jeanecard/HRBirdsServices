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
        public byte[] Image_data { get; set; }
    }
}
