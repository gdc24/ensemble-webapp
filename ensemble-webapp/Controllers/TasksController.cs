using ensemble_webapp.Database;
using ensemble_webapp.Models;
using ensemble_webapp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ensemble_webapp.Models.Task;

namespace ensemble_webapp.Controllers
{
    public class TasksController : Controller
    {
        // GET: Tasks
        public ActionResult Index()
        {
            if (!Globals.LOGIN_STATUS)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                TasksHomeVM model = new TasksHomeVM();
                model.CurrentUser = Globals.LOGGED_IN_USER;

                var taskEqualityComparer = new TaskEqualityComparer();

                GetDAL get = new GetDAL();
                get.OpenConnection();

                model.TasksUnfinishedNotYetDueForUser = get.GetUnfinishedTasksDueAfter(model.CurrentUser, DateTime.Now);
                model.LstAllUsers = get.GetAllUsers();

                model.FinishedTasks = get.GetFinishedTasks(model.CurrentUser);

                IEnumerable<Task> difference = get.GetTasksByAssignedToUser(model.CurrentUser).Except(model.TasksUnfinishedNotYetDueForUser, taskEqualityComparer).Except(model.FinishedTasks, taskEqualityComparer);

                model.TasksOverDueForUser = difference.ToList();

                model.TasksAssignedByUser = get.GetTasksByAssignedByUser(model.CurrentUser);

                model.LstAllEvents = get.GetAllEvents();

                get.CloseConnection();

                return View("TasksHome", model);
            }
        }

        public ActionResult TasksHome()
        {
            return RedirectToAction("Index");
        }

        public ActionResult MarkTaskFinished(int intTaskID)
        {
            InsertDAL insertDAL = new InsertDAL();
            insertDAL.OpenConnection();

            insertDAL.MarkTaskAsComplete(intTaskID);

            insertDAL.CloseConnection();

            return RedirectToAction("Index");
        }

        public ActionResult AddTask(TasksHomeVM vm)
        {
            InsertDAL insertDAL = new InsertDAL();

            vm.NewTask.UserAssignedBy = Globals.LOGGED_IN_USER;

            insertDAL.OpenConnection();

            insertDAL.InsertTask(vm.NewTask);

            insertDAL.CloseConnection();

            return RedirectToAction("Index");
        }
    }
}