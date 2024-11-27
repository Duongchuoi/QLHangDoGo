using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLon.Models
{
    public class RevenueByMonth
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}