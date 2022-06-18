using System.IO;
using Microsoft.AspNetCore.Hosting;
using zaliczenieBackend.Entities;

namespace zaliczenieBackend.Handlers
{
    public class GetUploadFirstFile
    {
        private readonly IWebHostEnvironment _environment;

        
        public GetUploadFirstFile(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public FileItem Get()
        {
            DirectoryInfo items = new DirectoryInfo(_environment.WebRootPath + "/Upload/");
            FileItem returnedItem = new FileItem();
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
                returnedItem = itemEtity;
            }

            return returnedItem;
        }
    }
}