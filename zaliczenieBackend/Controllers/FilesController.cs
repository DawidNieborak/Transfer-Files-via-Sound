using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using zaliczenieBackend.Entities;
using zaliczenieBackend.Handlers;
using zaliczenieBackend.Service;

namespace zaliczenieBackend.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {

        private readonly IWebHostEnvironment _environment;
        private readonly IRecurringFileDelete _recurringFileDelete;

        public FilesController(IWebHostEnvironment environment, IRecurringFileDelete recurringFileDelete)
        {
            _environment = environment;
            _recurringFileDelete = recurringFileDelete;
        }
        
        public class UplodedFile
        {
            public IFormFile files { get; set; }
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] UplodedFile objFile)
        {

            try
            {
                await using (FileStream fileStream =
                    System.IO.File.Create(_environment.WebRootPath + "/Upload/" + objFile.files.FileName))
                {
                
                    objFile.files.CopyTo(fileStream);
                    fileStream.Flush();
                    
                    var path = _environment.WebRootPath + "/Upload/";
                    var currentTime = DateTime.Now;
                    
                    
                    // hangfire create job
                    // change it later to 5.
                    DateTime x2MinsLater = currentTime.AddMinutes(10);
                    BackgroundJob.Schedule(
                        () => _recurringFileDelete.DeleteFileFromServer(objFile.files.FileName, path),
                        x2MinsLater
                    );
                
                    return Ok();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }


        [HttpGet]
        public async Task<List<FileItem>> GetList()
        {
            GetUploadFolder list = new GetUploadFolder(_environment);
            
            return list.Get();
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecific(string id)
        {
            if (String.IsNullOrEmpty(id))
                return BadRequest();
            
            GetUploadFolder getFiles = new GetUploadFolder(_environment);
            var list = getFiles.Get();
            
            
            // if fileName is equal to file in storage, return fileName to download file
            foreach (var item in list)
            {
                if (id.Contains(item.FileName))
                {
                    return Ok(item.FileName);
                }
            }

            return BadRequest();
        } 
    }
}