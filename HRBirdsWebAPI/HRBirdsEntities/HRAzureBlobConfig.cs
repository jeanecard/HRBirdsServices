using System;
using System.Collections.Generic;
using System.Text;

namespace HRBirdsEntities
{
    public class HRAzureBlobConfig
    {
        public String Password { get; set; }
        public String StorageSharedKeyCredentialName { get; set; }
        public String BlobRootURI { get; set; }
        public String WakeUpAzureFunctionEndPoint { get; set; }
    }
}
