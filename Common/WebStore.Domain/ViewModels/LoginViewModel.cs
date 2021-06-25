using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore_MVC.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
       
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        
        [HiddenInput(DisplayValue=false)]
        public string ReturnUrl { get; set; }
        
    }
}
