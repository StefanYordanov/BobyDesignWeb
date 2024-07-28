
using BobyDesignWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace BobyDesignWeb.Controllers
{
    public class WebContentController
    {
        private readonly WebContentService webContentService;

        public WebContentController(WebContentService webContentService)
        {
            this.webContentService = webContentService;
        }

        [HttpGet]
        public IEnumerable<object> GetCanvasBackgrounds()
        {
            return webContentService.GetCanvasBackgrounds();
        }
    }
}
