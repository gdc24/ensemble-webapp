using System;
using System.Collections.Generic;
using Npgsql;
using NodaTime;
using System.Linq;
using System.Web;
using ensemble_webapp.Models;
using System.Globalization;

namespace ensemble_webapp.Database
{
    public class GetDAL
    {// global connection used to run all queries
        NpgsqlConnection conn;

        public GetDAL()
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
            conn.Dispose();
        }

        #region GET FROM DATAREADERS

        private Group GetGroupFromDR(NpgsqlDataReader dr)
        {
            int intGroupID = Convert.ToInt32(dr["intGroupID"]);
            string strName = dr["strName"].ToString();

            return new Group(intGroupID, strName);
        }

        private Event GetEventFromDR(NpgsqlDataReader dr)
        {
            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string strName = dr["strName"].ToString();
            //DateTime dtmDate = Convert.ToDateTime(dr["dtmDate"]);
            DateTime dtmDate = SafeGetDateTime(dr, "dtmDate").GetValueOrDefault();
            string strLocation = dr["strLocation"].ToString();
            int intGroupID = Convert.ToInt32(dr["intGroupID"]);
            string strGroupName = dr["groupName"].ToString();

            Group group = new Group(intGroupID, strGroupName);

            return new Event(intEventID, strName, dtmDate, strLocation, group);
        }

        private Callboard GetCallboardFromDR(NpgsqlDataReader dr)
        {
            int intCallboardID = Convert.ToInt32(dr["intCallboardID"]);
            string strSubject = dr["strSubject"].ToString();
            string strNote = dr["strNote"].ToString();
            //DateTime dtmDateTime = Convert.ToDateTime(dr["dtmDateTime"]);
            DateTime dtmDateTime = SafeGetDateTime(dr, "dtmDateTime").GetValueOrDefault();

            int intPostedByUserID = Convert.ToInt32(dr["intPostedByUserID"]);
            string strName = dr["strName"].ToString();
            string strEmail = dr["strEmail"].ToString();
            string strPhone = dr["strPhone"].ToString();
            byte[] bytSalt = (byte[])dr["bytSalt"];
            byte[] bytKey = (byte[])dr["bytKey"];

            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string eventName = dr["eventName"].ToString();
            //DateTime dtmDate = Convert.ToDateTime(dr["dtmDate"]);
            DateTime dtmDate = SafeGetDateTime(dr, "dtmDate").GetValueOrDefault();
            string strLocation = dr["strLocation"].ToString();
            int intGroupID = Convert.ToInt32(dr["intGroupID"]);
            string strGroupName = dr["groupName"].ToString();
            Group group = new Group(intGroupID, strGroupName);
            Event paramEvent = new Event(intEventID, eventName, dtmDate, strLocation, group);

            Users postedByUser = new Users(intPostedByUserID, strName, bytSalt, bytKey, strEmail, strPhone);

            return new Callboard(intCallboardID, strSubject, strNote, dtmDateTime, postedByUser, paramEvent);
        }

        private Users GetUserFromDR(NpgsqlDataReader dr)
        {
            int intUserID = Convert.ToInt32(dr["intUserID"]);
            string strName = dr["strName"].ToString();
            string strEmail = dr["strEmail"].ToString();
            string strPhone = dr["strPhone"].ToString();
            byte[] bytSalt = (byte[])dr["bytSalt"];
            byte[] bytKey = (byte[])dr["bytKey"];
            //List<Event> events = this.GetEventsByUser(intUserID);

            return new Users(intUserID, strName, bytSalt, bytKey, strEmail, strPhone);
        }

        private Rehearsal GetRehearsalFromDR(NpgsqlDataReader dr)
        {
            int intRehearsalID = Convert.ToInt32(dr["intRehearsalID"]);
            //DateTime dtmStartDateTime = Convert.ToDateTime(dr["dtmStartDateTime"]);
            //DateTime dtmEndDateTime = Convert.ToDateTime(dr["dtmEndDateTime"]);
            DateTime dtmStartDateTime = SafeGetDateTime(dr, "dtmStartDateTime").GetValueOrDefault();
            DateTime dtmEndDateTime = SafeGetDateTime(dr, "dtmEndDateTime").GetValueOrDefault();
            string strLocation = dr["strLocation"].ToString();
            string strNotes = dr["strNotes"].ToString();

            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string eventName = dr["eventName"].ToString();
            //DateTime dtmDate = Convert.ToDateTime(dr["dtmDate"]);
            DateTime dtmDate = SafeGetDateTime(dr, "dtmDate").GetValueOrDefault();
            string strEventLocation = dr["eventLocation"].ToString();
            int intGroupID = Convert.ToInt32(dr["intGroupID"]);
            string strGroupName = dr["groupName"].ToString();
            Group group = new Group(intGroupID, strGroupName);
            Event paramEvent = new Event(intEventID, eventName, dtmDate, strEventLocation, group);

            return new Rehearsal(intRehearsalID, dtmStartDateTime, dtmEndDateTime, strLocation, strNotes, paramEvent);
        }

        private RehearsalPart GetRehearsalPartFromDR(NpgsqlDataReader dr)
        {
            int intRehearsalPartID = Convert.ToInt32(dr["intRehearsalPartID"]);
            DateTime? dtmStartDateTime = SafeGetDateTime(dr, "dtmStartDateTime");
            DateTime? dtmEndDateTime = SafeGetDateTime(dr, "dtmEndDateTime");
            string strDescription = dr["strDescription"].ToString();
            int intPriority = Convert.ToInt32(dr["intPriority"]);
            //Rehearsal rehearsal = GetRehearsalByID(Convert.ToInt32(dr["intRehearsalID"]));
            //Types type = GetTypesByID(Convert.ToInt32(dr["intTypeID"]));

            int ordinalLength = dr.GetOrdinal("durLength");
            Period durLength = dr.GetFieldValue<Period>(ordinalLength);

            Types type = new Types(
                Convert.ToInt32(dr["intTypeID"]),
                dr["typeName"].ToString()
                );

            Group group = new Group(
                Convert.ToInt32(dr["intGroupID"]), 
                dr["groupName"].ToString());
            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string strEventName = dr["strName"].ToString();

            //int ordinalDtmDate = dr.GetOrdinal("dtmDate");
            //DateTime dtmDate = dr.GetFieldValue<DateTime>(ordinalDtmDate);
            string strLocation = dr["strLocation"].ToString();

            Event @event = new Event(intEventID, strEventName, strLocation, group);

            return new RehearsalPart(intRehearsalPartID, dtmStartDateTime.GetValueOrDefault(), dtmEndDateTime.GetValueOrDefault(), strDescription, intPriority, durLength, type, @event);
        }

        private static DateTime? SafeGetDateTime(NpgsqlDataReader dr, string colName)
        {

            int ordinal = dr.GetOrdinal(colName);
            if (!dr.IsDBNull(ordinal))
            {
                LocalDateTime localDate = dr.GetFieldValue<LocalDateTime>(ordinal);
                //var timezone = DateTimeZoneProviders.Bcl.GetSystemDefault();
                DateTime eventDate = localDate.ToDateTimeUnspecified();
                return eventDate;
            }
            else
            {
                return null;
            }

        }
          
