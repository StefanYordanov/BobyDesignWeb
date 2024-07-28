using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Data;
using BobyDesignWeb.Models.Reports;
using BobyDesignWeb.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace BobyDesignWeb.Services
{
    public class ReportsService
    {
        private readonly ApplicationDbContext context;

        public ReportsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public OrderItemsReport GetWorkMaterialReport(int workMaterialId, string? fromDate, string? toDate, int? orderStatus, int? orderType, int? orderPaymentMethod)
        {
            var ordersQuery = OrderCraftingComponentsQuery(workMaterialId, fromDate?.ToDateTime(), toDate?.ToDateTime(), orderStatus, orderType, orderPaymentMethod);

            var orders = ordersQuery.ToList();

            return new OrderItemsReport()
            {
                Items = orders,
                TotalSoldQuantity = orders.Where(o => !o.IsDeposit).Sum(o => o.Quantity),
                TotalSoldRevenue = orders.Where(o => !o.IsDeposit).Sum(o => o.TotalComponentPrice),
                TotalDepositQuantity = orders.Where(o => o.IsDeposit).Sum(o => o.Quantity),
                TotalDepositRevenue = orders.Where(o => o.IsDeposit).Sum(o => o.TotalComponentPrice),
                TotalQuantity = orders.Sum(o => o.IsDeposit ? -o.Quantity : o.Quantity),
                TotalRevenue = orders.Sum(o => o.IsDeposit ? -o.TotalComponentPrice : o.TotalComponentPrice)
            };
        }

        [HttpGet]
        public byte[]? GetWorkMaterialReportFile(int workMaterialId, string? fromDate, string? toDate, int? orderStatus, int? orderType, int? orderPaymentMethod, out string fileName)
        {
            var report = GetWorkMaterialReport(workMaterialId, fromDate, toDate, orderStatus, orderType, orderPaymentMethod);

            WorkMaterial workMaterial = context.WorkMaterials.FirstOrDefault(wm => wm.WorkMaterialId == workMaterialId) ?? throw new ArgumentException("invalid work material");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using ExcelPackage pck = new();
            var columnIndexes = new
            {
                Id = 1,
                OrderCreatedOn = 2,
                IsDeposit = 3,
                WorkMaterialPrice = 4,
                TotalColIndex = 4,
                Quantity = 5,
                TotalComponentPrice = 6
            };
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(workMaterial.WorkMaterialName);
            int currentRow = 1;
            ws.Cells[currentRow, 1].Value = "Справка: " + workMaterial.WorkMaterialName;
            ws.Row(currentRow).Style.Font.Bold = true;
            currentRow++;
            ws.Cells[currentRow, columnIndexes.Id].Value = "ID Поръчка";
            ws.Cells[currentRow, columnIndexes.OrderCreatedOn].Value = "Дата";
            ws.Cells[currentRow, columnIndexes.IsDeposit].Value = "Депозит?";
            ws.Cells[currentRow, columnIndexes.WorkMaterialPrice].Value = "Цена на единица материал(лв.)";
            ws.Cells[currentRow, columnIndexes.Quantity].Value = $"Количество({workMaterial.WorkMaterialMeasuringUnit})";
            ws.Cells[currentRow, columnIndexes.TotalComponentPrice].Value = "Цена(лв.)";
            ws.Row(currentRow).Style.Font.Bold = true;
            currentRow++;
            foreach (var orderItem in report.Items)
            {
                ws.Cells[currentRow, columnIndexes.Id].Value = orderItem.Order.Id;
                ws.Cells[currentRow, columnIndexes.OrderCreatedOn].Value = orderItem.Order.OrderCreatedOn.ToDateOnlyModel().ToString();
                ws.Cells[currentRow, columnIndexes.IsDeposit].Value = orderItem.IsDeposit ? "Да" : "Не";
                ws.Cells[currentRow, columnIndexes.WorkMaterialPrice].Value = orderItem.WorkMaterialPrice;
                ws.Cells[currentRow, columnIndexes.Quantity].Value = orderItem.Quantity;
                ws.Cells[currentRow, columnIndexes.TotalComponentPrice].Value = orderItem.TotalComponentPrice;
                currentRow++;
            }
            currentRow++;
            currentRow++;
            ws.Cells[currentRow, columnIndexes.TotalColIndex].Value = "Тотал продажба:";
            ws.Cells[currentRow, columnIndexes.Quantity].Value = report.TotalSoldQuantity;
            ws.Cells[currentRow, columnIndexes.TotalComponentPrice].Value = report.TotalSoldRevenue;
            ws.Row(currentRow).Style.Font.Bold = true;
            currentRow++;
            ws.Cells[currentRow, columnIndexes.TotalColIndex].Value = "Тотал Депозит:";
            ws.Cells[currentRow, columnIndexes.Quantity].Value = report.TotalDepositQuantity;
            ws.Cells[currentRow, columnIndexes.TotalComponentPrice].Value = report.TotalDepositRevenue;
            ws.Row(currentRow).Style.Font.Bold = true;
            currentRow++;
            ws.Cells[currentRow, columnIndexes.TotalColIndex].Value = "Тотал баланс:";
            ws.Cells[currentRow, columnIndexes.Quantity].Value = report.TotalQuantity;
            ws.Cells[currentRow, columnIndexes.TotalComponentPrice].Value = report.TotalRevenue;
            ws.Row(currentRow).Style.Font.Bold = true;
            var fileContents = pck.GetAsByteArray();

            fileName = $"Справка({workMaterial.WorkMaterialName}).xlsx";
            return fileContents;
        }

        public IQueryable<OrderItemReportItem> OrderCraftingComponentsQuery(int workMaterialId, DateTime? fromDate, DateTime? toDate, int? orderStatus, int? orderType, int? orderPaymentMethod)
        {
            fromDate ??= DateTime.MinValue;
            toDate ??= DateTime.MaxValue;
            return context.OrderCraftingComponents.Where(occ => occ.WorkMaterialId == workMaterialId
                && occ.Order.OrderCreatedOn.Date >= fromDate && occ.Order.OrderCreatedOn.Date <= toDate
                && (orderStatus == null || (int)occ.Order.Status == orderStatus)
                && (orderType == null || (int)occ.Order.OrderType == orderType)
                && (orderPaymentMethod == null || (int)occ.Order.PaymentMethod == orderPaymentMethod)

            ).Select(occ => new OrderItemReportItem()
            {
                Id = occ.OrderCraftingComponentId,
                IsDeposit = occ.IsDeposit,
                Quantity = occ.WorkMaterialQuantity,
                TotalComponentPrice = occ.TotalComponentPrice,
                WorkMaterialPrice = occ.WorkMaterialPrice,
                WorkMaterial = new WorkMaterialModel()
                {
                    Id = occ.WorkMaterial.WorkMaterialId,
                    Name = occ.WorkMaterial.WorkMaterialName,
                    MeasuringUnit = occ.WorkMaterial.WorkMaterialMeasuringUnit,
                    PricingType = occ.WorkMaterial.WorkMaterialPricingType,
                    Quantity = occ.WorkMaterial.Quantity,
                    ReservedQuantity = occ.WorkMaterial.ReservedQuantity,
                    RelevantPrice = occ.WorkMaterial.WorkMaterialPriceForDates.OrderByDescending(x => x.Date)
                            .Where(x => x.Date < occ.Order.OrderCreatedOn)
                            .Select(x => new LatestWorkMaterialRelevantPriceModel()
                            {
                                LastUpdatedOn = x.Date,
                                SellingPrice = x.SellingPrice,
                                PurchasingPrice = x.PurchasingPrice,
                                WorkMaterialId = x.WorkMaterialId,
                                Id = x.WorkMaterialPriceForDateId,
                            }).FirstOrDefault()
                },
                Order = new() { Id = occ.OrderId, OrderCreatedOn = occ.Order.OrderCreatedOn }
            });
        }
    }
}
