using ensemble_webapp.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Database
{
    public class InsertDAL
    {
        // global connection used to run all queries
        NpgsqlConnection conn;

        public InsertDAL()
        {
            OpenConnection();
        }

        public void OpenConnection()
        {
            conn = DatabaseConnection.GetConnection();
            conn.Open();
        }

        public void CloseConnection()
        {
            conn.Close();
        }


        public bool InsertGroup(Group group)
        {
            // define a query
            string query = "INSERT INTO public.groups(" +
                "\"strName\")" +
                " VALUES(@strName); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("strName", group.StrName);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertEvent(Event paramEvent)
        {
            // define a query
            string query = "INSERT INTO public.events(" +
                "\"strName\", \"dtmDate\", \"strLocation\", \"intGroupID\")" +
                " VALUES(@strName, @dtmDate, @strLocation, @intGroupID); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("strName", paramEvent.StrName);
            cmd.Parameters.AddWithValue("dtmDate", paramEvent.DtmDate);
            cmd.Parameters.AddWithValue("strLocation", paramEvent.StrLocation);
            cmd.Parameters.AddWithValue("intGroupID", paramEvent.Group.IntGroupID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool ChangeEmailAndPhone(Users currentUser)
        {
            // define a query
            string query = "UPDATE \"users\" SET" +
                " \"strEmail\" = @strEmail," +
                " \"strPhone\" = @strPhone" +
                " WHERE \"intUserID\" = @intUserID";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("strEmail", currentUser.StrEmail);
            cmd.Parameters.AddWithValue("strPhone", currentUser.StrPhone);
            cmd.Parameters.AddWithValue("intUserID", currentUser.IntUserID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertCallboard(Callboard callboard)
        {
            // define a query
            string query = "INSERT INTO public.callboard(" + 
                "\"strSubject\", \"strNote\", \"dtmDateTime\", \"intPostedByUserID\", \"intEventID\")" +
                " VALUES(@strSubject, @strNote, @dtmDateTime, @intPostedByUserID, @intEventID);";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("strSubject", callboard.StrSubject);
            cmd.Parameters.AddWithValue("strNote", callboard.StrNote);
            cmd.Parameters.AddWithValue("dtmDateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("intPostedByUserID", Globals.LOGGED_IN_USER.IntUserID);
            cmd.Parameters.AddWithValue("intEventID", callboard.Event.IntEventID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool MarkTaskAsComplete(int intTaskID)
        {
            // define a query
            string query = "UPDATE \"tasks\" SET \"ysnIsFinished\" = true" +
                " WHERE \"intTaskID\" = " + intTaskID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertToUserEvents(Event e, Users editedUserProfile)
        {
            // define a query
            string query = "INSERT INTO public.\"userEvents\"(" +
                " \"intEventID\", \"intUserID\")" +
                " VALUES(@intEventID, @intUserID);";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("intEventID", e.IntEventID);
            cmd.Parameters.AddWithValue("intUserID", editedUserProfile.IntUserID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertConflict(Conflict conflict)
        {
            // define a query
            string query = "INSERT INTO public.\"conflicts\"(" +
                " \"dtmStartDateTime\", \"dtmEndDateTime\", \"intUserID\")" +
                " VALUES(@dtmStartDateTime, @dtmEndDateTime, @intUserID);";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("dtmStartDateTime", conflict.DtmStartDateTime);
            cmd.Parameters.AddWithValue("dtmEndDateTime", conflict.DtmEndDateTime);
            cmd.Parameters.AddWithValue("intUserID", conflict.User.IntUserID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertTask(Task task)
        {
            // define a query
            string query = "INSERT INTO public.tasks(" +
                "\"dtmDue\", \"strName\", \"strAttachment\", \"intAssignedToUserID\", \"intAssignedByUserID\", \"intEventID\", \"ysnIsFinished\")" +
                " VALUES(@dtmDue, @strName, @strAttachment, @intAssignedToUserID, @intAssignedByUserID, @intEventID, false); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("dtmDue", task.DtmDue);
            cmd.Parameters.AddWithValue("strName", task.StrName);
            cmd.Parameters.AddWithValue("strAttachment", task.StrAttachment);
            cmd.Parameters.AddWithValue("intAssignedToUserID", task.UserAssignedTo.IntUserID);
            cmd.Parameters.AddWithValue("intAssignedByUserID", task.UserAssignedBy.IntUserID);
            cmd.Parameters.AddWithValue("intEventID", task.Event.IntEventID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertRehearsal(Rehearsal rehearsal)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        // return new id
        public int InsertRehearsalPart(RehearsalPart rehearsalPart)
        {
            conn.TypeMapper.UseNodaTime();
            string query = "INSERT INTO public.\"rehearsalParts\"(" +
                "\"intTypeID\", \"strDescription\", \"intEventID\", \"durLength\")" +
                " VALUES(@intTypeID, @strDescription, @intEventID, @durLength)" +
                " RETURNING \"intRehearsalPartID\";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            //cmd.Parameters.AddWithValue("dtmStartDateTime", rehearsalPart.DtmStartDateTime);
            //cmd.Parameters.AddWithValue("dtmEndDateTime", rehearsalPart.DtmEndDateTime);
            //cmd.Parameters.AddWithValue("intRehearsalID", rehearsalPart.IntRehearsalPartID);
            cmd.Parameters.AddWithValue("intTypeID", rehearsalPart.Type.IntTypeID);
            cmd.Parameters.AddWithValue("strDescription", rehearsalPart.StrDescription);
            cmd.Parameters.AddWithValue("durLength", rehearsalPart.DurLength);
            cmd.Parameters.AddWithValue("intEventID", rehearsalPart.Event.IntEventID);

            int result = (int)cmd.ExecuteScalar();

            return result;
        }

        public bool InsertAttendancePlanned(AttendancePlanned attendancePlanned)
        {
            string query = "INSERT INTO public.\"attendancePlanned\"(" +
                "\"intRehearsalPartID\", \"intUserID\")" +
                " VALUES(@intRehearsalPartID, @intUserID); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("intRehearsalPartID", attendancePlanned.RehearsalPart.IntRehearsalPartID);
            cmd.Parameters.AddWithValue("intUserID", attendancePlanned.User.IntUserID);

            int result = (int)cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertAttendanceActual(AttendanceActual attendanceActual)
        {
            //// define a query
            //string query = "INSERT INTO public.\"attendanceActual\"(" +
            //    " \"ysnDidShow\", \"intAttendancePlannedID\", \"dtmInTime\", \"dtmOutTime\")" +
            //    " VALUES(@ysnDidShow, @intAttendancePlannedID, @dtmInTime, @dtmOutTime); ";
            //NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            //cmd.Parameters.AddWithValue("ysnDidShow", attendanceActual.YsnDidShow);
            //cmd.Parameters.AddWithValue("intAttendancePlannedID", attendanceActual.AttendancePlanned.IntAttendancePlannedID);
            //cmd.Parameters.AddWithValue("dtmInTime", attendanceActual.DtmInTime);
            //cmd.Parameters.AddWithValue("dtmOutTime", attendanceActual.DtmOutTime);

            //int result = cmd.ExecuteNonQuery();

            //if (result == 1)
            //    return true;
            //else
            //    return false;
            ////TODO
            throw new NotImplementedException("todo");
        }

        public int InsertUser(Users user)
        {
            // define a query
            string query = "INSERT INTO \"users\"" +
                " (\"strName\", \"strEmail\", \"strPhone\", \"bytKey\", \"bytSalt\")" +
                " VALUES" +
                " (@strName, @strEmail, @strPhone, @bytKey, @bytSalt)" +
                " RETURNING \"intUserID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("strName", user.StrName);
            cmd.Parameters.AddWithValue("strEmail", user.StrEmail);
            cmd.Parameters.AddWithValue("strPhone", user.StrPhone);
            cmd.Parameters.AddWithValue("bytKey", user.BytKey);
            cmd.Parameters.AddWithValue("bytSalt", user.BytSalt);

            int result = (int)cmd.ExecuteScalar();

            return result;
            // return new user id
        }

        public bool InsertType(Types type)
        {
            // define a query
            string query = "INSERT INTO public.types(" +
                "\"strName\")" +
                " VALUES(@strName); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("strName", type.StrName);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertEventSchedule(EventSchedule eventSchedule)
        {

            conn.TypeMapper.UseNodaTime();

            // define a query
            string query = "INSERT INTO public.\"eventSchedule\"(" +
                "\"tmeMondayStart\", \"tmeTuesdayStart\", \"tmeWednesdayStart\"," +
                " \"tmeThursdayStart\", \"tmeFridayStart\", \"tmeSaturdayStart\"," +
                " \"tmeSundayStart\", \"durWeekdayDuration\", \"durWeekendDuration\"," +
                " \"intEventID\")" +
                " VALUES(@tmeMondayStart, @tmeTuesdayStart, @tmeWednesdayStart, @tmeThursdayStart, @tmeFridayStart, @tmeSaturdayStart, @tmeSundayStart, @durWeekdayDuration, @durWeekendDuration, @intEventID);";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("tmeMondayStart", eventSchedule.TmeMondayStart);
            cmd.Parameters.AddWithValue("tmeTuesdayStart", eventSchedule.TmeTuesdayStart); ;
            cmd.Parameters.AddWithValue("tmeWednesdayStart", eventSchedule.TmeWednesdayStart);
            cmd.Parameters.AddWithValue("tmeThursdayStart", eventSchedule.TmeThursdayStart);
            cmd.Parameters.AddWithValue("tmeFridayStart", eventSchedule.TmeFridayStart);
            cmd.Parameters.AddWithValue("tmeSaturdayStart", eventSchedule.TmeSaturdayStart);
            cmd.Parameters.AddWithValue("tmeSundayStart", eventSchedule.TmeSundayStart);
            cmd.Parameters.AddWithValue("durWeekdayDuration", eventSchedule.PerWeekdayDuration);
            cmd.Parameters.AddWithValue("durWeekendDuration", eventSchedule.PerWeekendDuration);
            cmd.Parameters.AddWithValue("intEventID", eventSchedule.Event.IntEventID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertPart(Part part)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool UpdateUserKey(int intUserID, byte[] newKey)
        {
            // define a query
            string query = "UPDATE \"users\" SET \"bytKey\" = @newKey" +
                " WHERE \"intUserID\" = @intUserID";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("newKey", newKey);
            cmd.Parameters.AddWithValue("intUserID", intUserID);

            int result = cmd.ExecuteNonQuery();

            if (result == 1)
                return true;
            else
                return false;
        }
    }
}