using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime NewTaskDateDue { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeSpan NewTaskTimeDue { get; set; }

        public Task NewTask { get; set; }

        public List<Users> LstAllUsers { get; set; }

        public List<Event> LstAllEvents { get; set; }

        public List<Event> LstAdminEvents { get; set; }
    }
}