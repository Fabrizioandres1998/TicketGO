namespace TicketsGO.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileService> _logger;

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxAvatarSizeBytes = 2 * 1024 * 1024;   // 2 MB
        private const long MaxEventoSizeBytes = 5 * 1024 * 1024;   // 5 MB

        public FileService(IWebHostEnvironment env, ILogger<FileService> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> SaveAvatarAsync(IFormFile file)
        {
            return await SaveImageAsync(file, "avatars", MaxAvatarSizeBytes, "El avatar");
        }

        public async Task<string> SaveEventoImagenAsync(IFormFile file)
        {
            return await SaveImageAsync(file, "eventos", MaxEventoSizeBytes, "La imagen del evento");
        }

        private async Task<string> SaveImageAsync(IFormFile file, string subfolder, long maxSizeBytes, string descripcion)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo está vacío.");

            if (file.Length > maxSizeBytes)
                throw new InvalidOperationException($"{descripcion} no puede superar los {maxSizeBytes / (1024 * 1024)} MB.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException("Formato no permitido. Usá JPG, PNG o WEBP.");

            var folder = Path.Combine(_env.WebRootPath, "uploads", subfolder);
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation("Imagen guardada en {Path}", fullPath);

            return $"/uploads/{subfolder}/{fileName}";
        }

        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("Archivo eliminado: {Path}", fullPath);
            }
        }
    }
}