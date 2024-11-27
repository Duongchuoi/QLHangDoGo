namespace BaiTapLon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nhanvien")]
    public partial class nhanvien
    {
        [StringLength(50)]
        public string TenNV { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        public int? Tuoi { get; set; }

        [StringLength(10)]
        public string MaCV { get; set; }

        [Key]
        [StringLength(50)]
        public string email { get; set; }

        [StringLength(10)]
        public string SDT { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }

        public virtual chucvu chucvu { get; set; }
    }
}
