using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
namespace WebApplication1
{
    public partial class BlobStoreTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CloudStorageAccount csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobClient = csa.CreateCloudBlobClient();
            blobClient.Timeout = new TimeSpan(0, 20, 0);
            CloudBlobContainer container = blobClient.GetContainerReference("vivcontainer");
            try
            {
                container.CreateIfNotExist();
            }
            catch (Exception ex)
            {
                int i = 0;
            }
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            CloudBlob blob = container.GetBlobReference("inkscape3");

            using (var fileStream = System.IO.File.OpenRead(@"d:\inkscape.exe"))
            {
                try
                {
                    blob.UploadFromStream(fileStream);
                }
                catch (Exception ex)
                {
                    int i = 0;
                }
            }

            foreach (var blobItem in container.ListBlobs())
            {
                int i = 0;
            }
        }
    }
}