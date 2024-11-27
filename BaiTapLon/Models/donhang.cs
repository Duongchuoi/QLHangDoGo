namespace BaiTapLon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("donhang")]
    public partial class donhang
    {
        [Key]
        [StringLength(10)]
        public string MaDH { get; set; }

        public DateTime ThoiGian { get; set; }

        [Required]
        [StringLength(50)]
        public string tk { get; set; }

        [Required]
        [StringLength(10)]
        public string MaSP { get; set; }

        public int? SoLuong { get; set; }

        [Required]
        [StringLength(50)]
        public string DiaChi { get; set; }

        public virtual sanpham sanpham { get; set; }

        public virtual taikhoan taikhoan { get; set; }
    }
}
