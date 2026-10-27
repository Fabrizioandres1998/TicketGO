namespace TicketsGO.Services
{
    public interface IFileService
    {
        Task<string> SaveAvatarAsync(IFormFile file);
        void DeleteFile(string? relativePath);
    }
}