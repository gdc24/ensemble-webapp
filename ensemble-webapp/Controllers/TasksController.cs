using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ensemble_webapp.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        public ActionResult Index()
        {
            TasksHomeVM model = new TasksHomeVM();
            model.CurrentUser = Globals.LOGGED_IN_USER;

            GetDAL get = new GetDAL();
            get.OpenConnection();

            model.TasksNotYetDueForUser = get.GetTasksDueAfter(model.CurrentUser, DateTime.Now);

            IEnumerable<Task> difference = get.GetTasksByAssignedToUser(model.CurrentUser).Except(model.TasksNotYetDueForUser);

            model.TasksOverDueForUser = difference.ToList();

            model.TasksAssignedByUser = get.GetTasksByAssignedByUser(model.CurrentUser);

            return View("TasksHome", model);
        }

        public ActionResult TasksHome()
        {
            return RedirectToAction("Index");
        }

        public ActionResult NewTask(Task newTask)
        {
            InsertDAL insertDAL = new InsertDAL();
            insertDAL.OpenConnection();

            insertDAL.InsertTask(newTask);

            insertDAL.CloseConnection();

            return RedirectToAction("Index");
        }
    }
}