        private static int? SafeGetInt(NpgsqlDataReader dr, string colName)
        {
            int ordinal = dr.GetOrdinal(colName);
            if (!dr.IsDBNull(ordinal))
                return Convert.ToInt32(dr[colName]);
            else
                return null;
        }

        public RehearsalPart GetRehearsalPartsFromDR(NpgsqlDataReader dr)
        {
            int intRehearsalPartID = Convert.ToInt32(dr["intRehearsalPartID"]);
            DateTime? rehearsalStart = SafeGetDateTime(dr, "rehearsalStart");
            DateTime? rehearsalEnd = SafeGetDateTime(dr, "rehearsalEnd");
            string rehearsalLocation = dr["rehearsalLocation"].ToString();
            string rehearsalNotes = dr["rehearsalNotes"].ToString();
            DateTime? dtmStartDateTime = SafeGetDateTime(dr, "dtmStartDateTime");
            DateTime? dtmEndDateTime = SafeGetDateTime(dr, "dtmEndDateTime");
            int? intRehearsalID = SafeGetInt(dr, "intRehearsalID");
            int intTypeID = Convert.ToInt32(dr["intTypeID"]);
            string strDescription = dr["strDescription"].ToString();
            int intEventID = Convert.ToInt32(dr["intEventID"]);
            int intPriority = Convert.ToInt32(dr["intPriority"]);
            string strEventName = dr["strName"].ToString();
            //DateTime dtmEventDate = Convert.ToDateTime(dr["dtmDate"]);
            string strLocation = dr["strLocation"].ToString();
            int intGroupID = Convert.ToInt32(dr["intGroupID"]);
            string strGroupName = dr["groupName"].ToString();
            string strTypeName = dr["typeName"].ToString();

            Group group = new Group(intGroupID, strGroupName);
            Event @event = new Event(intEventID, strEventName, strLocation, group);
            Types type = new Types(intTypeID, strTypeName);

            RehearsalPart rp;

            if (intRehearsalID == null)
            {
                rp = new RehearsalPart(intRehearsalPartID, dtmStartDateTime.GetValueOrDefault(), dtmEndDateTime.GetValueOrDefault(), strDescription, intPriority, null, type, @event);
            }
            else
            {
                Rehearsal tmpRehearsal = new Rehearsal(intRehearsalID.GetValueOrDefault(), rehearsalStart.GetValueOrDefault(), rehearsalEnd.GetValueOrDefault(), rehearsalLocation, rehearsalNotes, @event);
                rp = new RehearsalPart(intRehearsalPartID, dtmStartDateTime.GetValueOrDefault(), dtmEndDateTime.GetValueOrDefault(), strDescription, intPriority, tmpRehearsal, type, @event);
            }

            return rp;
        }

        private Conflict GetConflictFromDR(NpgsqlDataReader dr)
        {
            int intConflictID = Convert.ToInt32(dr["intConflictID"]);
            DateTime dtmStartDateTime = SafeGetDateTime(dr, "dtmStartDateTime").GetValueOrDefault();
            DateTime dtmEndDateTime = SafeGetDateTime(dr, "dtmEndDateTime").GetValueOrDefault();
            //DateTime dtmStartDateTime = Convert.ToDateTime(dr["dtmStartDateTime"]);
            //DateTime dtmEndDateTime = Convert.ToDateTime(dr["dtmEndDateTime"]);

            int intUserID = Convert.ToInt32(dr["intUserID"]);
            string strName = dr["strName"].ToString();
            string strEmail = dr["strEmail"].ToString();
            string strPhone = dr["strPhone"].ToString();
            byte[] bytSalt = (byte[])dr["bytSalt"];
            byte[] bytKey = (byte[])dr["bytKey"];


            Users user = new Users(intUserID, strName, bytSalt, bytKey, strEmail, strPhone);

            return new Conflict(intConflictID, dtmStartDateTime, dtmEndDateTime, user);
        }

        private AttendancePlanned GetAttendancePlannedFromDR(NpgsqlDataReader dr)
        {
            int intAttendancePlannedID = Convert.ToInt32(dr["intAttendancePlannedID"]);

            /*GIULIANA THIS IS THE LINE I COMMENTED OUT*/
            //RehearsalPart rehearsalPart = GetRehearsalPartByID(Convert.ToInt32(dr["intRehearsalPartID"]));
            int intRehearsalPartID = Convert.ToInt32(dr["intRehearsalPartID"]);
            DateTime? dtmStartDateTime = SafeGetDateTime(dr, "dtmStartDateTime");
            DateTime? dtmEndDateTime = SafeGetDateTime(dr, "dtmEndDateTime");
            string strDescription = dr["strDescription"].ToString();
            int intPriority = Convert.ToInt32(dr["intPriority"]);
            //Rehearsal rehearsal = GetRehearsalByID(Convert.ToInt32(dr["intRehearsalID"]));
            //Types type = GetTypesByID(Convert.ToInt32(dr["intTypeID"]));

            int ordinalLength = dr.GetOrdinal("durLength");
            Period durLength = dr.GetFieldValue<Period>(ordinalLength);

            Types type = new Types(
                Convert.ToInt32(dr["intTypeID"]),
                dr["typeName"].ToString()
                );

            Group group = new Group(
                Convert.ToInt32(dr["intGroupID"]),
                dr["groupName"].ToString());
            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string strEventName = dr["eventName"].ToString();

            //int ordinalDtmDate = dr.GetOrdinal("dtmDate");
            //DateTime dtmDate = dr.GetFieldValue<DateTime>(ordinalDtmDate);
            string strLocation = dr["eventLocation"].ToString();

            Event @event = new Event(intEventID, strEventName, strLocation, group);

            RehearsalPart rehearsalPart = new RehearsalPart(intRehearsalPartID, dtmStartDateTime.GetValueOrDefault(), dtmEndDateTime.GetValueOrDefault(), strDescription, intPriority, durLength, type, @event);

            //Users user = GetUserByID(Convert.ToInt32(dr["intUserID"]));
            int intUserID = Convert.ToInt32(dr["intUserID"]);
            string strName = dr["strName"].ToString();
            string strEmail = dr["strEmail"].ToString();
            string strPhone = dr["strPhone"].ToString();
            byte[] bytSalt = (byte[])dr["bytSalt"];
            byte[] bytKey = (byte[])dr["bytKey"];
            //List<Event> events = this.GetEventsByUser(intUserID);

            Users user = new Users(intUserID, strName, bytSalt, bytKey, strEmail, strPhone);

            return new AttendancePlanned(intAttendancePlannedID, rehearsalPart, user);
        }

