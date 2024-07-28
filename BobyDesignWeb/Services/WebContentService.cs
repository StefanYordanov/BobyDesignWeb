using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Services
{
    public class WebContentService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public WebContentService(IWebHostEnvironment webHostEnvironment)
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
                    return new
                    {
                        url = '/' + imagesPathName + '/' + canvasBackgroundsPathName + '/' + Path.GetFileName(path),
                        name = Path.GetFileName(path)
                    };

                });
            return files;
        }
    }
}
