using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using BobyDesignWeb.Models.Reports;
using BobyDesignWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace BobyDesignWeb.Controllers
{
    [Authorize(Roles = UserRolesConstants.Admin)]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ReportsService _reportsService;

        public ReportController(ApplicationDbContext context, ReportsService reportsService)
        {
            this.context = context;
            this._reportsService = reportsService;
        }

        [HttpGet]
        public OrderItemsReport GetWorkMaterialReport(int workMaterialId, string? fromDate, string? toDate, int? orderStatus, int? orderType, int? orderPaymentMethod)
        {
            return _reportsService.GetWorkMaterialReport(workMaterialId, fromDate, toDate, orderStatus, orderType, orderPaymentMethod);
        }

        [HttpGet]
        public IActionResult GetWorkMaterialReportFile(int workMaterialId, string? fromDate, string? toDate, int? orderStatus, int? orderType, int? orderPaymentMethod)
        {
            var content = _reportsService.GetWorkMaterialReportFile(
                workMaterialId, fromDate, toDate, orderStatus, orderType, orderPaymentMethod, out string fileName);

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
