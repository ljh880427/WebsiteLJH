﻿//[User][2]
using System.ComponentModel.DataAnnotations;

namespace WebsiteLJH
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "아이디")]
        [Required(ErrorMessage = "아이디를 입력하시오.")]
        [StringLength(25, MinimumLength = 3,
            ErrorMessage = "아이디는 3자 이상 25자 이하로 입력하시오.")]
        public string UserId { get; set; }

        [Display(Name = "비밀번호")]
        [Required(ErrorMessage = "비밀번호를 입력하시오.")]
        [StringLength(20, MinimumLength = 6,
            ErrorMessage = "비밀번호는 6자 이상 20자 이하로 입력하시오.")]
        public string Password { get; set; }
    }
}
