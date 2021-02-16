using Azure.Storage;
using Azure.Storage.Blobs;
using HRBirdService.Interface;
using HRBirdsModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRBirdsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRPictureStorageController : ControllerBase
    {
        private IHRPictureStorageService _storageService = null;

        private HRPictureStorageController()
        {
            // Dummy for DI.
        }

        public HRPictureStorageController(IHRPictureStorageService service)
        {
            _storageService = service;
        }
        // GET: api/<PictureStorageController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost("add-picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] FileToCreateDto theFile)
        {
            if(theFile == null)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            try
            {
                using var uploadTask = _storageService.UploadAsync(theFile);
                await uploadTask;
                if(uploadTask.IsCompletedSuccessfully)
                {
                    return Ok();
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            
        }
    }
}
