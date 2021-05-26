using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocParty.Services.Repositories
{
    /// <summary>
    /// Repository for files that keep them on aws s3 storage.
    /// </summary>
    class AwsS3FileRepository : IRepository<byte[],string>
    {
        private static readonly RegionEndpoint Region = RegionEndpoint.EUNorth1;
        private readonly string BucketName;
        private readonly string AccessId;
        private readonly string AccessKey;

        public AwsS3FileRepository(IConfiguration configuration)
        {
            var awsSection = configuration.GetSection("AWS");

            BucketName = awsSection[nameof(BucketName)];
            AccessId = awsSection[nameof(AccessId)];
            AccessKey = awsSection[nameof(AccessKey)];
        }

        public void Create(string id, byte[] fileBytes)
        {
            using var client = new AmazonS3Client(AccessId, AccessKey, Region);
            using var stream = new MemoryStream(fileBytes);

            var fileTransferUtility = new TransferUtility(client);
            fileTransferUtility.Upload(stream, BucketName, id);     
        }

        public void Delete(string id)
        {
            using var client = new AmazonS3Client(AccessId, AccessKey, Region);

            var request = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = id
            };

            client.DeleteObjectAsync(request);
        }

        public byte[] Select(string id)
        {
            using var client = new AmazonS3Client(AccessId, AccessKey, Region);

            var fileTransferUtility = new TransferUtility(client);

            using var stream = fileTransferUtility.OpenStream(BucketName, id);

            byte[] fileBytes = new byte[stream.Length];

            stream.Read(fileBytes, 0, fileBytes.Length);

            return fileBytes;
        }

        public void Update(string id, byte[] fileBytes)
        {
            using var client = new AmazonS3Client(AccessId, AccessKey, Region);

            var fileTransferUtility = new TransferUtility(client);

            using var stream = fileTransferUtility.OpenStream(BucketName, id);

            stream.Write(fileBytes);
        }
    }
}
