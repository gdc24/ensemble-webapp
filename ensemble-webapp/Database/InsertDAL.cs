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
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertEvent(Event paramEvent)
        {
            //TODO
            throw new NotImplementedException("todo");
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

            conn.Close();

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
                " VALUES(@intEventID, @intUserID); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("intEventID", e.IntEventID);
            cmd.Parameters.AddWithValue("intUserID", editedUserProfile.IntUserID);

            int result = cmd.ExecuteNonQuery();

            conn.Close();

            if (result == 1)
                return true;
            else
                return false;
        }

        public bool InsertConflict(Conflict conflict)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertTask(Task task)
        {
            // define a query
            string query = "INSERT INTO public.tasks(" +
                "\"dtmDue\", \"strName\", \"strAttachment\", \"intAssignedToUserID\", \"intAssignedByUserID\", \"intEventID\")" +
                " VALUES(@dtmDue, @strName, @strAttachment, @intAssignedToUserID, @intAssignedByUserID, @intEventID); ";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("dtmDue", task.DtmDue);
            cmd.Parameters.AddWithValue("strName", task.StrName);
            cmd.Parameters.AddWithValue("strAttachment", task.StrAttachment);
            cmd.Parameters.AddWithValue("intAssignedToUserID", task.UserAssignedTo.IntUserID);
            cmd.Parameters.AddWithValue("intAssignedByUserID", task.UserAssignedBy.IntUserID);
            cmd.Parameters.AddWithValue("intEventID", task.Event.IntEventID);

            int result = cmd.ExecuteNonQuery();

            conn.Close();

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

        public bool InsertRehearsalPart(RehearsalPart rehearsalPart)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertAttendancePlanned(AttendancePlanned attendancePlanned)
        {
            //TODO
            throw new NotImplementedException("todo");
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

            NpgsqlConnection conn = DatabaseConnection.GetConnection();
            conn.Open();

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

            conn.Close();

            return result;
            // return new user id
        }

        public bool InsertType(Type type)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertEventSchedule(EventSchedule eventSchedule)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertPart(Part part)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertNewHash(byte[] newHash)
        {
            //TODO
            throw new NotImplementedException("todo");
        }
    }
}