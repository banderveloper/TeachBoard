using System.Net;
using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using TeachBoard.FileService.Configurations;
using TeachBoard.FileService.Interfaces;

namespace TeachBoard.FileService.Services;

public class FileService : IFileService
{
    private readonly BasicAWSCredentials _credentials;
    private readonly FileApiConfiguration _fileApiConfiguration;

    public FileService(FileApiConfiguration fileApiConfiguration)
    {
        _fileApiConfiguration = fileApiConfiguration;
        _credentials = new BasicAWSCredentials(_fileApiConfiguration.Key, _fileApiConfiguration.Secret);
    }

    public async Task<bool> UploadFileAsync(IFormFile file, string name)
    {
        using var client = new AmazonS3Client(_credentials, RegionEndpoint.EUCentral1);
        using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = name,
            BucketName = _fileApiConfiguration.BucketName,
        };

        var fileTransferUtility = new TransferUtility(client);
        await fileTransferUtility.UploadAsync(uploadRequest);

        return true;
    }

    public async Task<byte[]> DownloadFileAsync(string fileName)
    {
        using var client = new AmazonS3Client(_credentials, RegionEndpoint.EUCentral1);
        MemoryStream? ms = null;

        using var getObjectResponse = await client.GetObjectAsync(_fileApiConfiguration.BucketName, fileName);

        using (ms = new MemoryStream())
        {
            await getObjectResponse.ResponseStream.CopyToAsync(ms);
        }

        if (ms.ToArray().Length < 1)
            throw new FileNotFoundException(string.Format("The document '{0}' is not found", fileName));

        return ms.ToArray();
    }
}