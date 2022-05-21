using System;
using System.IO;

namespace zaliczenieBackend.Service
{
    public class RecurringFileDelete : IRecurringFileDelete
    {
        public void DeleteFileFromServer(string fileName, string environment)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(environment);

            if (!directoryInfo.Exists)
            {
                
                // make it better.
                throw new Exception();
            }
            foreach (var item in directoryInfo.GetFiles())
                item.Delete();
        }
    }
}