        private AttendanceActual GetAttendanceActualFromDR(NpgsqlDataReader dr)
        {
            int intAttendanceActualID = Convert.ToInt32(dr["intAttendanceActualID"]);
            //DateTime dtmInTime = Convert.ToDateTime(dr["dtmInTime"]);
            DateTime dtmInTime = SafeGetDateTime(dr, "dtmInTime").GetValueOrDefault();
            //DateTime dtmOutTime = Convert.ToDateTime(dr["dtmOutTime"]);
            DateTime dtmOutTime = SafeGetDateTime(dr, "dtmOutTime").GetValueOrDefault();
            bool ysnDidShow = Convert.ToBoolean(dr["ysnDidShow"]);

            int intRehearsalPartID = Convert.ToInt32(dr["intRehearsalPartID"]);
            DateTime? dtmStartDateTime = SafeGetDateTime(dr, "dtmStartDateTime");
            DateTime? dtmEndDateTime = SafeGetDateTime(dr, "dtmEndDateTime");
            string strDescription = dr["strDescription"].ToString();
            int intPriority = Convert.ToInt32(dr["intPriority"]);

            int ordinalLength = dr.GetOrdinal("durLength");
            Period durLength = dr.GetFieldValue<Period>(ordinalLength);

            Types type = new Types(
                Convert.ToInt32(dr["intTypeID"]),
                dr["typeName"].ToString()
                );

            Group group = new Group(
                Convert.ToInt32(dr["intGroupID"]),
                dr["groupName"].ToString());
            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string strEventName = dr["eventName"].ToString();
            string strLocation = dr["eventLocation"].ToString();

            Event @event = new Event(intEventID, strEventName, strLocation, group);

            RehearsalPart rp = new RehearsalPart(intRehearsalPartID, dtmStartDateTime.GetValueOrDefault(), dtmEndDateTime.GetValueOrDefault(), strDescription, intPriority, durLength, type, @event);

            int intUserID = Convert.ToInt32(dr["intUserID"]);
            string strName = dr["strName"].ToString();
            string strEmail = dr["strEmail"].ToString();
            string strPhone = dr["strPhone"].ToString();
            byte[] bytSalt = (byte[])dr["bytSalt"];
            byte[] bytKey = (byte[])dr["bytKey"];
            Users user = new Users(intUserID, strName, bytSalt, bytKey, strEmail, strPhone);

            AttendancePlanned attendancePlanned = new AttendancePlanned(Convert.ToInt32(dr["intAttendancePlannedID"]), rp, user);

            //AttendancePlanned attendancePlanned = GetAttendancePlannedByID(Convert.ToInt32(dr["intAttendancePlannedID"]));

            return new AttendanceActual(intAttendanceActualID, dtmInTime, dtmOutTime, ysnDidShow, attendancePlanned);
        }

        private Types GetTypesFromDR(NpgsqlDataReader dr)
        {
            int intTypeID = Convert.ToInt32(dr["intTypeID"]);
            string strName = dr["strName"].ToString();
            //Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Types(intTypeID, strName);
        }

        private Task GetTaskFromDR(NpgsqlDataReader dr)
        {
            int intTaskID = Convert.ToInt32(dr["intTaskID"]);
            //DateTime dtmDue = Convert.ToDateTime(dr["dtmDue"]);
            DateTime dtmDue = SafeGetDateTime(dr, "dtmDue").GetValueOrDefault();
            string strName = dr["strName"].ToString();
            string strAttachment = dr["strAttachment"].ToString();
            bool ysnIsFinished = Convert.ToBoolean(dr["ysnIsFinished"]);
            int intAssignedToUserID = Convert.ToInt32(dr["intAssignedToUserID"]);
            string assignedToUserName = dr["assignedToUserName"].ToString();

            int intAssignedByUserID = Convert.ToInt32(dr["intAssignedByUserID"]);
            string strAssignedByName = dr["strAssignedByName"].ToString();

            string groupName = dr["groupName"].ToString();
            int intGroupID = Convert.ToInt32(dr["intGroupID"]);
            Group group = new Group(intGroupID, groupName);

            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string eventName = dr["eventName"].ToString();
            //DateTime eventDate = Convert.ToDateTime(dr["eventDate"]);
            DateTime eventDate = SafeGetDateTime(dr, "eventDate").GetValueOrDefault();
            string eventLocation = dr["eventLocation"].ToString();

            Users userAssignedTo = new Users(intAssignedToUserID, assignedToUserName);
            Users userAssignedBy = new Users(intAssignedByUserID, strAssignedByName);
            Event paramEvent = new Event(intEventID, eventName, eventDate, eventLocation, group);

            return new Task(intTaskID, dtmDue, strName, strAttachment, userAssignedTo, userAssignedBy, paramEvent, ysnIsFinished);
        }

        private EventSchedule GetEventScheduleFromDR(NpgsqlDataReader dr)
        {
            string groupName = dr["groupName"].ToString();
            int intGroupID = Convert.ToInt32(dr["intGroupID"]);
            Group group = new Group(intGroupID, groupName);

            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string eventName = dr["strName"].ToString();
            int ordinalDtmDate = dr.GetOrdinal("dtmDate");
            Instant instantDate = dr.GetFieldValue<Instant>(ordinalDtmDate);
            var timezone = DateTimeZoneProviders.Bcl.GetSystemDefault();
            DateTime eventDate = instantDate.InZone(timezone).ToDateTimeUnspecified();
            string eventLocation = dr["strLocation"].ToString();

            Event paramEvent = new Event(intEventID, eventName, eventDate, eventLocation, group);

            int intEventScheduleID = Convert.ToInt32(dr["intEventScheduleID"]);

            int ordinalMon = dr.GetOrdinal("tmeMondayStart");
            LocalTime tmeMondayStart = dr.GetFieldValue<LocalTime>(ordinalMon);
            int ordinalTue = dr.GetOrdinal("tmeTuesdayStart");
            LocalTime tmeTuesdayStart = dr.GetFieldValue<LocalTime>(ordinalTue);
            int ordinalWed = dr.GetOrdinal("tmeWednesdayStart");
            LocalTime tmeWednesdayStart = dr.GetFieldValue<LocalTime>(ordinalWed);
            int ordinalThu = dr.GetOrdinal("tmeThursdayStart");
            LocalTime tmeThursdayStart = dr.GetFieldValue<LocalTime>(ordinalThu);
            int ordinalFri = dr.GetOrdinal("tmeFridayStart");
            LocalTime tmeFridayStart = dr.GetFieldValue<LocalTime>(ordinalFri);
            int ordinalSat = dr.GetOrdinal("tmeSaturdayStart");
            LocalTime tmeSaturdayStart = dr.GetFieldValue<LocalTime>(ordinalSat);
            int ordinalSun = dr.GetOrdinal("tmeSundayStart");
            LocalTime tmeSundayStart = dr.GetFieldValue<LocalTime>(ordinalSun);

            int ordinalWeekday = dr.GetOrdinal("durWeekdayDuration");
            int ordinalWeekend = dr.GetOrdinal("durWeekendDuration");

            Period perWeekdayDuration = dr.GetFieldValue<Period>(ordinalWeekday);
            Period perWeekendDuration = dr.GetFieldValue<Period>(ordinalWeekend);


            return new EventSchedule(intEventScheduleID, tmeMondayStart, tmeTuesdayStart, tmeWednesdayStart, tmeThursdayStart, tmeFridayStart, tmeSaturdayStart, tmeSundayStart, perWeekdayDuration, perWeekendDuration, paramEvent);

        }

        private Part GetPartFromDR(NpgsqlDataReader dr)
        {
            int intPartID = Convert.ToInt32(dr["intPartID"]);
            string strRole = dr["strRole"].ToString();
            Users user = GetUserByID(Convert.ToInt32(dr["intUserID"]));
            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Part(intPartID, strRole, user, paramEvent);
        }

        #endregion

        #region GET SINGLE ITEMS BY ID

