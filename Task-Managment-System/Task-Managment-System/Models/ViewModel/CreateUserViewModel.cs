using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Task_Managment_System.Models.ViewModel
{
    public class CreateUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public double? DailySalary { get; set; }
        public string RoleName { get; set; }
        public ICollection<SelectListItem> Roles { get; set; }

        public CreateUserViewModel() { }

        public CreateUserViewModel(ApplicationUser user, List<SelectListItem> roles)
        {
            this.UserName = user.Email;
            this.Email = user.Email;
            this.DailySalary = user.DailySalary;
            this.Roles = roles;
        }
    }
}