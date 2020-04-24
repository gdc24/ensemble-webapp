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
            Group group = GetGroupByID(Convert.ToInt32(dr["intGroupID"]));

            return new Event(intEventID, strName, dtmDate, strLocation, group);
        }

        private Callboard GetCallboardFromDR(NpgsqlDataReader dr)
        {
            int intCallboardID = Convert.ToInt32(dr["intCallboardID"]);
            string strSubject = dr["strSubject"].ToString();
            string strNote = dr["strNote"].ToString();
            DateTime dtmDateTime = Convert.ToDateTime(dr["dtmDateTime"]);
            Member postedByMember = GetMemberByID(Convert.ToInt32(dr["intPostedByMemberID"]));
            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Callboard(intCallboardID, strSubject, strNote, dtmDateTime, postedByMember, paramEvent);
        }

        private Member GetMemberFromDR(NpgsqlDataReader dr)
        {
            int intMemberID = Convert.ToInt32(dr["intMemberID"]);
            Users user = GetUserByID(Convert.ToInt32(dr["intUserID"]));
            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Member(intMemberID, paramEvent, user);
        }

        private Users GetUserFromDR(NpgsqlDataReader dr)
        {
            int intUserID = Convert.ToInt32(dr["intUserID"]);
            string strName = dr["strName"].ToString();
            string strEmail = dr["strEmail"].ToString();
            int intPhone = Convert.ToInt32(dr["intPhone"]);
            string strUsername = dr["strUsername"].ToString();
            byte[] bytSalt = (byte[])dr["bytSalt"];
            byte[] bytKey = (byte[])dr["bytKey"];

            return new Users(intUserID, strName, bytSalt, bytKey, strUsername, strEmail, intPhone);
        }

        private Rehearsal GetRehearsalFromDR(NpgsqlDataReader dr)
        {
            int intRehearsalID = Convert.ToInt32(dr["intRehearsalID"]);
            DateTime dtmStartDateTime = Convert.ToDateTime(dr["dtmStartDateTime"]);
            DateTime dtmEndDateTime = Convert.ToDateTime(dr["dtmEndDateTime"]);
            string strLocation = dr["strLocation"].ToString();
            string strNotes = dr["strNotes"].ToString();
            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Rehearsal(intRehearsalID, dtmStartDateTime, dtmEndDateTime, strLocation, strNotes, paramEvent);
        }

        private RehearsalPart GetRehearsalPartFromDR(NpgsqlDataReader dr)
        {
            int intRehearsalPartID = Convert.ToInt32(dr["intRehearsalPartID"]);
            DateTime dtmStartDateTime = Convert.ToDateTime(dr["dtmStartDateTime"]);
            DateTime dtmEndDateTime = Convert.ToDateTime(dr["dtmEndDateTime"]);
            Rehearsal rehearsal = GetRehearsalByID(Convert.ToInt32(dr["intRehearsalID"]));
            Types type = GetTypesByID(Convert.ToInt32(dr["intTypeID"]));

            return new RehearsalPart(intRehearsalPartID, dtmStartDateTime, dtmEndDateTime, rehearsal, type);
        }

        private Conflict GetConflictFromDR(NpgsqlDataReader dr)
        {
            int intConflictID = Convert.ToInt32(dr["intConflictID"]);
            DateTime dtmStartDateTime = Convert.ToDateTime(dr["dtmStartDateTime"]);
            DateTime dtmEndDateTime = Convert.ToDateTime(dr["dtmEndDateTime"]);
            Member member = GetMemberByID(Convert.ToInt32(dr["intMemberID"]));

            return new Conflict(intConflictID, dtmStartDateTime, dtmEndDateTime, member);
        }

        private AttendancePlanned GetAttendancePlannedFromDR(NpgsqlDataReader dr)
        {
            int intAttendancePlannedID = Convert.ToInt32(dr["intAttendancePlannedID"]);
            RehearsalPart rehearsalPart = GetRehearsalPartByID(Convert.ToInt32(dr["intRehearsalPartID"]));
            Member member = GetMemberByID(Convert.ToInt32(dr["intMemberID"]));

            return new AttendancePlanned(intAttendancePlannedID, rehearsalPart, member);
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
            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Types(intTypeID, strName, paramEvent);
        }

        private Task GetTaskFromDR(NpgsqlDataReader dr)
        {
            int intTaskID = Convert.ToInt32(dr["intTaskID"]);
            DateTime dtmDue = Convert.ToDateTime(dr["dtmDue"]);
            string strName = dr["strName"].ToString();
            string strAttachment = dr["strAttachment"].ToString();
            Member memberAssignedTo = GetMemberByID(Convert.ToInt32(dr["intAssignedToMemberID"]));
            Member memberAssignedBy = GetMemberByID(Convert.ToInt32(dr["intAssignedByMemberID"]));
            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Task(intTaskID, dtmDue, strName, strAttachment, memberAssignedTo, memberAssignedBy, paramEvent);
        }

        private EventSchedule GetEventScheduleFromDR(NpgsqlDataReader dr)
        {
            int intEventScheduleID = Convert.ToInt32(dr["intEventScheduleID"]);

            TimeSpan tmpTime;
            tmpTime = (TimeSpan)dr["tmeMondayStart"];
            LocalTime tmeMondayStart = LocalTime.FromTicksSinceMidnight(tmpTime.Ticks);
            tmpTime = (TimeSpan)dr["tmeTuesdayStart"];
            LocalTime tmeTuesdayStart = LocalTime.FromTicksSinceMidnight(tmpTime.Ticks);
            tmpTime = (TimeSpan)dr["tmeWednesdayStart"];
            LocalTime tmeWednesdayStart = LocalTime.FromTicksSinceMidnight(tmpTime.Ticks);
            tmpTime = (TimeSpan)dr["tmeThursdayStart"];
            LocalTime tmeThursdayStart = LocalTime.FromTicksSinceMidnight(tmpTime.Ticks);
            tmpTime = (TimeSpan)dr["tmeFridayStart"];
            LocalTime tmeFridayStart = LocalTime.FromTicksSinceMidnight(tmpTime.Ticks);
            tmpTime = (TimeSpan)dr["tmeSaturdayStart"];
            LocalTime tmeSaturdayStart = LocalTime.FromTicksSinceMidnight(tmpTime.Ticks);
            tmpTime = (TimeSpan)dr["tmeSundayStart"];
            LocalTime tmeSundayStart = LocalTime.FromTicksSinceMidnight(tmpTime.Ticks);

            Duration durWeekdayDuration = Duration.FromMinutes(Convert.ToInt32(dr["durWeekdayDuration"]));
            Duration durWeekendDuration = Duration.FromMinutes(Convert.ToInt32(dr["durWeekendDuration"]));

            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new EventSchedule(intEventScheduleID, tmeMondayStart, tmeTuesdayStart, tmeWednesdayStart, tmeThursdayStart, tmeFridayStart, tmeSaturdayStart, tmeSundayStart, durWeekdayDuration, durWeekendDuration, paramEvent);

        }

        private Part GetPartFromDR(NpgsqlDataReader dr)
        {
            int intPartID = Convert.ToInt32(dr["intPartID"]);
            string strRole = dr["strRole"].ToString();
            Member member = GetMemberByID(Convert.ToInt32(dr["intMemberID"]));
            Event paramEvent = GetEventByID(Convert.ToInt32(dr["intEventID"]));

            return new Part(intPartID, strRole, member, paramEvent);
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

            return retval;
        }

        public Event GetEventByID(int intEventID)
        {
            Event retval = null;

            // define a query
            string query = "SELECT * FROM \"events\" WHERE \"intEventID\" = " + intEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetEventFromDR(dr);
            }

            return retval;
        }

        public Callboard GetCallboardByID(int intCallboardID)
        {
            Callboard retval = null;

            // define a query
            string query = "SELECT * FROM \"callboard\" WHERE \"intCallboardID\" = " + intCallboardID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetCallboardFromDR(dr);
            }

            return retval;
        }

        public Member GetMemberByID(int intMemberID)
        {
            Member retval = null;

            // define a query
            string query = "SELECT * FROM \"members\" WHERE \"intMemberID\" = " + intMemberID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetMemberFromDR(dr);
            }

            return retval;
        }

        public Rehearsal GetRehearsalByID(int intRehearsalID)
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

            return retval;
        }

        public Task GetTaskByID(int intTaskID)
        {
            Task retval = null;

            // define a query
            string query = "SELECT * FROM \"tasks\" WHERE \"intTaskID\" = " + intTaskID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetTaskFromDR(dr);
            }

            return retval;
        }

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

            return retval;
        }

        #endregion

        #region GET LISTS

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

            return retval;
        }

        public List<Event> GetEventsByGroup(Group group)
        {
            List<Event> retval = new List<Event>();

            // define a query
            string query = "SELECT * FROM \"events\" WHERE \"intGroupID\" = " + group.IntGroupID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Event tmpEvent = GetEventFromDR(dr);
                retval.Add(tmpEvent);
            }

            return retval;
        }

        public List<Callboard> GetCallboardsByEvent(Event paramEvent)
        {
            List<Callboard> retval = new List<Callboard>();

            // define a query
            string query = "SELECT * FROM \"callboard\" WHERE \"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Callboard tmpCallboard = GetCallboardFromDR(dr);
                retval.Add(tmpCallboard);
            }

            return retval;
        }

        public List<Callboard> GetCallboardsByPostedByMember(Member member)
        {
            List<Callboard> retval = new List<Callboard>();

            // define a query
            string query = "SELECT * FROM \"callboard\" WHERE \"intPostedByMemberID\" = " + member.IntMemberID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Callboard tmpCallboard = GetCallboardFromDR(dr);
                retval.Add(tmpCallboard);
            }

            return retval;
        }

        public List<Member> GetAllMembers()
        {
            List<Member> retval = new List<Member>();

            // define a query
            string query = "SELECT * FROM \"members\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Member tmpMember = GetMemberFromDR(dr);
                retval.Add(tmpMember);
            }

            return retval;
        }

        public List<Users> GetUsersByEvent(Event paramEvent)
        {
            List<Users> retval = new List<Users>();

            // define a query
            string query = "SELECT u.* FROM \"users\" u, \"members\" me" +
                " WHERE me.\"intUserID\" = u.\"intUserID\"" +
                " AND me.\"intEventID\" = " + paramEvent.IntEventID;
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

        public Users GetUserByUsername(string strUsername)
        {
            Users retval = null;

            // define a query
            string query = "SELECT * FROM \"users\" WHERE \"strUsername\" = " + strUsername;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                retval = GetUserFromDR(dr);
            }

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

            return retval;
        }

        public List<RehearsalPart> GetRehearsalPartsByRehearsal(Rehearsal rehearsal)
        {
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            string query = "SELECT * FROM \"rehearsalParts\" WHERE \"intRehearsalID\" = " + rehearsal.IntRehearsalID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            return retval;
        }

        public List<RehearsalPart> GetRehearsalPartsByEvent(Event paramEvent)
        {
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            string query = "SELECT * FROM \"rehearsalParts\" rp, \"rehearsals\" r" +
                " WHERE r.\"intEventID\" = " + paramEvent.IntEventID +
                " AND r.\"intRehearsalID\" = rp.\"intRehearsalID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            return retval;
        }

        public List<RehearsalPart> GetRehearsalPartsByEventAndType(Event paramEvent, Types type)
        {
            List<RehearsalPart> retval = new List<RehearsalPart>();

            // define a query
            string query = "SELECT * FROM \"rehearsalParts\"rp, \"rehearsals\" r" +
                " WHERE r.\"intEventID\" = " + paramEvent.IntEventID +
                " AND rp.\"intTypeID\" = " + type.IntTypeID +
                " AND r.\"intRehearsalID\" = rp.\"intRehearsalID\"";
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                RehearsalPart tmpRehearsalPart = GetRehearsalPartFromDR(dr);
                retval.Add(tmpRehearsalPart);
            }

            return retval;
        }

        public List<AttendancePlanned> GetAttendancePlannedByMember(Member member)
        {
            List<AttendancePlanned> retval = new List<AttendancePlanned>();

            // define a query
            string query = "SELECT * FROM \"attendancePlanned\" WHERE \"intMemberID\" = " + member.IntMemberID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                AttendancePlanned tmpAttendancePlanned = GetAttendancePlannedFromDR(dr);
                retval.Add(tmpAttendancePlanned);
            }

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

            return retval;
        }

        // get a list of the attendance for a given member
        public List<AttendanceActual> GetAttendanceActualByMember(Member member)
        {
            List<AttendanceActual> retval = new List<AttendanceActual>();

            // define a query
            string query = "SELECT * FROM \"attendanceActual\" aa, \"attendancePlanned\" ap" +
                " WHERE ap.\"intMemberID\" = " + member.IntMemberID +
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

            return retval;
        }

        public List<Conflict> GetConflictsByMember(Member member)
        {
            List<Conflict> retval = new List<Conflict>();

            // define a query
            string query = "SELECT * FROM \"conflicts\" WHERE \"intMemberID\" = " + member.IntMemberID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Conflict conflict = GetConflictFromDR(dr);
                retval.Add(conflict);
            }

            return retval;
        }

        public List<Conflict> GetConflictsByMemberAndDay(Member member, LocalDate date)
        {
            List<Conflict> retval = new List<Conflict>();
            string strDateOnly = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // define a query
            string query = "SELECT * FROM \"conflicts\" WHERE \"intAssignedToMemberID\" = " + member.IntMemberID +
                " AND DATE(\"dtmStartDateTime\") = " + strDateOnly;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Conflict conflict = GetConflictFromDR(dr);
                retval.Add(conflict);
            }

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

            return retval;
        }

        public List<Task> GetTasksByAssignedToMember(Member member)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT * FROM \"tasks\" WHERE \"intAssignedToMemberID\" = " + member.IntMemberID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            return retval;
        }

        public List<Task> GetTasksByAssignedByMember(Member member)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT * FROM \"tasks\" WHERE \"intAssignedByMemberID\" = " + member.IntMemberID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            return retval;
        }

        public List<Task> GetTasksByEvent(Event paramEvent)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT * FROM \"tasks\" WHERE \"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            return retval;
        }

        // get a list of tasks assigned to a user after a certain time
        public List<Task> GetTasksDueAfter(Member member, DateTime dateTime)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT * FROM \"tasks\" WHERE \"intAssignedToMemberID\" = " + member.IntMemberID + " AND \"dtmDue\" < '" + dateTime + "'"; // YYYY-MM-DD HH:MM:SS.MMM
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            return retval;
        }

        public List<Task> GetTasksByEventAndMember(Member member, Event paramEvent)
        {
            List<Task> retval = new List<Task>();

            // define a query
            string query = "SELECT * FROM \"tasks\" WHERE \"intAssignedToMemberID\" = " + member.IntMemberID + " AND \"intEventID\" = " + paramEvent.IntEventID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Task tasks = GetTaskFromDR(dr);
                retval.Add(tasks);
            }

            return retval;
        }

        public List<Part> GetPartsByMember(Member member)
        {
            List<Part> retval = new List<Part>();

            // define a query
            string query = "SELECT * FROM \"parts\" WHERE \"intMemberID\" = " + member.IntMemberID;
            NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

            // execute query
            NpgsqlDataReader dr = cmd.ExecuteReader();

            // read all rows and output the first column in each row
            while (dr.Read())
            {
                Part parts = GetPartFromDR(dr);
                retval.Add(parts);
            }

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

            return retval;
        }

        #endregion

        #region MISC GETS

        #endregion
    }
}