using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLon.Models
{
    public class ProductViewModel
    {
        public sanpham Product { get; set; }
        public bool IsTopSelling { get; set; }
    }
}