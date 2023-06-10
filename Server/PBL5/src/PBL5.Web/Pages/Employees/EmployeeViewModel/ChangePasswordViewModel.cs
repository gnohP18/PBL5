using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace PBL5.Web.Pages.Employees
{
    public class ChangePasswordViewModel
    {
        [HiddenInput]
        [DisplayOrder(10001)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Employee:Password")]
        public string Password { get; set; }
    }
}