using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaiTapLon.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string TenND { get; set; }
        [Required]
        public int Tuoi { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string SDT { get; set; }
        [Required]
        public string DiaChi { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}