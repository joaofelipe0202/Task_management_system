﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string CreatorId { get; set; }
        public int TaskId { get; set; }
        public DateTime DateCreated { get; set; }
        public ApplicationUser Creator { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public Priority Priority { get; set; }

        public Comment()
        {
            DateCreated = DateTime.Now;
        }

        public Comment(ProjectTask task, Priority priority)
        {
            DateCreated = DateTime.Now;
            TaskId = task.Id;
            ProjectTask = task;
            Priority = priority;
        }           
    }
}