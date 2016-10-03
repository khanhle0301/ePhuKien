using System;

namespace MyShop.Models
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public int ProductId { set; get; }
        public ProductViewModel Product { set; get; }
        public int Quantity { set; get; }
        public string Color { set; get; }
    }
}