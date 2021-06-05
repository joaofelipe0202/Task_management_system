using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class TimeHelper
    {
        private ApplicationDbContext db { get; set; }
        private NotificationHelper nh { get; set; }
        private BudgetHelper bh { get; set; }

        public TimeHelper()
        {
            db = new ApplicationDbContext();
            nh = new NotificationHelper(db);
            bh = new BudgetHelper(db);
        }

        public void RunUpdateTick()
        {
            foreach (var project in db.Projects)
            {
                bh.UpdateActualPriceSinceLastBudgetUpdate(project.Id);
                nh.IsDayBeforeDeadline(project.Id, false);
                nh.IsOverbudget(project.Id);
                nh.IsOverdue(project.Id, false);
            }
            db.SaveChanges();

            foreach (var task in db.Projects)
            {
                nh.IsDayBeforeDeadline(task.Id, true);
                nh.IsOverdue(task.Id, true);
            }
            db.SaveChanges();
        }
    }
}