using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Blob.Storage
{
    public class Container
    {
        void GetFile(string ContainerName, string BlobBlock, string FilePath, string StorageConnection)
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting(StorageConnection));
            var myClient = storageAccount.CreateCloudBlobClient();
            var container = myClient.GetContainerReference(ContainerName);
            container.CreateIfNotExists(Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Blob);

            var blockBlob = container.GetBlockBlobReference(BlobBlock);
            using (var fileStream = System.IO.File.OpenWrite(FilePath))
            {
                blockBlob.DownloadToStream(fileStream);
            }

           
        }
        void GetFiles()
        {
            var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(Microsoft.Azure.CloudConfigurationManager.GetSetting("StorageConnection"));
            var myClient = storageAccount.CreateCloudBlobClient();
            var container = myClient.GetContainerReference("images-backup");
            container.CreateIfNotExists(Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Blob);

            var list = container.ListBlobs();
            var blobs = list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>().Where(b => System.IO.Path.GetExtension(b.Name).Equals(".png"));

            foreach (var item in blobs)
            {
                string name = ((Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob)item).Name;
                Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
                string path = (@"C:\Users\mbcrump\Downloads\test\" + name);
                blockBlob.DownloadToFile(path, System.IO.FileMode.OpenOrCreate);
            }
        }

    }
}
