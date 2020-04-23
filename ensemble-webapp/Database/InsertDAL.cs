﻿using ensemble_webapp.Models;
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
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertMember(Member member)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertConflict(Conflict conflict)
        {
            //TODO
            throw new NotImplementedException("todo");
        }

        public bool InsertTask(Task task)
        {
            //TODO
            throw new NotImplementedException("todo");
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
            //TODO
            throw new NotImplementedException("todo");
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
    }
}