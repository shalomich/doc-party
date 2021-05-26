using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Repositories
{
    /// <summary>
    /// Repository for file that keep them in file system.
    /// </summary>
    class FileSystemRepository : IRepository<byte[], string>
    {
        private readonly string RooteFilePath;
        public FileSystemRepository(IWebHostEnvironment environment)
        {
            RooteFilePath = environment.WebRootPath + "/Files/";       
        }

        private string GetFilePath(string fileName)
        {
            return $"{RooteFilePath}/{fileName}";
        }

        public void Create(string id, byte[] fileBytes)
        {
            using var stream = new FileStream(GetFilePath(id), FileMode.Create);
            stream.Write(fileBytes);
        }

        public void Delete(string id)
        {
            File.Delete(GetFilePath(id));
        }

        public byte[] Select(string id)
        {
            using var stream = new FileStream(GetFilePath(id), FileMode.Open);
            byte[] fileBytes = new byte[stream.Length];
            
            stream.Read(fileBytes, 0, fileBytes.Length);
            
            return fileBytes;
        }

        public void Update(string id, byte[] fileBytes)
        {
            using var stream = new FileStream(GetFilePath(id), FileMode.OpenOrCreate);
            stream.Write(fileBytes);
        }
    }
}
