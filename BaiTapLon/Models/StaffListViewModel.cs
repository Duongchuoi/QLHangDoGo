using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLon.Models
{
    public class StaffListViewModel
    {
        public List<nhanvien> NhanViens { get; set; }
        public List<taikhoan> TaiKhoans { get; set; }
        public List<chucvu> ChucVus { get; set; }
    }
}