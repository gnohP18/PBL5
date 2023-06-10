using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PBL5.Enum;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace PBL5.Web.Pages.Employees
{
    public class UpdateEmployeeViewModel
    {
        [HiddenInput]
        [DisplayOrder(10001)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Employee:FirstName")]
        public string Name { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "Employee:DateOfBirth")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Employee:Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Employee:IdentityCard")]
        public string IdentityCard { get; set; }

        [Required]
        [Display(Name = "Employee:Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Employee:Phone")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Employee:EmployeeCode")]
        public string EmployeeCode { get; set; }
    }
}