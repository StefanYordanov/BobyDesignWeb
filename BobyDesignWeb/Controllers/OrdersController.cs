using BobyDesignWeb.Data;
using BobyDesignWeb.Data.Entities;
using BobyDesignWeb.Models;
using BobyDesignWeb.Services;
using BobyDesignWeb.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BobyDesignWeb.Controllers
{
    [Authorize(Roles = UserRolesConstants.SellerAndAdmin)]
    public class OrdersController : Controller
    {
        private readonly OrdersService _ordersService;

        public OrdersController(OrdersService ordersService)
        {
            this._ordersService = ordersService;
        }

        [HttpGet]
        public IEnumerable<OrderViewModel> GetOrders(string? fromDate, string? toDate, string? searchPhrase, int? customerId, int? status, int? type)
        {
            return _ordersService.OrdersQuery(fromDate?.ToDateTime(), toDate?.ToDateTime(), searchPhrase, customerId, status, type).ToList();
        }

        [HttpGet]
        public OrderViewModel GetOrder(int orderId)
        {
            return _ordersService.GetOrderDetails(orderId) ?? throw new Exception("Няма поръчка със зададеното id");
        }

        [HttpGet]
        public PageViewModel<OrderViewModel> GetOrdersPagination(int page, string? fromDate, string? toDate, string? searchPhrase, int? customerId, int? status, int? type)
        {
            return _ordersService.GetOrdersPagination(page, fromDate, toDate, searchPhrase, customerId, status, type);
        }

        [HttpPost]
        public ActionResult Pay([FromBody] PayOrderQuery query)
        {
            var result = _ordersService.Pay(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder([FromForm] string model, IFormFile? sketchBlob)
        {
            var order = JsonConvert.DeserializeObject<SubmitEditOrderModel>(model);

            if (order == null || order.Model == null)
            {
                throw new ArgumentNullException(nameof(model), "Невалиден модел");
            }

            var updatedOrder = await this._ordersService.UpdateOrder(order, sketchBlob);
            return Ok(updatedOrder);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOrder([FromForm] string model, IFormFile? sketchBlob)
        {
            var order = JsonConvert.DeserializeObject<OrderViewModel>(model);

            if (order == null)
            {
                throw new ArgumentNullException(nameof(model), "Невалиден модел");
            }

            var insertedOrder = await this._ordersService.InsertOrder(order, sketchBlob, User);
            return Ok(insertedOrder);
        }
    }
}
