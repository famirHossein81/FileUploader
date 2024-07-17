using Amazon.S3;
using Amazon.S3.Model;
using FileUploader.Repositories.Contracts;
namespace FileUploader.Services;

public class Aws3Services : IAws3Services
{
    private const string BUCKET_NAME = "ui-ac-spotify";
    private readonly IConfiguration _configuration;
    private readonly IAmazonS3 _s3Client;

    public Aws3Services(IConfiguration configuration)
    {
        _configuration = configuration;

        var credentials = new Amazon.Runtime.BasicAWSCredentials(
            _configuration.GetSection("CDN").GetSection("AccessKey").Value,
            _configuration.GetSection("CDN").GetSection("SecretKey").Value
        );

        var config = new AmazonS3Config { ServiceURL = _configuration.GetSection("CDN").GetSection("ServiceURL").Value };
        _s3Client = new AmazonS3Client(credentials, config);
    }

    public async Task<bool> UploadFile(IFormFile file, string folderName)
    {
        try
        {
            using (var stream = file.OpenReadStream())
            {
                PutObjectRequest request = new()
                {
                    BucketName = BUCKET_NAME,
                    Key = $"{folderName}/{file.FileName}",
                    InputStream = stream,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.Private
                };

                request.Metadata.Add("x-amz-meta-title", "mytitle");
                PutObjectResponse response = await _s3Client.PutObjectAsync(request);
                return true;
                // return $"https://{BUCKET_NAME}.s3.ir-thr-at1.arvanstorage.ir/{file.FileName}";
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error in uploading...");
            return false;
        }
    }

    public async Task<bool> Remove(string objectKey, string folderName)
    {
        try
        {
            DeleteObjectsRequest request = new()
            {
                BucketName = BUCKET_NAME,
                Objects = new List<KeyVersion> { new KeyVersion() { Key = $"{folderName}/{objectKey}", VersionId = null } }
            };

            DeleteObjectsResponse response = await _s3Client.DeleteObjectsAsync(request);
            return true;
        }
        catch (AmazonS3Exception amazonS3Exception)
        {
            Console.WriteLine("An AmazonS3Exception was thrown. Exception: " + amazonS3Exception.ToString());
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
            return false;
        }
    }

    public async Task<string> GeneratePreSignedURLAsync(string objectKey, string folderName)
    {
        GetPreSignedUrlRequest request = new()
        {
            BucketName = BUCKET_NAME,
            Key = $"{folderName}/{objectKey}",
            Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
            Verb = HttpVerb.GET
        };

        string url = await _s3Client.GetPreSignedURLAsync(request);
        return url;
    }
}