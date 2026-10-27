namespace TicketsGO.Services
{
    public interface IFileService
    {
        Task<string> SaveAvatarAsync(IFormFile file);
        Task<string> SaveEventoImagenAsync(IFormFile file);
        void DeleteFile(string? relativePath);
    }
}