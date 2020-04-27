﻿using System;
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
            conn.Close();
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
            DateTime dtmDate = Convert.ToDateTime(dr["dtmDate"]);
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
            DateTime dtmDateTime = Convert.ToDateTime(dr["dtmDateTime"]);

            int intPostedByUserID = Convert.ToInt32(dr["intPostedByUserID"]);
            string strName = dr["strName"].ToString();
            string strEmail = dr["strEmail"].ToString();
            string strPhone = dr["strPhone"].ToString();
            byte[] bytSalt = (byte[])dr["bytSalt"];
            byte[] bytKey = (byte[])dr["bytKey"];

            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string eventName = dr["eventName"].ToString();
            DateTime dtmDate = Convert.ToDateTime(dr["dtmDate"]);
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
            DateTime dtmStartDateTime = Convert.ToDateTime(dr["dtmStartDateTime"]);
            DateTime dtmEndDateTime = Convert.ToDateTime(dr["dtmEndDateTime"]);
            string strLocation = dr["strLocation"].ToString();
            string strNotes = dr["strNotes"].ToString();

            int intEventID = Convert.ToInt32(dr["intEventID"]);
            string eventName = dr["eventName"].ToString();
            DateTime dtmDate = Convert.ToDateTime(dr["dtmDate"]);
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

            //int ordinalLength = dr.GetOrdinal("durLength");
            //Period durLength = dr.GetFieldValue<Period>(ordinalLength);

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

            return new RehearsalPart(intRehearsalPartID, dtmStartDateTime.GetValueOrDefault(), dtmEndDateTime.GetValueOrDefault(), strDescription, intPriority, null, type, @event);
        }

        private static DateTime? SafeGetDateTime(NpgsqlDataReader dr, string colName)
        {
            int ordinal = dr.GetOrdinal(colName);
            if (!dr.IsDBNull(ordinal))
                return Convert.ToDateTime(dr[colName]);
            else
                return null;
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
                rp = new RehearsalPart(intRehearsalPartID, dtmStartDateTime, dtmEndDateTime, strDescription, intPriority, null, type, @event);
            }
            else
            {
                rp = new RehearsalPart(intRehearsalPartID, dtmStartDateTime, dtmEndDateTime, strDescription, intPriority, GetRehearsalByID(intRehearsalID), type, @event);
            }

            return rp;
        }

        private Conflict GetConflictFromDR(NpgsqlDataReader dr)
        {
            int intConflictID = Convert.ToInt32(dr["intConflictID"]);
            DateTime dtmStartDateTime = Convert.ToDateTime(dr["dtmStartDateTime"]);
            DateTime dtmEndDateTime = Convert.ToDateTime(dr["dtmEndDateTime"]);

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
            RehearsalPart rehearsalPart = GetRehearsalPartByID(Convert.ToInt32(dr["intRehearsalPartID"]));
            Users user = GetUserByID(Convert.ToInt32(dr["intUserID"]));

            return new AttendancePlanned(intAttendancePlannedID, rehearsalPart, user);
        }

        private AttendanceActual GetAttendanceActualFromDR(NpgsqlDataReader dr)
        {
            int intAttendanceActualID = Convert.ToInt32(dr["intAttendanceActualID"]);
            DateTime dtmInTime = Convert.ToDateTime(dr["dtmInTime"]);
            DateTime dtmOutTime = Convert.ToDateTime(dr["dtmOutTime"]);
            bool ysnDidShow = Convert.ToBoolean(dr["ysnDidShow"]);
            AttendancePlanned attendancePlanned = GetAttendancePlannedByID(Convert.ToInt32(dr["intAttendancePlannedID"]));

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
            DateTime dtmDue = Convert.ToDateTime(dr["dtmDue"]);
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
            DateTime eventDate = Convert.ToDateTime(dr["eventDate"]);
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

        public Rehearsal GetRehearsalByID(int? intRehearsalID)
        {
            Rehearsal retval = null;

            // define a query
            string query = "SELECT * FROM \"rehearsals\" WHERE \"intRehearsalID\" = " + intRehearsalID;
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
            RehearsalPart retval = null;

            // define a query
            string query = "SELECT * FROM \"rehearsalParts\" WHERE \"intRehearsalPartID\" = " + intRehearsalPartID;
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
            string query = "SELECT * FROM \"attendancePlanned\" WHERE \"intAttendancePlannedID\" = " + intAttendancePlannedID;
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
            conn.TypeMapper.UseNodaTime();

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
                " FROM \"eventSchedule\") s on es.\"intEventScheduleID\" = s.\"intEventScheduleID\", events e where e.\"intEventID\" = es.\"intEventID\"" +
                " AND e.\"intEventID\" = " + intEventID + "" +
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
            conn.TypeMapper.UseNodaTime();
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
                " AND ap.\"intRehearsalPartID\" = u.\"intRehearsalPartID\"";
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
            conn.TypeMapper.UseNodaTime();
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
            List<RehearsalPart> retval = new List<RehearsalPart>();
            string strDateOnly = DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // define a query
            string query = "SELECT t.\"strName\" as \"typeName\", rp.*, e.*, g.\"strName\" as \"groupName\"" +
                " from \"rehearsalParts\" rp, \"events\" e, \"groups\" g, \"attendancePlanned\" ap, \"users\" u, \"types\" t" +
                " WHERE rp.\"intEventID\" = e.\"intEventID\"" +
                " AND e.\"intGroupID\" = g.\"intGroupID\"" +
                " AND ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " AND ap.\"intUserID\" = u.\"intUserID\"" +
                " AND t.\"intTypeID\" = rp.\"intTypeID\"" +
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
            string query = "SELECT * FROM \"rehearsals\" WHERE \"intEventID\" = " + paramEvent.IntEventID;
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
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            //string query = "SELECT * FROM \"rehearsalParts\" WHERE \"intRehearsalID\" = " + rehearsal.IntRehearsalID;
            string query = "select r.*, e.*, g.\"strName\" as \"groupName\"" +
                " from \"rehearsalParts\" r, \"events\" e, \"groups\" g" +
                " where r.\"intEventID\" = e.\"intEventID\"" +
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
            List<Rehearsal> retval = new List<Rehearsal>();
            string strDateOnly = DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            string query = "select r.*," +
                " e.\"strName\", e.\"dtmDate\", e.\"strLocation\" as \"eventLocation\", e.\"intGroupID\"," +
                " g.\"strName\" as \"groupName\"" +
                " from \"rehearsals\" r, \"attendancePlanned\" ap, \"rehearsalParts\" rp, \"events\" e, \"groups\" g" +
                " where ap.\"intRehearsalPartID\" = rp.\"intRehearsalPartID\"" +
                " and r.\"intRehearsalID\" = rp.\"intRehearsalID\"" +
                " and e.\"intGroupID\" = g.\"intGroupID\"" +
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
            string query = "SELECT * FROM \"attendancePlanned\" WHERE \"intRehearsalPartID\" = " + rehearsalPart.IntRehearsalPartID;
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
            string query = "SELECT * FROM \"attendanceActual\" WHERE \"intAttendancePlannedID\" = " + attendancePlanned.IntAttendancePlannedID;
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