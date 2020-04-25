using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class TasksHomeVM
    {

        public List<Task> TasksUnfinishedNotYetDueForUser { get; set; }

        public Users CurrentUser { get; set; }

        public List<Task> TasksOverDueForUser { get; set; }

        public List<Task> TasksAssignedByUser { get; set; }

        public List<Task> FinishedTasks { get; set; }

        public Task NewTask { get; set; }

        public List<Users> LstAllUsers { get; set; }

        public List<Event> LstAllEvents { get; set; }
    }
}