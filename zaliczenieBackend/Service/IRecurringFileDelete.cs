using System;
using Microsoft.AspNetCore.Hosting;

namespace zaliczenieBackend.Service
{
    public interface IRecurringFileDelete
    {
        public void DeleteFileFromServer(string fileName, string environment);
    }
}