        public Group GetGroupByID(int intGroupID)
        {
            Group retval = null;

            // define a query
            string query = "SELECT * FROM \"groups\" WHERE \"intGroupID\" = " + intGroupID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetGroupFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public Event GetEventByID(int intEventID)
        {

            Event retval = null;

            // define a query
            string query = "select e.*, g.\"strName\" as \"groupName\" from \"events\" e, \"groups\" g where g.\"intGroupID\" = e.\"intGroupID\" AND \"intEventID\" = " + intEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetEventFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        //public Callboard GetCallboardByID(int intCallboardID)
        //{
        //    Callboard retval = null;

        //    // define a query
        //    string query = "SELECT * FROM \"callboard\" WHERE \"intCallboardID\" = " + intCallboardID;
        //    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

        //    // execute query
        //    NpgsqlDataReader dr = cmd.ExecuteReader();

        //    // read all rows and output the first column in each row
        //    while (dr.Read())
        //    {
        //        retval = GetCallboardFromDR(dr);
        //    }

        //    dr.Close();

        //    return retval;
        //}

        //public Member GetMemberByID(int intMemberID)
        //{
        //    Member retval = null;

        //    // define a query
        //    string query = "SELECT * FROM \"members\" WHERE \"intMemberID\" = " + intMemberID;
        //    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

        //    // execute query
        //    NpgsqlDataReader dr = cmd.ExecuteReader();

        //    // read all rows and output the first column in each row
        //    while (dr.Read())
        //    {
        //        retval = GetMemberFromDR(dr);
        //    }

        //    return retval;
        //}

        public Rehearsal GetRehearsalByID(int intRehearsalID)
        {
            Rehearsal retval = null;

            // define a query
            //string query = "SELECT * FROM \"rehearsals\" WHERE \"intRehearsalID\" = " + intRehearsalID;
            string query = "select distinct r.*," +
                " e.\"strName\" as \"eventName\", e.\"dtmDate\", e.\"strLocation\" as \"eventLocation\", e.\"intGroupID\"," +
                " g.\"strName\" as \"groupName\", t.\"strName\" as \"typeName\"" +
                " from \"types\" t, \"rehearsals\" r, \"attendancePlanned\" ap, \"rehearsalParts\" rp, \"events\" e, \"groups\" g" +
                " where ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " and r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " and e.\"intGroupID\" = g.\"intGroupID\"" +
                " and t.\"intTypeID\" = rp.\"intTypeID\"" +
                " and r.\"intEventID\" = e.\"intEventID\"" +
                " and r.\"intRehearsalID\" = " + intRehearsalID + ";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetRehearsalFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public RehearsalPart GetRehearsalPartByID(int intRehearsalPartID)
        {

            //conn.TypeMapper.UseNodaTime();

            RehearsalPart retval = null;

            // define a query
            string query = "SELECT rp.*, t.\"intTypeID\", t.\"strName\" as \"typeName\", g.\"strName\" as \"groupName\", g.\"intGroupID\"," +
                " e.\"intEventID\", e.\"strName\" as \"strName\",  e.\"strLocation\" as \"strLocation\"" +
                " FROM \"rehearsalParts\" rp, \"types\" t, \"groups\" g, \"events\" e" +
                " WHERE t.\"intTypeID\" = rp.\"intTypeID\"" +
                " AND rp.\"intEventID\" = e.\"intEventID\"" + 
                " AND g.\"intGroupID\" = e.\"intGroupID\"" +
                " AND rp.\"intRehearsalPartID\" = " + intRehearsalPartID;
                

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetRehearsalPartFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public Conflict GetConflictByID(int intConflictID)
        {
            Conflict retval = null;

            // define a query
            string query = "SELECT * FROM \"conflicts\" WHERE \"intConflictID\" = " + intConflictID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetConflictFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public AttendancePlanned GetAttendancePlannedByID(int intAttendancePlannedID)
        {
            AttendancePlanned retval = null;

            // define a query
            string query = "select ap.*, rp.*, t.\"strName\" as \"typeName\", g.\"strName\" as \"groupName\", u.*," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " e.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"" +
                " from \"attendancePlanned\" ap, \"rehearsalParts\" rp, \"types\" t, \"groups\" g, \"events\" e, \"users\" u, \"rehearsals\" r where r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " and r.\"intEventID\" = e.\"intEventID\"" +
                " and ap.\"intUserID\" = u.\"intUserID\"" +
                " and ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " and rp.\"intTypeID\" = t.\"intTypeID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and e.\"intEventID\" = rp.\"intEventID\"" +
                " and ap.\"intAttendancePlannedID\" = " + intAttendancePlannedID + ";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetAttendancePlannedFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public AttendanceActual GetAttendanceActualByID(int intAttendanceActualID)
        {
            AttendanceActual retval = null;

            // define a query
            string query = "SELECT * FROM \"attendanceActual\" WHERE \"intAttendanceActualID\" = " + intAttendanceActualID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetAttendanceActualFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public Types GetTypesByID(int intTypeID)
        {
            Types retval = null;

            // define a query
            string query = "SELECT * FROM \"types\" WHERE \"intTypeID\" = " + intTypeID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetTypesFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        //public Task GetTaskByID(int intTaskID)
        //{
        //    Task retval = null;

        //    // define a query
        //    string query = "SELECT * FROM \"tasks\" WHERE \"intTaskID\" = " + intTaskID;
        //    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

        //    // execute query
        //    NpgsqlDataReader dr = cmd.ExecuteReader();

        //    // read all rows and output the first column in each row
        //    while (dr.Read())
        //    {
        //        retval = GetTaskFromDR(dr);
        //    }

        //    dr.Close();

        //    return retval;
        //}

        public EventSchedule GetEventScheduleByID(int intEventScheduleID)
        {
            EventSchedule retval = null;

            // define a query
            string query = "SELECT * FROM \"eventSchedule\" WHERE \"intEventScheduleID\" = " + intEventScheduleID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetEventScheduleFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        // gets only the most recent event schedule
        public EventSchedule GetEventScheduleByEvent(int intEventID)
        {
            //conn.TypeMapper.UseNodaTime();

            EventSchedule retval = null;

            // define a query
            //string query = "SELECT es.* FROM \"eventSchedule\" es" +
            //    " INNER JOIN (" +
            //    "   SELECT MAX(\"intEventScheduleID\") AS \"intEventScheduleID\" from \"eventSchedule\") s" +
            //    " ON es.\"intEventScheduleID\" = s.\"intEventScheduleID\"" +
            //    " AND es.\"intEventID\" = " + intEventID + ";";
            string query = "SELECT es.*, e.*, g.\"intGroupID\", g.\"strName\" as \"groupName\"" +
                " FROM \"groups\" g, \"eventSchedule\" es" +
                " INNER JOIN(SELECT MAX(\"intEventScheduleID\") as \"intEventScheduleID\"" +
                " FROM \"eventSchedule\" WHERE \"intEventID\" = " + intEventID +
                " ) s on es.\"intEventScheduleID\" = s.\"intEventScheduleID\", events e " +
                " WHERE e.\"intEventID\" = " + intEventID + "" +
                " AND g.\"intGroupID\" = e.\"intGroupID\";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetEventScheduleFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public Part GetPartByID(int intPartID)
        {
            Part retval = null;

            // define a query
            string query = "SELECT * FROM \"parts\" WHERE \"intPartID\" = " + intPartID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetPartFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        public Users GetUserByID(int intUserID)
        {
            Users retval = null;

            // define a query
            string query = "SELECT * FROM \"users\" WHERE \"intUserID\" = " + intUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetUserFromDR(dr);
            }

            dr.Close();

            return retval;
        }

        #endregion

        #region GET LISTS

        public DateTime GetFirstTimeByDayAndUser(DateTime date, Users m)
        {
            //conn.TypeMapper.UseNodaTime();
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            string query = "select rp.*, e.*, g.\"strName\" as \"groupName\", t.\"strName\" as \"typeName\", t.\"intTypeID\"" +
                " from types t, \"rehearsalParts\" rp, \"events\" e, \"groups\" g, \"users\" u, \"attendancePlanned\" ap" +
                " WHERE rp.\"intEventID\" = e.\"intEventID\"" +
                " AND g.\"intGroupID\" = e.\"intGroupID\"" +
                " AND rp.\"intTypeID\" = t.\"intTypeID\"" +
                " AND u.\"intUserID\" = " + m.IntUserID +
                " AND DATE(rp.\"dtmStartDateTime\") = '" + date.Date.ToString("yyyy-MM-dd") + "'" +
                " AND ap.\"intUserID\" = u.\"intUserID\"" +
                " AND ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            dr.Close();

            DateTime earliestTimeInList = retval.Min(x => x.DtmStartDateTime.Value);

            return retval.Where(x => x.DtmStartDateTime.Equals(earliestTimeInList)).First().DtmStartDateTime.Value;

            //return retval.OrderBy(d => d.DtmStartDateTime ?? DateTime.MaxValue).First().DtmStartDateTime.Value;
        }


        public List<RehearsalPart> GetAllRehearsalParts()
        {
            //conn.TypeMapper.UseNodaTime();
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            string query = "select rp.*, e.*, g.\"strName\" as \"groupName\", t.\"strName\" as \"typeName\", t.\"intTypeID\"" +
                " from types t, \"rehearsalParts\" rp, events e, groups g" +
                " WHERE rp.\"intEventID\" = e.\"intEventID\"" +
                " AND g.\"intGroupID\" = e.\"intGroupID\"" +
                " AND rp.\"intTypeID\" = t.\"intTypeID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            dr.Close();

            return retval;
        }

        public List<Group> GetAllGroups()
        {
            List<Group> retval = new List<Group>();

            // define a query
            string query = "SELECT * FROM \"groups\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Group tmpGroup = GetGroupFromDR(dr);
                retval.Add(tmpGroup);
            }

            dr.Close();

            return retval;
        }


        public List<Event> GetEventsByUser(int intUserID)
        {
            //conn.TypeMapper.UseNodaTime();
            List<Event> retval = new List<Event>();

            // define a query
            string query = "SELECT e.*, u.\"intUserID\", g.\"strName\" as \"groupName\"" +
                " FROM events e, \"userEvents\" ue, \"users\" u, \"groups\" g" +
                " WHERE u.\"intUserID\" = ue.\"intUserID\"" +
                " AND e.\"intEventID\" = ue.\"intEventID\"" +
                " AND g.\"intGroupID\" = e.\"intGroupID\"" +
                " AND u.\"intUserID\" = " + intUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Event tmpEvent = GetEventFromDR(dr);
                //int intEventID = Convert.ToInt32(dr["intEventID"]);
                //Event tmpEvent = GetEventByID(intEventID);
                retval.Add(tmpEvent);
            }

            dr.Close();

            return retval;

        }

        public List<Event> GetAdminEventsByUser(int intUserID)
        {
            //conn.TypeMapper.UseNodaTime();
            List<Event> retval = new List<Event>();

            // define a query
            string query = "SELECT e.*, u.\"intUserID\", g.\"strName\" as \"groupName\"" +
                " FROM events e, \"userEvents\" ue, \"users\" u, \"groups\" g" +
                " WHERE u.\"intUserID\" = ue.\"intUserID\"" +
                " AND e.\"intEventID\" = ue.\"intEventID\"" +
                " AND g.\"intGroupID\" = e.\"intGroupID\"" +
                " AND ue.\"ysnIsAdmin\" = true" +
                " AND u.\"intUserID\" = " + intUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Event tmpEvent = GetEventFromDR(dr);
                //int intEventID = Convert.ToInt32(dr["intEventID"]);
                //Event tmpEvent = GetEventByID(intEventID);
                retval.Add(tmpEvent);
            }

            dr.Close();

            return retval;

        }

        public List<Event> GetEventsByGroup(Group group)
        {
            List<Event> retval = new List<Event>();

            // define a query
            string query = "select e.*, g.\"strName\" as \"groupName\" from \"events\" e, \"groups\" g where g.\"intGroupID\" = e.\"intGroupID\" AND \"intGroupID\" = " + group.IntGroupID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Event tmpEvent = GetEventFromDR(dr);
                retval.Add(tmpEvent);
            }

            dr.Close();

            return retval;
        }

        public List<Callboard> GetCallboardsByEvent(Event paramEvent)
        {
            List<Callboard> retval = new List<Callboard>();

            // define a query
            //string query = "SELECT * FROM \"callboard\" WHERE \"intEventID\" = " + paramEvent.IntEventID;
            string query = "SELECT c.*, u.*, e.\"strName\" as \"eventName\", e.\"dtmDate\", e.\"strLocation\", g.\"strName\" as \"groupName\", g.\"intGroupID\"" +
                " from \"callboard\" c, \"users\" u, \"events\" e, \"groups\" g" +
                " where c.\"intEventID\" = e.\"intEventID\"" +
                " and c.\"intPostedByUserID\" = u.\"intUserID\"" +
                " and e.\"intGroupID\" = g.\"intGroupID\"" +
                " and e.\"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Callboard tmpCallboard = GetCallboardFromDR(dr);
                retval.Add(tmpCallboard);
            }

            dr.Close();

            return retval;
        }

        public List<Callboard> GetCallboardsByPostedByUser(Users user)
        {
            List<Callboard> retval = new List<Callboard>();

            // define a query
            //string query = "SELECT * FROM \"callboard\" WHERE \"intPostedByUserID\" = " + user.IntUserID;
            string query = "SELECT c.*, u.*, e.\"strName\" as \"eventName\", e.\"dtmDate\", e.\"strLocation\", g.\"strName\" as \"groupName\", g.\"intGroupID\"" +
                " from \"callboard\" c, \"users\" u, \"events\" e, \"groups\" g" +
                " where c.\"intEventID\" = e.\"intEventID\"" +
                " and c.\"intPostedByUserID\" = u.\"intUserID\"" +
                " and e.\"intGroupID\" = g.\"intGroupID\"" +
                " and u.\"intUserID\" = " + user.IntUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Callboard tmpCallboard = GetCallboardFromDR(dr);
                retval.Add(tmpCallboard);
            }

            dr.Close();

            return retval;
        }

        public List<Users> GetAllUsers()
        {
            List<Users> retval = new List<Users>();

            // define a query
            string query = "SELECT * FROM \"users\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Users tmpUser = GetUserFromDR(dr);
                retval.Add(tmpUser);
            }

            dr.Close();

            return retval;
        }

        public List<Users> GetAllUsersForAdminEvents(List<Event> LstAdminEvents)
        {
            List<Users> retval = new List<Users>();
            List<int> lstEventIDs = new List<int>();

            foreach(var e in LstAdminEvents)
            {
                lstEventIDs.Add(e.IntEventID);
            }

            string strList = "(";

            strList += string.Join(",", lstEventIDs);

            strList += ")";

            // define a query
            string query = "SELECT DISTINCT u.* FROM \"users\" u, \"userEvents\" ue" +
                " WHERE ue.\"intEventID\" in " + strList + "" +
                " AND ue.\"intUserID\" = u.\"intUserID\";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Users tmpUser = GetUserFromDR(dr);
                retval.Add(tmpUser);
            }

            dr.Close();

            return retval;
        }


        public List<Users> GetUsersByRehearsalPart(RehearsalPart rp)
        {
            List<Users> retval = new List<Users>();

            // define a query
            string query = "SELECT u.* FROM \"users\" u, \"attendancePlanned\" ap" +
                " WHERE u.\"intUserID\" = ap.\"intUserID\"" +
                " AND ap.\"intRehearsalPartID\" = " + rp.IntRehearsalPartID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Users tmpUser = GetUserFromDR(dr);
                retval.Add(tmpUser);
            }

            dr.Close();

            return retval;
        }

        public List<Event> GetAllEvents()
        {
            //conn.TypeMapper.UseNodaTime();
            List<Event> retval = new List<Event>();

            // define a query
            string query = "select e.*, g.\"strName\" as \"groupName\" from \"events\" e, \"groups\" g where g.\"intGroupID\" = e.\"intGroupID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Event tmpEvent = GetEventFromDR(dr);
                retval.Add(tmpEvent);
            }

            dr.Close();

            return retval;
        }

