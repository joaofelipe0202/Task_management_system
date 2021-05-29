using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class DeveloperViewModel
    {
        public string DeveloperId { get; set; }
        public List<Project> Projects { get; set; }
        public List<int> ProjectTasks { get; set; }        
    }
}