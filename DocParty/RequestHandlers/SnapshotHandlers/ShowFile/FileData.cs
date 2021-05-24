using MimeTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.RequestHandlers.SnapshotHandlers.ShowFile
{
    public class FileData
    {
        public byte[] Bytes { set; get; }
        public string ContentType { set; get; }

        public static string GetFileName(string name, string contentType)
        {
            return $"{name}{MimeTypeMap.GetExtension(contentType)}";
        }
    }
}
