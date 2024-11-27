using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaiTapLon.Models
{
    public class StaffViewModel
    {
        [Key]
        [StringLength(10)]
        public string MaNV { get; set; }

        [StringLength(50)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        public int? Tuoi { get; set; }

        [StringLength(10)]
        public string MaCV { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(10)]
        public string SDT { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        [StringLength(50)]
        public string Quyen { get; set; }

        [StringLength(50)]
        public string MatKhau { get; set; }

        public virtual chucvu chucvu { get; set; }
    }
}