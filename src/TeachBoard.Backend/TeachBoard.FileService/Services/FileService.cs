using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using TeachBoard.FileService.Configurations;
using TeachBoard.FileService.Interfaces;

namespace TeachBoard.FileService.Services;

public class FileService : IFileService
{
    private readonly BasicAWSCredentials _credentials;

    public FileService(FileApiConfiguration fileApiConfiguration)
    {
        _credentials = new BasicAWSCredentials(fileApiConfiguration.Key, fileApiConfiguration.Secret);
        // Console.WriteLine("Key: " + fileApiConfiguration.Key);
        // Console.WriteLine("Secret: " + fileApiConfiguration.Secret);
    }

    public async Task UploadFileAsync(IFormFile file, string name)
    {
        using var client = new AmazonS3Client(_credentials, RegionEndpoint.EUCentral1);
        using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = name,
            BucketName = "teachboard-bucket",
        };

        var fileTransferUtility = new TransferUtility(client);
        await fileTransferUtility.UploadAsync(uploadRequest);
    }

    public async Task Test()
    {
        using var client = new AmazonS3Client(_credentials, RegionEndpoint.EUCentral1);
        var bucketsResponse = await client.ListBucketsAsync();

        foreach (var bucket in bucketsResponse.Buckets)
        {
            Console.WriteLine(JsonSerializer.Serialize(bucket));
        }
    }
}