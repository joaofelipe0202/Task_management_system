﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string Name { get; set; }
        public double Budget { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Deadline { get; set; }
        public double ActualCost { get; set; }
        public double PercentageCompleted { get; set; }
        public bool Complete { get; set; }

        public ApplicationUser Creator { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }

        public Project(string name, double budget, DateTime deadline, string creatorId)
        {
            Name = name;
            Budget = budget;
            Deadline = deadline;
            CreatorId = creatorId;

            Members = new HashSet<ApplicationUser>();
            Tasks = new HashSet<ProjectTask>();
        }
    }
}