        public List<Users> GetUsersByEvent(Event paramEvent)
        {
            List<Users> retval = new List<Users>();

            // define a query
            string query = "SELECT u.* FROM \"users\" u, \"userEvents\" ue" +
                " WHERE ue.\"intUserID\" = u.\"intUserID\"" +
                " AND ue.\"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Users tmpUsers = GetUserFromDR(dr);
                retval.Add(tmpUsers);
            }

            return retval;
        }

        public Users GetUserByName(string strName)
        {
            Users retval = null;

            // define a query
            string query = "SELECT * FROM \"users\" WHERE \"strName\" = '" + strName + "'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetUserFromDR(dr);
            }

            dr.Close();

            return retval;
        }


        public List<RehearsalPart> GetUpcomingRehearsalPartsByUser(Users user)
        {
            //conn.TypeMapper.UseNodaTime();

            List<RehearsalPart> retval = new List<RehearsalPart>();
            string strDateOnly = DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // define a query
            string query = "SELECT r.\"intRehearsalID\", r.\"dtmStartDateTime\" as \"rehearsalStart\", r.\"dtmEndDateTime\" as \"rehearsalEnd\", r.\"strLocation\" as \"rehearsalLocation\", r.\"strNotes\" as \"rehearsalNotes\"," +
                " t.\"strName\" as \"typeName\", rp.*, e.*, g.\"strName\" as \"groupName\"" +
                " from \"rehearsals\" r, \"rehearsalParts\" rp, \"events\" e, \"groups\" g, \"attendancePlanned\" ap, \"users\" u, \"types\" t" +
                " WHERE rp.\"intEventID\" = e.\"intEventID\"" +
                " AND e.\"intGroupID\" = g.\"intGroupID\"" +
                " AND ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " AND ap.\"intUserID\" = u.\"intUserID\"" +
                " AND t.\"intTypeID\" = rp.\"intTypeID\"" +
                " AND r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " AND u.\"intUserID\" = " + user.IntUserID +
                " AND DATE(rp.\"dtmStartDateTime\") > '" + strDateOnly + "'";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            Rehearsal tmpRehearsal = new Rehearsal();
            while (dr.Read())
            {
                RehearsalPart rp = GetRehearsalPartsFromDR(dr);
                retval.Add(rp);
            }

            dr.Close();

            return retval;
        }

        public List<Rehearsal> GetRehearsalsByEvent(Event paramEvent)
        {
            List<Rehearsal> retval = new List<Rehearsal>();

            // define a query
            //string query = "SELECT r.*, e.\"strName\" as \"eventName\", e."" +
            //    "FROM \"rehearsals\" r, \"events\" e" + 
            //    " WHERE r.\"intEventID\" = " + paramEvent.IntEventID;
            string query = "select distinct r.*," +
                " e.\"strName\" as \"eventName\", e.\"dtmDate\", e.\"strLocation\" as \"eventLocation\", e.\"intGroupID\"," +
                " g.\"strName\" as \"groupName\", t.\"strName\" as \"typeName\"" +
                " from \"types\" t, \"rehearsals\" r, \"attendancePlanned\" ap, \"rehearsalParts\" rp, \"events\" e, \"groups\" g" +
                " where ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " and r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " and e.\"intGroupID\" = g.\"intGroupID\"" +
                " and t.\"intTypeID\" = rp.\"intTypeID\"" +
                " and r.\"intEventID\" = e.\"intEventID\"" +
                " and r.\"intEventID\" = " + paramEvent.IntEventID + ";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Rehearsal tmpRehearsal = GetRehearsalFromDR(dr);
                retval.Add(tmpRehearsal); 
            }

            dr.Close();

            return retval;
        }

        //gets all rehearsal parts for an event that are happening today
        public List<RehearsalPart> GetRehearsalPartsByDayAndEvent(Event @event)
        {
            List<RehearsalPart> retval = new List<RehearsalPart>();

            string strDateOnly = DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // define a query
            //string query = "SELECT * FROM \"rehearsalParts\" WHERE \"intRehearsalID\" = " + rehearsal.IntRehearsalID;
            string query = "select r.*, e.*, g.\"strName\" as \"groupName\"" +
                " from \"rehearsalParts\" r, \"events\" e, \"groups\" g" +
                " where r.\"intEventID\" = e.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and DATE(rp.\"dtmStartDateTime\") = '" + strDateOnly  + "'" +
                " and r.\"intEventID\" = " + @event.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            dr.Close();

            return retval;
        }

        public List<RehearsalPart> GetRehearsalPartsByRehearsal(Rehearsal rehearsal)
        {
            //conn.TypeMapper.UseNodaTime();
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            //string query = "SELECT * FROM \"rehearsalParts\" WHERE \"intRehearsalID\" = " + rehearsal.IntRehearsalID;
            string query = "select r.*, e.*, g.\"strName\" as \"groupName\", t.\"strName\" as \"typeName\"" +
                " from \"rehearsalParts\" r, \"events\" e, \"groups\" g, \"types\" t" +
                " where r.\"intEventID\" = e.\"intEventID\"" +
                " and t.\"intTypeID\" = r.\"intTypeID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and \"intRehearsalID\" = " + rehearsal.IntRehearsalID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            dr.Close();

            return retval;
        }

        public List<RehearsalPart> GetRehearsalPartsByEvent(Event paramEvent)
        {
            //conn.TypeMapper.UseNodaTime();

            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            string query = "select rp.*, e.*, g.\"strName\" as \"groupName\", t.\"strName\" as \"typeName\", t.\"intTypeID\"" +
                " from types t, \"rehearsalParts\" rp, events e, groups g" +
                " WHERE rp.\"intEventID\" = e.\"intEventID\"" +
                " AND g.\"intGroupID\" = e.\"intGroupID\"" +
                " AND rp.\"intEventID\" = " + paramEvent.IntEventID + 
                " AND rp.\"intTypeID\" = t.\"intTypeID\";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            dr.Close();

            return retval;
        }

        public List<RehearsalPart> GetRehearsalPartsByEventAndType(Event paramEvent, Types type)
        {
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            //string query = "SELECT * FROM \"rehearsalParts\"rp, \"rehearsals\" r" +
            //    " WHERE r.\"intEventID\" = " + paramEvent.IntEventID +
            //    " AND rp.\"intTypeID\" = " + type.IntTypeID +
            //    " AND r.\"intRehearsalID\" = rp.\"intRehearsalID\"";

            string query = "select rp.*, e.*, g.\"strName\" as \"groupName\"" +
                " from \"rehearsalParts\" rp, \"events\" e, \"groups\" g, \"rehearsals\" r" +
                " WHERE r.\"intEventID\" = " + paramEvent.IntEventID +
                " AND rp.\"intTypeID\" = " + type.IntTypeID +
                " AND r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " AND r.\"intEventID\" = e.\"intEventID\"" +
                " AND g.\"intGroupID\" = e.\"intGroupID\"";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            dr.Close();

            return retval;
        }

        public List<Rehearsal> GetUpcomingRehearsalsByUser(Users user)
        {
            //conn.TypeMapper.UseNodaTime();
            List<Rehearsal> retval = new List<Rehearsal>();
            string strDateOnly = DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            string query = "select distinct r.*," +
                " e.\"strName\" as \"eventName\", e.\"dtmDate\", e.\"strLocation\" as \"eventLocation\", e.\"intGroupID\"," +
                " g.\"strName\" as \"groupName\", t.\"strName\" as \"typeName\"" +
                " from \"types\" t, \"rehearsals\" r, \"attendancePlanned\" ap, \"rehearsalParts\" rp, \"events\" e, \"groups\" g" +
                " where ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " and r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " and e.\"intGroupID\" = g.\"intGroupID\"" +
                " and t.\"intTypeID\" = rp.\"intTypeID\"" +
                " and r.\"intEventID\" = e.\"intEventID\"" +
                " and ap.\"intUserID\" = " + user.IntUserID +
                " and DATE(r.\"dtmStartDateTime\") > '" + strDateOnly +"';";

            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Rehearsal tmpRehearsal = GetRehearsalFromDR(dr);
                retval.Add(tmpRehearsal);
            }

            dr.Close();

            return retval;

        }

        public List<AttendancePlanned> GetAttendancePlannedByUser(Users user)
        {
            List<AttendancePlanned> retval = new List<AttendancePlanned>();

            // define a query
            string query = "SELECT * FROM \"attendancePlanned\" WHERE \"intUserID\" = " + user.IntUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                AttendancePlanned tmpAttendancePlanned = GetAttendancePlannedFromDR(dr);
                retval.Add(tmpAttendancePlanned);
            }

            dr.Close();

            return retval;
        }

        public List<AttendancePlanned> GetAttendancePlannedByRehearsalPart(RehearsalPart rehearsalPart)
        {
            List<AttendancePlanned> retval = new List<AttendancePlanned>();

            // define a query
            string query = "select ap.*, rp.*, t.\"strName\" as \"typeName\", g.\"strName\" as \"groupName\", u.*," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " e.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"" +
                " from \"attendancePlanned\" ap, \"rehearsalParts\" rp, \"types\" t, \"groups\" g, \"events\" e, \"users\" u, \"rehearsals\" r where r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " and r.\"intEventID\" = e.\"intEventID\"" +
                " and ap.\"intUserID\" = u.\"intUserID\"" +
                " and ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " and rp.\"intTypeID\" = t.\"intTypeID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and e.\"intEventID\" = rp.\"intEventID\"" +
                " and rp.\"intRehearsalPartID\" = " + rehearsalPart.IntRehearsalPartID + ";";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                AttendancePlanned tmpAttendancePlanned = GetAttendancePlannedFromDR(dr);
                retval.Add(tmpAttendancePlanned);
            }

            dr.Close();

            return retval;
        }

        public List<Types> GetAllTypes()
        {
            List<Types> retval = new List<Types>();

            // define a query
            string query = "SELECT * FROM \"types\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Types tmpTypes = GetTypesFromDR(dr);
                retval.Add(tmpTypes);
            }

            dr.Close();

            return retval;
        }


        // get a list of the attendance for a planned rehearsal part
        public List<AttendanceActual> GetAttendanceActualByPlanned(AttendancePlanned attendancePlanned)
        {
            List<AttendanceActual> retval = new List<AttendanceActual>();

            // define a query
            //string query = "SELECT * FROM \"attendanceActual\" WHERE \"intAttendancePlannedID\" = " + attendancePlanned.IntAttendancePlannedID;
            string query = "select aa.*, ap.*, rp.*, t.\"strName\" as \"typeName\", g.\"strName\" as \"groupName\", u.*," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " e.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"" +
                " from \"attendancePlanned\" ap, \"rehearsalParts\" rp, \"types\" t, \"groups\" g, \"events\" e, \"users\" u, \"rehearsals\" r, \"attendanceActual\" aa" +
                " where r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " and r.\"intEventID\" = e.\"intEventID\"" +
                " and aa.\"intAttendancePlannedID\" = ap.\"intAttendancePlannedID\"" +
                " and ap.\"intUserID\" = u.\"intUserID\"" +
                " and ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " and rp.\"intTypeID\" = t.\"intTypeID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and e.\"intEventID\" = rp.\"intEventID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                AttendanceActual tmpAttendanceActual = GetAttendanceActualFromDR(dr);
                retval.Add(tmpAttendanceActual);
            }

            dr.Close();

            return retval;
        }

        // get a list of the attendance for a given member
        public List<AttendanceActual> GetAttendanceActualByUser(Users user)
        {
            List<AttendanceActual> retval = new List<AttendanceActual>();

            // define a query
            string query = "SELECT * FROM \"attendanceActual\" aa, \"attendancePlanned\" ap" +
                " WHERE ap.\"intUserID\" = " + user.IntUserID +
                " AND ap.\"intAttendancePlannedID\" = aa.\"intAttendancePlannedID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                AttendanceActual tmpAttendanceActual = GetAttendanceActualFromDR(dr);
                retval.Add(tmpAttendanceActual);
            }

            dr.Close();

            return retval;
        }

        // get a list of attendance for a rehearsal part (kinda same as GetAttendanceActualByPlanned but further abstracted up)
        public List<AttendanceActual> GetAttendanceActualByRehearsalPart(RehearsalPart rehearsalPart)
        {
            List<AttendanceActual> retval = new List<AttendanceActual>();

            // define a query
            string query = "SELECT * FROM \"attendanceActual\" aa, \"attendancePlanned\" ap" +
                " WHERE ap.\"intRehearsalPartID\" = " + rehearsalPart.IntRehearsalPartID +
                " AND ap.\"intAttendancePlannedID\" = aa.\"intAttendancePlannedID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                AttendanceActual tmpAttendanceActual = GetAttendanceActualFromDR(dr);
                retval.Add(tmpAttendanceActual);
            }

            dr.Close();

            return retval;
        }

        // get a list of attendance for a whole rehearsal
        public List<AttendanceActual> GetAttendanceActualByRehearsal(Rehearsal rehearsal)
        {
            List<AttendanceActual> retval = new List<AttendanceActual>();

            // define a query
            string query = "SELECT * FROM \"attendanceActual\" aa, \"attendancePlanned\" ap, \"rehearsalParts\" rp" +
                " WHERE rp.\"intRehearsalID\" = " + rehearsal.IntRehearsalID +
                " AND rp.\"intRehearsalPartID\" = ap.\"intRehearsalPartID\"" +
                " AND ap.\"intAttendancePlannedID\" = aa.\"intAttendancePlannedID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                AttendanceActual tmpAttendanceActual = GetAttendanceActualFromDR(dr);
                retval.Add(tmpAttendanceActual);
            }

            dr.Close();

            return retval;
        }

        public List<Conflict> GetConflictsByUser(Users user)
        {
            //conn.TypeMapper.UseNodaTime();
            List<Conflict> retval = new List<Conflict>();

            // define a query
            string query = "SELECT u.*, c.*" +
                " FROM \"conflicts\" c, \"users\" u" +
                " WHERE u.\"intUserID\" = c.\"intUserID\"" +
                " AND u.\"intUserID\" = " + user.IntUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Conflict conflict = GetConflictFromDR(dr);
                retval.Add(conflict);
            }

            dr.Close();

            return retval;
        }

        public List<Conflict> GetConflictsByUserAndDay(Users user, LocalDate date)
        {
            //conn.TypeMapper.UseNodaTime();
            List<Conflict> retval = new List<Conflict>();
            string strDateOnly = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // define a query
            string query = "SELECT c.*, u.* FROM \"conflicts\" c, \"users\" u" +
                " WHERE u.\"intUserID\" = " + user.IntUserID +
                " AND u.\"intUserID\" = c.\"intUserID\"" +
                " AND DATE(\"dtmStartDateTime\") = '" + strDateOnly + "';";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Conflict conflict = GetConflictFromDR(dr);
                retval.Add(conflict);
            }

            dr.Close();

            return retval;
        }

        public List<Conflict> GetConflictsByDay(LocalDate date)
        {
            List<Conflict> retval = new List<Conflict>();
            string strDateOnly = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // define a query
            string query = "SELECT * FROM \"conflicts\" WHERE DATE(\"dtmStartDateTime\") = " + strDateOnly;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Conflict conflict = GetConflictFromDR(dr);
                retval.Add(conflict);
            }

            dr.Close();

            return retval;
        }

        public List<Task> GetTasksByAssignedToUser(Users user)
        {
            List<Task> retval = new List<Task>();

            // define a query
            //string query = "SELECT * FROM \"tasks\" WHERE \"intAssignedToUserID\" = " + user.IntUserID;
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"intAssignedToUserID\" = " + user.IntUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }

        public List<Task> GetTasksByAssignedByUser(Users user)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"intAssignedByUserID\" = " + user.IntUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }

        public List<Task> GetTasksByEvent(Event paramEvent)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }

        // get a list of tasks assigned to a user after a certain time
        public List<Task> GetUnfinishedTasksDueAfter(Users user, DateTime dateTime)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"ysnIsFinished\" = false" +
                " and s.\"intAssignedToUserID\" = " + user.IntUserID +
                " and s.\"dtmDue\" >'" + dateTime + "'"; // YYYY-MM-DD HH:MM:SS.MMM
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }

        // get a list of tasks assigned to a user due before a certain time
        public List<Task> GetUnfinishedTasksDueBefore(Users user, DateTime dateTime)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"ysnIsFinished\" = false" +
                " and s.\"intAssignedToUserID\" = " + user.IntUserID +
                " and s.\"dtmDue\" <'" + dateTime + "'"; // YYYY-MM-DD HH:MM:SS.MMM
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }


        public List<Task> GetFinishedTasks(Users user)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"ysnIsFinished\" = true" +
                " and s.\"intAssignedToUserID\" = " + user.IntUserID; // YYYY-MM-DD HH:MM:SS.MMM
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }

        // get a list of tasks assigned to a user before a certain time
        public List<Task> GetTasksDueBefore(Users user, DateTime dateTime)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"intAssignedToUserID\" = " + user.IntUserID +
                " and s.\"dtmDue\" <'" + dateTime + "'"; // YYYY-MM-DD HH:MM:SS.MMM
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }

        public List<Task> GetTasksByEventAndUser(Users user, Event paramEvent)
        {
            List<Task> retval = new List<Task>();

            // define a query
            //string query = "SELECT * FROM \"tasks\" WHERE \"intAssignedToUserID\" = " + user.IntUserID + " AND \"intEventID\" = " + paramEvent.IntEventID;
            string query = "SELECT g.\"strName\" as \"groupName\"," +
                " e.\"dtmDate\" as \"eventDate\"," +
                " e.\"strLocation\" as \"eventLocation\"," +
                " e.\"intGroupID\"," +
                " s.\"intEventID\"," +
                " e.\"strName\" as \"eventName\"," +
                " s.\"intAssignedToUserID\"," +
                " s.\"assignedToUserName\"," +
                " s.\"intTaskID\"," +
                " s.\"ysnIsFinished\"," +
                " s.\"dtmDue\"," +
                " s.\"strName\"," +
                " s.\"strAttachment\"," +
                " u.\"strName\" as \"strAssignedByName\"," +
                " u.\"intUserID\" as \"intAssignedByUserID\" from (" +
                " select t.*, u.\"strName\" as \"assignedToUserName\"" +
                " from \"tasks\" t, \"users\" u" +
                " where u.\"intUserID\" = t.\"intAssignedToUserID\"" +
                " ) s, \"users\" u, \"events\" e, \"groups\" g" +
                " WHERE u.\"intUserID\" = s.\"intAssignedByUserID\"" +
                " and e.\"intEventID\" = s.\"intEventID\"" +
                " and g.\"intGroupID\" = e.\"intGroupID\"" +
                " and s.\"intAssignedToUserID\" = " + user.IntUserID +
                " and s.\"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            dr.Close();

            return retval;
        }

        public List<Part> GetPartsByUser(Users user)
        {
            List<Part> retval = new List<Part>();

            // define a query
            string query = "SELECT * FROM \"parts\" WHERE \"intUserID\" = " + user.IntUserID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Part parts = GetPartFromDR(dr);
                retval.Add(parts);
            }

            dr.Close();

            return retval;
        }

        public List<Part> GetPartsByEvent(Event paramEvent)
        {
            List<Part> retval = new List<Part>();

            // define a query
            string query = "SELECT * FROM \"parts\" WHERE \"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Part parts = GetPartFromDR(dr);
                retval.Add(parts);
            }

            dr.Close();

            return retval;
        }

        #endregion

        #region MISC GETS

        #endregion
    }
}