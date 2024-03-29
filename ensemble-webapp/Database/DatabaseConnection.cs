﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ensemble_webapp.Database
{
    public class DatabaseConnection
    {
        public static readonly string SERVER = "ec2-23-20-129-146.compute-1.amazonaws.com";
        public static readonly string USER = "mxerlyvbjyicty";
        public static readonly string DATABASE = "deodkjnaj6p2t2";
        public static readonly string PASS = "876dfd50897a0f97872b0acb2353c92d28e72bf5908df03cbde4db116cee034c";
        public static readonly string SSL = "Require";
        public static readonly string TRUST = "true";
        public static readonly string MAX_POOL_SIZE = "200";
        public static readonly string CONN_IDLE_LIFE = "1";
        public static readonly string CONN_PRUNE = "2";

        public static NpgsqlConnection GetConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection("" +
                "Server=" + SERVER + ";" +
                "User Id=" + USER + ";" +
                "Password=" + PASS + ";" +
                "Database=" + DATABASE + ";" +
                "Trust Server Certificate=" + TRUST + ";" +
                "Maximum Pool Size=" + MAX_POOL_SIZE + ";" +
                "Connection Idle Lifetime=" + CONN_IDLE_LIFE + ";" +
                "Connection Pruning Interval=" + CONN_IDLE_LIFE + ";" +
                "SSL Mode=" + SSL + ";");

            return conn;
        }
    }
}