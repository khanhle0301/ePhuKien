﻿using System.Collections.Generic;

namespace MyShop.Models
{
    public class HomeViewModel
    {
        public IEnumerable<SlideViewModel> Slides { set; get; }       
        public IEnumerable<ProductViewModel> LastestProducts { set; get; }
        public IEnumerable<ProductViewModel> SaleProducts { set; get; }
        public IEnumerable<ProductViewModel> HotProducts { set; get; }
        public IEnumerable<ProductViewModel> HotSaleProducts { set; get; }
    }
}