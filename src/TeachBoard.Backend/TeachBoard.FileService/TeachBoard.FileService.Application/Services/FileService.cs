using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TeachBoard.FileService.Application.Configurations;
using TeachBoard.FileService.Application.Exceptions;
using TeachBoard.FileService.Application.Interfaces;

namespace TeachBoard.FileService.Application.Services;

public class FileService : IFileService
{
    private readonly BasicAWSCredentials _credentials;
    private readonly FileApiConfiguration _fileApiConfiguration;
    private readonly ILogger<FileService> _logger;

    public FileService(FileApiConfiguration fileApiConfiguration, ILogger<FileService> logger)
    {
        _fileApiConfiguration = fileApiConfiguration;
        _logger = logger;
        _credentials = new BasicAWSCredentials(_fileApiConfiguration.Key, _fileApiConfiguration.Secret);
    }

    public async Task<bool> UploadFileAsync(IFormFile file, string fileName)
    {
        if (file.Length < 1)
            throw new BadRequestApiException
            {
                ErrorCode = ErrorCode.FileEmpty,
                PublicErrorMessage = "File is empty"
            };

        if (string.IsNullOrEmpty(fileName))
            throw new BadRequestApiException
            {
                ErrorCode = ErrorCode.FileNameEmpty,
                PublicErrorMessage = "File name to upload is empty"
            };
        
        using var client = new AmazonS3Client(_credentials, RegionEndpoint.EUCentral1);
        using var newMemoryStream = new MemoryStream();
        await file.CopyToAsync(newMemoryStream);

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = newMemoryStream,
            Key = fileName,
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
            throw new HostingErrorException
            {
                ErrorCode = ErrorCode.HostingFileNotFound,
                PublicErrorMessage = "File at hosting not found"
            };

        return ms.ToArray();
    }
}