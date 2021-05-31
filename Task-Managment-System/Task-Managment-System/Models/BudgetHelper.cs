using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class BudgetHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public BudgetHelper(ApplicationDbContext database)
        {
            db = database;
        }

        public bool UpdateActualPriceSinceDateCreated(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return false;

            double dailyCost = project.Members.Sum(m => m.DailySalary).Value;
            int numDays = Convert.ToInt32((project.DateCreated - DateTime.Now).TotalDays);
            project.ActualCost = dailyCost * numDays;
            db.SaveChanges();

            return true;
        }

        public bool UpdateActualPriceForDay(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return false;

            double dailyCost = project.Members.Sum(m => m.DailySalary).Value;
            project.ActualCost += dailyCost;
            db.SaveChanges();

            return true;
        }
    }
}