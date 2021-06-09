using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebStore_MVC.ViewModels
{
    public class EmployeeViewModel
    {
        [HiddenInput(DisplayValue =false)]
        public int Id { get; set; }

        [Display(Name ="First Name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(20,MinimumLength =2, ErrorMessage ="The field should contain the range of min 2 and max 20 symbols")]
        [RegularExpression(@"[A-Z][a-z]+", ErrorMessage ="String is in wrong format (Should contain only characters starting with Capital)")]
        public string FirstName { get; set; }

        [Display(Name ="Last Name")]
        [Required(ErrorMessage ="This field is required")]
        [StringLength(20,MinimumLength =2, ErrorMessage ="The field should contain the range of min 2 and max 20 symbols")]
        [RegularExpression(@"[A-Z][a-z]+", ErrorMessage ="String is in wrong format (Should contain only characters starting with Capital)")]
        public string LastName { get; set; }

        [Display(Name ="Age")]
        [Range(18, 80, ErrorMessage = "Age restrictions. Should be between 18 and 80 to continue")]
        public int Age { get; set; }

        public EmployeeViewModel()
        {
        }
    }
}
