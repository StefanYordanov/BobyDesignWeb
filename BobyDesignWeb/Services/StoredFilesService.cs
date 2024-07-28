using BobyDesignWeb.Data;
using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Services
{
    public class StoredFilesService
    {
        private ApplicationDbContext _context;

        public StoredFilesService(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Get(string fileName)
        //{
        //    var fileEntity = _context.StoredFiles.FirstOrDefault(s => s.FileName == fileName) ?? throw new ArgumentException("Невалидно име на файл");
        //    return File(fileEntity.Content, fileEntity.FileType);
        //}
    }
}
