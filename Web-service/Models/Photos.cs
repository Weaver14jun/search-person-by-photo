using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_service.Models
{
    public class Photos
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] ImageData { get; set; }
        public int IdUser { get; set; }
    }
}