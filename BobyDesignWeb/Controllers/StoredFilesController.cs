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

            //Stream stream = new MemoryStream(fileEntity.Content);
            //HttpResponseMessage response = new HttpResponseMessage { Content = new StreamContent(stream) };
            //response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            //response.Content.Headers.ContentLength = stream.Length;
            return File(fileEntity.Content, "image/png");
        }
    }
}
