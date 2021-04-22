using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBirdsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IHRImageNotifySignalR _signal = null;
        private ISubmittedImageNotifier _queueService = null;

        private TestController()
        {

        }

        public TestController(
            IHRImageNotifySignalR truc,
            ISubmittedImageNotifier machin)
        {
            _signal = truc;
            _queueService = machin;
        }
        // GET: api/<TestController>
        [HttpGet]
        public string Get()
        {

            return "machin";
        }

        // GET api/<TestController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(string id)
        {
            HRSubmitPictureInputDto item = new HRSubmitPictureInputDto()
            {
                AgeType = Guid.NewGuid(),
                Comment = "un commentaire bateau",
                Credit = "un credit bateau é ",

                //Credit = "un crédit bateâu",
                FullImageUrl = "http://monimage.jpeg",
                GenderType = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                SourceType = Guid.NewGuid(),
                ThumbnailUrl = String.Empty,
                VernacularName = "turdus merula",
               
            };
            using var queuetask = _queueService.OnNewMetadataImageAsync(null);
            await queuetask;
            return "yes";


        }
        //public static string Base64Decode(string base64EncodedData)
        //{
        //    var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        //    return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        //}
        // POST api/<TestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
