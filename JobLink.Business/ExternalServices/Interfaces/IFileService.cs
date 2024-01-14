using Microsoft.AspNetCore.Http;

namespace JobLink.Business.ExternalServices.Interfaces;

public interface IFileService
{
    Task<string> UploadAsync(IFormFile file, string path, string contentType = "image", int mb = 3);
    Task SaveAsync(IFormFile file, string path);
    void Delete(string path);
}

