
using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Controllers
{
    public class WebContentController
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public WebContentController(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IEnumerable<object> GetCanvasBackgrounds()
        {
            string imagesPathName = "images";
            string canvasBackgroundsPathName = "canvasBackgrounds";

            string canvasBackgroundsSubpath = Path.Combine(imagesPathName, canvasBackgroundsPathName);
            var files = System.IO.Directory.GetFiles(
                Path.Combine(webHostEnvironment.WebRootPath, canvasBackgroundsSubpath)
                ).Select((path) =>
                {
                    return new { 
                        url = '/' + imagesPathName + '/' + canvasBackgroundsPathName + '/' + Path.GetFileName(path),
                        name = Path.GetFileName(path)
                    };

                });
            return files;
        }
    }
}
