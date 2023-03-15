using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
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

    public Task UploadFileAsync(IFormFile file, string name)
    {
        throw new NotImplementedException();
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