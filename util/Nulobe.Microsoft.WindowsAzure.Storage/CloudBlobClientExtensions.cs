using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nulobe.Microsoft.WindowsAzure.Storage
{
    public static class CloudBlobClientExtensions
    {
        public static async Task<CloudBlobContainer> GetCloudBlobContainerAsync(
            this CloudBlobClient client,
            string containerName,
            BlobContainerPublicAccessType? accessType = null)
        {
            if (!new Regex("[a-z]+").IsMatch(containerName))
            {
                throw new ArgumentException("Container name must only container lowercase letters", nameof(containerName));
            }

            var container = client.GetContainerReference(containerName);
            if (await container.CreateIfNotExistsAsync())
            {
                if (accessType.HasValue)
                {
                    await container.SetPermissionsAsync(new BlobContainerPermissions()
                    {
                        PublicAccess = accessType.Value
                    });
                }
            }

            return container;
        }
    }
}
