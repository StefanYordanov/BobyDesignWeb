using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
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

        private readonly ApplicationDbContext _context;
        public WorkMaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public WorkMaterialModel[] GetAll(DateTime? pricingDate)
        {
            var query = from workMaterial in _context.WorkMaterials
                        select new WorkMaterialModel
                        {
                            Id = workMaterial.WorkMaterialId,
                            Name = workMaterial.WorkMaterialName,
                            MeasuringUnit = workMaterial.WorkMaterialMeasuringUnit,
                            PricingType = workMaterial.WorkMaterialPricingType,
                            Quantity = workMaterial.Quantity,
                            ReservedQuantity = workMaterial.ReservedQuantity,
                            RelevantPrice = _context.WorkMaterialPriceForDates.
                            Where(x => x.WorkMaterialId == workMaterial.WorkMaterialId)
                            .OrderByDescending(x => x.Date)
                            .Where(x => pricingDate == null || x.Date < pricingDate)
                            .Select(x => new LatestWorkMaterialRelevantPriceModel()
                            {
                                LastUpdatedOn = x.Date,
                                SellingPrice = x.SellingPrice,
                                PurchasingPrice = x.PurchasingPrice,
                                WorkMaterialId = x.WorkMaterialId,
                                Id = x.WorkMaterialPriceForDateId,
                            }).FirstOrDefault()
                        };

            return query.ToArray();
        }

        [HttpGet]
        public WorkMaterialModel Get(int id, DateTime? pricingDate)
        {
            var query = from workMaterial in _context.WorkMaterials
                        select new WorkMaterialModel
                        {
                            Id = workMaterial.WorkMaterialId,
                            Name = workMaterial.WorkMaterialName,
                            MeasuringUnit = workMaterial.WorkMaterialMeasuringUnit,
                            PricingType = workMaterial.WorkMaterialPricingType,
                            Quantity = workMaterial.Quantity,
                            ReservedQuantity = workMaterial.ReservedQuantity,
                            RelevantPrice = _context.WorkMaterialPriceForDates.
                            Where(x => x.WorkMaterialId == workMaterial.WorkMaterialId)
                            .OrderByDescending(x => x.Date)
                            .Where(x => pricingDate == null || x.Date < pricingDate)
                            .Select(x => new LatestWorkMaterialRelevantPriceModel()
                            {
                                LastUpdatedOn = x.Date,
                                SellingPrice = x.SellingPrice,
                                PurchasingPrice = x.PurchasingPrice,
                                WorkMaterialId = x.WorkMaterialId,
                                Id = x.WorkMaterialPriceForDateId,
                            }).FirstOrDefault()
                        };

            return query.FirstOrDefault(wm => wm.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> InsertWorkMaterial([FromBody] WorkMaterialModel workMaterial)
        {
            var newWorkMaterialEntry = new WorkMaterial()
            {
                WorkMaterialName = workMaterial.Name,
                WorkMaterialMeasuringUnit = workMaterial.MeasuringUnit,
                WorkMaterialPricingType = workMaterial.PricingType,
                Quantity = workMaterial.Quantity,
            };

            this._context.WorkMaterials.Add(newWorkMaterialEntry);
            await this._context.SaveChangesAsync();
            workMaterial.Id = newWorkMaterialEntry.WorkMaterialId;

            if (workMaterial.RelevantPrice != null)
            {
                workMaterial.RelevantPrice.WorkMaterialId = newWorkMaterialEntry.WorkMaterialId;
                var newWorkMaterialPricePerDateEntry = new WorkMaterialPriceForDate()
                {
                    WorkMaterialId = workMaterial.RelevantPrice.WorkMaterialId,
                    SellingPrice = workMaterial.RelevantPrice.SellingPrice,
                    PurchasingPrice = workMaterial.RelevantPrice.PurchasingPrice,
                    Date = DateTime.UtcNow.ToBulgarianDateTime()
                };

                this._context.WorkMaterialPriceForDates.Add(newWorkMaterialPricePerDateEntry);
                await this._context.SaveChangesAsync();
                workMaterial.RelevantPrice.Id = newWorkMaterialPricePerDateEntry.WorkMaterialPriceForDateId;
            }

            return Ok(workMaterial);
        }

        [HttpPost]
        public async Task<ActionResult> EditWorkMaterial([FromBody] WorkMaterialModel workMaterial)
        {
            WorkMaterial? existingWorkMaterialEntry = this._context.WorkMaterials
                .FirstOrDefault(x => x.WorkMaterialId == workMaterial.Id);

            if (existingWorkMaterialEntry == null)
            {
                return NotFound("Няма материал със зададеното Id в базата данни");
            }

            existingWorkMaterialEntry.WorkMaterialName = workMaterial.Name;
            existingWorkMaterialEntry.WorkMaterialMeasuringUnit = workMaterial.MeasuringUnit;
            existingWorkMaterialEntry.WorkMaterialPricingType = workMaterial.PricingType;

            if (workMaterial.RelevantPrice != null)
            {
                var newWorkMaterialPricePerDateEntry = new WorkMaterialPriceForDate()
                {
                    WorkMaterialId = workMaterial.RelevantPrice.WorkMaterialId,
                    SellingPrice = workMaterial.RelevantPrice.SellingPrice,
                    PurchasingPrice = workMaterial.RelevantPrice.PurchasingPrice,
                    Date = DateTime.UtcNow.ToBulgarianDateTime()
                };
                this._context.WorkMaterialPriceForDates.Add(newWorkMaterialPricePerDateEntry);
                await this._context.SaveChangesAsync();
                workMaterial.RelevantPrice.LastUpdatedOn = newWorkMaterialPricePerDateEntry.Date;
                workMaterial.RelevantPrice.Id = newWorkMaterialPricePerDateEntry.WorkMaterialPriceForDateId;
            }
            else
            {
                await this._context.SaveChangesAsync();
            }            
                        
            return Ok(workMaterial);
        }

        [HttpPost]
        public async Task<ActionResult> InsertRelevantPrice([FromBody] LatestWorkMaterialRelevantPriceModel workMaterialRelevantPrice)
        {
            var bulgarianTime = DateTime.UtcNow.ToBulgarianDateTime();
               
            workMaterialRelevantPrice.LastUpdatedOn = bulgarianTime;
            var newEntry = new WorkMaterialPriceForDate()
            {
                Date = workMaterialRelevantPrice.LastUpdatedOn,
                WorkMaterialId = workMaterialRelevantPrice.WorkMaterialId,
                SellingPrice = workMaterialRelevantPrice.SellingPrice,
                PurchasingPrice = workMaterialRelevantPrice.PurchasingPrice
            };

            this._context.WorkMaterialPriceForDates.Add(newEntry);
            await this._context.SaveChangesAsync();
            workMaterialRelevantPrice.Id = newEntry.WorkMaterialPriceForDateId;

            return Ok(workMaterialRelevantPrice);
        }
    }
}
