using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nulobe.Microsoft.WindowsAzure.Storage
{
    public static class CloudBlobContainerExtensions
    {
        public static async Task<string> UploadAsync(
            this CloudBlobContainer container,
            CloudBlob blob)
        {
            var blockBlob = container.GetBlockBlobReference(blob.Path);
            blockBlob.Properties.ContentType = blob.ContentType;
            await blockBlob.UploadFromStreamAsync(blob.Stream);
            return blockBlob.Uri.ToString();
        }

        public static async Task<CloudBlob> GetAsync(
            this CloudBlobContainer container,
            string path)
        {
            //var relativePath = string.Concat((new Uri(path)).Segments.Skip(2));
            var blockBlob = container.GetBlockBlobReference(path);
            var stream = await blockBlob.OpenReadAsync();

            return new CloudBlob()
            {
                Path = blockBlob.Name,
                ContentType = blockBlob.Properties.ContentType,
                Stream = stream
            };
        }
    }
}
