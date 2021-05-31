﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CreatorId { get; set; }
        public int ProjectTaskId { get; set; }
        public DateTime? DateCreated { get; set; }
        public ApplicationUser Creator { get; set; }
        public ProjectTask ProjectTask { get; set; }
        public Priority? Priority { get; set; }

        public Comment()
        {
            DateCreated = DateTime.Now;
        }

        public Comment(ProjectTask task)
        {
            DateCreated = DateTime.Now;
            ProjectTaskId = task.Id;
            ProjectTask = task;
        }
    }
}