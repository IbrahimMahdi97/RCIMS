using Microsoft.AspNetCore.Http;

namespace Service.Interface;

public interface IFileStorageService
{
    Task CopyFileToServer(int entityId, string folderName, IFormFile? file, string? fileName = null);
    void DeleteFilesFromServer(int entityId, string folderName, bool allFolderFiles = false);
    IEnumerable<string> GetFilesUrlsFromServer(int entityId, string folderName, string? urlPrefix);
}