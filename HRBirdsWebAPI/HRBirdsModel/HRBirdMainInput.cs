using System;

namespace HRBirdsModel
{
    /// <summary>
    /// 
    /// </summary>
    public class HRBirdMainInput
    {
        /// <summary>
        /// 
        /// </summary>
        public HRSeason Season { get; set; }
        /// <summary>
        /// WGS84 lattitude
        /// </summary>
        public float Lat { get; set; }
        /// <summary>
        /// WGS84 Longitude
        /// </summary>
        public float Lon { get; set; }
        /// <summary>
        /// Range of search in kilometers
        /// </summary>
        public float Range { get; set; }
        /// <summary>
        /// Iso639 Language code on 2 caracters
        /// </summary>
        public String Lang_Iso_Code { get; set; }
}
}
