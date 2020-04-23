using ensemble_webapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.ViewModels
{
    public class TasksHomeVM
    {

        public List<Task> TasksNotYetDueForUser { get; set; }


        public List<Task> TasksOverDueForUser { get; set; }


        public Task NewTask { get; set; }
    }
}