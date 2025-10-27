using Microsoft.AspNetCore.Http;
using Service.Interface;

namespace Service;

public class FileStorageService : IFileStorageService
{
    public async Task CopyFileToServer(int entityId, string folderName, IFormFile? file, string? fileName = null)
    {
        if (entityId <= 0 || file is not { Length: > 0 }) return;

        var name = fileName ?? entityId + " - " + Guid.NewGuid();
        var filePath = Path.Combine(folderName, name + Path.GetExtension(file.FileName));

        if (!Directory.Exists(folderName))
            Directory.CreateDirectory(folderName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    public void DeleteFilesFromServer(int entityId, string folderName, bool allFolderFiles = false)
    {
        if (!File.Exists(folderName)) return;

        var directory = new DirectoryInfo(folderName);
        var files = allFolderFiles ? directory.GetFiles()
            : directory.GetFiles(entityId + "*");

        foreach (var file in files)
        {
            file.Delete();
        }
    }

    public IEnumerable<string> GetFilesUrlsFromServer(int entityId, string folderName, string? urlPrefix)
    {
        if (!Directory.Exists(folderName)) return Array.Empty<string>();

        var directory = new DirectoryInfo(folderName);
        var files = directory.GetFiles(entityId + " - *");

        return files.Select(f => urlPrefix + f.Name);
    }
}