namespace BaiTapLon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("giohang")]
    public partial class giohang
    {
        [StringLength(10)]
        public string id { get; set; }

        [StringLength(10)]
        public string MaSP { get; set; }

        public int? SoLuong { get; set; }

        [StringLength(50)]
        public string tk { get; set; }

        public virtual sanpham sanpham { get; set; }

        public virtual taikhoan taikhoan { get; set; }
    }
}
