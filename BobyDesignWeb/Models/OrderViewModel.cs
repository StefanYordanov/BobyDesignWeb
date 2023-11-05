﻿namespace BobyDesignWeb.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public CustomerViewModel Customer { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime FinishingDate { get; set; }
        public ICollection<OrderCraftingComponentViewModel> CraftingComponents { get; set; } = new List<OrderCraftingComponentViewModel>();
        public decimal LaborPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Deposit { get; set; }
        public OrderStatus Status { get; set; }
        public UserViewModel ShopUser { get; set; } = new UserViewModel();
    }

    public class OrderCraftingComponentViewModel
    {
        public decimal WorkMaterial {  get; set; }
        public decimal WorkMaterialPrice { get; set; }
        public decimal Quantity { get; set; }
    }

    public enum OrderStatus
    {
        Opened = 0,
        Closed = 1
    }
}
