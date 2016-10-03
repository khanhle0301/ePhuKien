using Model.EF;
using System;

namespace MyShop.Models
{
    [Serializable]
    public class RecentProductItem
    {
        public int ProductId { set; get; }
        public Product Product { set; get; }
        public DateTime CreateDate { set; get; }       
    }
}