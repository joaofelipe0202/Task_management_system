
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class NotificationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void Create(string userId, string title, string contents, NotificationType type)
        {
            if (userId == null || title == null || contents == null)
                return;
            var user = db.Users.Find(userId);
            if (user == null)
                return;

            var notification = new Notification(userId, title, contents, type);
            db.Notifications.Add(notification);
            db.SaveChanges();
        }

        public static void Delete(int id)
        {
            var notification = db.Notifications.Find(id);
            if (notification == null)
                return;

            db.Notifications.Remove(notification);
            db.SaveChanges();
        }

        public void Update(int notificationId)
        {
            var notification = db.Notifications.Find(notificationId);
            if (notification == null)
                return;
            db.Entry(notification).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool IsOverdue(int taskId)
        {
            var task = db.Tasks.Find(taskId);
            if (task == null)
                return false;

            if (DateTime.Now.CompareTo(task.Deadline) > 0)
            {
                var notification = db.Notifications.FirstOrDefault(n => n.TaskId == task.Id && n.Type == NotificationType.Overdue);
                if (notification == null)
                {
                    notification = new Notification(task.AssignedUser.Id, $"'{task.Title}' is Overdue", "", NotificationType.Overdue);
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }
                return true;
            }

            return false;
        }

        public bool IsOverbudget(int projectId)
        {
            var project = db.Projects.Find(projectId);
            if (project == null)
                return false;

            if (project.ActualCost > project.Budget)
            {
                var notification = db.Notifications.FirstOrDefault(n => n.ProjectId == project.Id && n.Type == NotificationType.Overbudget);
                if (notification == null)
                {
                    notification = new Notification(project.Creator.Id, $"'{project.Name}' is Overbudget", "", NotificationType.Overbudget);
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }
                return true;
            }

            return false;
        }

        public bool IsDayBeforeDeadline(int id, bool IsTask)
        {
            if (IsTask)
            {
                var task = db.Tasks.Find(id);

                if (task == null)
                    return false;

                if (task.Deadline.Date.AddDays(-1) == DateTime.Now.Date)
                {
                    var notification = db.Notifications.FirstOrDefault(n => n.TaskId == task.Id && n.Type == NotificationType.Notice);
                    if (notification == null)
                    {
                        notification = new Notification(task.AssignedUser.Id, $"'{task.Title}' is due in a day", "", NotificationType.Notice);
                        db.Notifications.Add(notification);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            else
            {
                var project = db.Projects.Find(id);

                if (project == null)
                    return false;

                if (project.Deadline.Date.AddDays(-1) == DateTime.Now.Date)
                {
                    var notification = db.Notifications.FirstOrDefault(n => n.TaskId == project.Id && n.Type == NotificationType.Notice);
                    if (notification == null)
                    {
                        notification = new Notification(project.Creator.Id, $"'{project.Name}' is due in a day", "", NotificationType.Notice);
                        db.Notifications.Add(notification);
                        db.SaveChanges();
                    }
                    return true;
                }
            }

            return false;
        }

        public bool IsComplete(int taskId)
        {
            var task = db.Tasks.Find(taskId);
            if (task == null)
                return false;

            if (DateTime.Now.CompareTo(task.Deadline) > 0)
            {
                var notification = db.Notifications.FirstOrDefault(n => n.TaskId == task.Id && n.Type == NotificationType.Complete);
                if (notification == null)
                {
                    notification = new Notification(task.AssignedUser.Id, $"'{task.Title}' is Complete", $"'{task.Title}' was completed at {DateTime.Now}", NotificationType.Complete);
                    db.Notifications.Add(notification);
                    db.SaveChanges();
                }
                return true;
            }

            return false;
        }

        public bool UrgentNote(int taskId, string title, string contents)
        {
            var task = db.Tasks.Find(taskId);
            if (task == null)
                return false;

            var notification = new Notification(task.AssignedUser.Id, $"URGENT '{title}'", contents, NotificationType.Urgent);
            db.Notifications.Add(notification);
            return true;
        }
    }
}