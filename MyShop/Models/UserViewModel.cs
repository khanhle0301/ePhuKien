using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyShop.Models
{
    public class UserViewModel
    {
        public int ID { set; get; }
      
        public string UserName { set; get; }
    
        public string Password { set; get; }

        public string GroupID { set; get; }

        public string Name { set; get; }

        public string Address { set; get; }
    
        public string Email { set; get; }
    
        public string Phone { set; get; }
    }
}