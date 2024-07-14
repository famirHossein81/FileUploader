namespace FileUploader.Repositories.Contracts;
public interface IAws3Services
{
    Task<bool> UploadFile(IFormFile file, string folderName);

    Task<string> GeneratePreSignedURLAsync(string objectKey, string folerName);

    Task<bool> Remove(string objectKey, string folderName);


}