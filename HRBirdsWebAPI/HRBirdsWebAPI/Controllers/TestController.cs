using Azure.Storage.Queues;
using HRBirdService;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml;

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
                Credit = "un credit bateau",

                //Credit = "un crédit bateâu",
                FullImageUrl = "http://monimage.jpeg",
                GenderType = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                SourceType = Guid.NewGuid(),
                ThumbnailUrl = String.Empty,
                VernacularName = "turdus merula",
               
            };
            using var queuetask = _queueService.OnNewMetadataImageAsync(item);
            await queuetask;
            return "yes";


        }

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
