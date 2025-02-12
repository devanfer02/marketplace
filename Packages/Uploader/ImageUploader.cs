namespace Marketplace.Packages.Uploader
{
    public interface IImageUploader
    {
        public Task<string> Update(IFormFile file, string oldImagePath);
        public Task<string> Upload(IFormFile file);
        public void Delete(string path);
    }

    public class ImageUploader : IImageUploader
    {
        public async Task<string> Update(IFormFile file, string oldImagePath)
        {
            var imageUrl = await Upload(file);
            Delete(oldImagePath);

            return imageUrl;
        }
        public async Task<string> Upload(IFormFile file)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return "/uploads/" + uniqueFileName;
        }
        public void Delete(string path)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path.TrimStart('/'));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);    
            }
        }
    }
}
