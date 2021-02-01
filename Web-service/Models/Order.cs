using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_service.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int Sum { get; set; }
        public DateTime Date { get; set; }
    }
}