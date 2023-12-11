using BobyDesignWeb.Data.Entities;

namespace BobyDesignWeb.Utils
{
    public static class FormFileUtils
    {
        public static StoredFile ToEntity(this IFormFile file, string? customFileName = null)
        {
            using var stream = new MemoryStream();
            file.CopyTo(stream);
            var fileBytes = stream.ToArray();
            return new StoredFile()
            {
                FileName = customFileName ?? file.FileName,
                FileType = "." + file.FileName.Split(".").Last(),
                ContentType = file.ContentType,
                Content = fileBytes,
                Size = fileBytes.Length
            };
        }
    }
}
