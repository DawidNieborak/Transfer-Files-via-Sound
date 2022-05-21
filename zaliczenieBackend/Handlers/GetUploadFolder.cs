using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using zaliczenieBackend.Entities;

namespace zaliczenieBackend.Handlers
{
    public class GetUploadFolder
    {
        private readonly IWebHostEnvironment _environment;

        public GetUploadFolder(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        
        public List<FileItem> Get()
        {
            DirectoryInfo items = new DirectoryInfo(_environment.WebRootPath + "/Upload/");
            List<FileItem> returnList = new List<FileItem>();
            
            foreach (var item in items.GetFiles())
            {
                var temp = item.ToString();
                int index = temp.LastIndexOf("\\");
                
                if (index >= 0)
                    temp = temp.Substring(index + 1);


                FileItem itemEtity = new()
                {
                    FileName = temp
                };
                returnList.Add(itemEtity);
            }

            return returnList;
        }
    }
}