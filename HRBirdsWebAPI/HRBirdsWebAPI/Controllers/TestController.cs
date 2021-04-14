using HRBirdService;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBirdsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IHRImageNotifySignalR _signal = null;

        private TestController()
        {

        }

        public TestController(IHRImageNotifySignalR truc)
        {
            _signal = truc;
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
            HRBirdsSignalRNotificationDto toto = new HRBirdsSignalRNotificationDto();
            toto.Id = new Guid(id);
            toto.VernacularName = "turdus merula";
            toto.Url = "agagagaga";
            try
            {
                using var saucisse = _signal.NotifySignalRRestAsync(toto, HRImageNotifySignalR.THUMBNAIL_REST_END_POINT_ENV_KEY);
                await saucisse;
                return "Ben OK";
            }
            catch (Exception ex)
            {
                return "ca a chier : " + ex.Message;
            }
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
