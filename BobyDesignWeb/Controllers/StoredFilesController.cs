using BobyDesignWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Net.Http.Headers;

namespace BobyDesignWeb.Controllers
{
    public class StoredFilesController: ControllerBase
    {
        private ApplicationDbContext _context;

        public StoredFilesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Get(string fileName)
        {
            var fileEntity = _context.StoredFiles.FirstOrDefault(s => s.FileName == fileName) ?? throw new ArgumentException("Невалидно име на файл");
            return File(fileEntity.Content, fileEntity.ContentType);
        }
    }
}
