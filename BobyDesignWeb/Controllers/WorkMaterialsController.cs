using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using BobyDesignWeb.Services;
using BobyDesignWeb.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BobyDesignWeb.Controllers
{
    [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
    public class WorkMaterialsController: Controller
    {
        private readonly WorkMaterialService _workMaterialService;

        public WorkMaterialsController(WorkMaterialService workMaterialService)
        {
            this._workMaterialService = workMaterialService;
        }

        [HttpGet]
        public WorkMaterialModel[] GetAll(DateTime? pricingDate)
        {
            return _workMaterialService.GetAll(pricingDate);
        }

        [HttpGet]
        public WorkMaterialModel[] GetBySearch(DateTime? pricingDate, string? search)
        {
            return _workMaterialService.GetBySearch(pricingDate, search);
        }

        [HttpGet]
        public WorkMaterialModel? Get(int id, DateTime? pricingDate)
        {
            return _workMaterialService.Get(id, pricingDate);
        }

        [HttpPost]
        public async Task<ActionResult> InsertWorkMaterial([FromBody] WorkMaterialModel workMaterial)
        {
            var insertedModel = await _workMaterialService.InsertWorkMaterial(workMaterial);
            return Ok(insertedModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditWorkMaterial([FromBody] WorkMaterialModel workMaterial)
        {
            return Ok(await _workMaterialService.EditWorkMaterial(workMaterial));
        }

        [HttpPost]
        public async Task<ActionResult> InsertRelevantPrice([FromBody] LatestWorkMaterialRelevantPriceModel workMaterialRelevantPrice)
        {
            return Ok(await this._workMaterialService.InsertRelevantPrice(workMaterialRelevantPrice));
            
        }
    }
}
