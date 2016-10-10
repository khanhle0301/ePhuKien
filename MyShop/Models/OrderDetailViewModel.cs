namespace MyShop.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { set; get; }

        public int ProductID { set; get; }

        public int Quantitty { set; get; }

        public decimal Price { set; get; }

        public string Note { set; get; }
    }
}