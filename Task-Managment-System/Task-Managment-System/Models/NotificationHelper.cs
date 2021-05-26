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
    }
}