using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Task_Managment_System.Models.ViewModel
{
    public class TaskViewModel
    {
        public ProjectTask ProjectTask { get; set; }

        public int ProjectId { get; set; }

        [DisplayName("Project")]
        public IEnumerable<SelectListItem> ProjectList {get; set;}

        public string DeveloperId { get; set; }

        [DisplayName("Developer Assigned")]
        public IEnumerable<SelectListItem> DevelopersList { get; set; }
    }
}