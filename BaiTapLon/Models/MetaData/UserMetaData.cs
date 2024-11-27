using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaiTapLon.Models.MetaData
{
    public class UserMetaData
    {
        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string tk { get; set; }
        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu không được ngắn hơn 6 kí tự!")]
        public string mk { get; set; }
    }
}