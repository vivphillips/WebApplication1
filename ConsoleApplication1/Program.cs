using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = csa.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("VivContainer");
            try
            {
                container.CreateIfNotExist();
            }
            catch (Exception ex)
            {
                int i = 0;
            }
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            CloudBlob blob = container.GetBlobReference("myBlob");

            using (var fileStream = System.IO.File.OpenRead(@"d:\TestSdf.sdf"))
            {
                blob.UploadFromStream(fileStream);
            }

            foreach (var blobItem in container.ListBlobs())
            {
                int i = 0;
            }

        }
    }
}
