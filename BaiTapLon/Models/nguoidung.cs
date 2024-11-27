namespace BaiTapLon.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nguoidung")]
    public partial class nguoidung
    {
        [StringLength(50)]
        public string TenND { get; set; }

        [StringLength(50)]
        public string Avatar { get; set; }

        public int? Tuoi { get; set; }

        [Key]
        [StringLength(50)]
        public string email { get; set; }

        [StringLength(10)]
        public string SDT { get; set; }

        [StringLength(50)]
        public string DiaChi { get; set; }
    }
}
