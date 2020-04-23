--
-- PostgreSQL database dump
--

-- Dumped from database version 12.2 (Ubuntu 12.2-2.pgdg16.04+1)
-- Dumped by pg_dump version 12.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: attendanceActual; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public."attendanceActual" (
    "intAttendanceActualID" integer NOT NULL,
    "ysnDidShow" boolean,
    "intAttendancePlannedID" integer,
    "dtmInTime" timestamp without time zone,
    "dtmOutTime" timestamp without time zone
);


ALTER TABLE public."attendanceActual" OWNER TO gccnzcuoxvsjiv;

--
-- Name: attendanceActual_intAttendanceActualID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."attendanceActual_intAttendanceActualID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."attendanceActual_intAttendanceActualID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: attendanceActual_intAttendanceActualID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."attendanceActual_intAttendanceActualID_seq" OWNED BY public."attendanceActual"."intAttendanceActualID";


--
-- Name: attendancePlanned; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public."attendancePlanned" (
    "intAttendancePlannedID" integer NOT NULL,
    "intRehearsalPartID" integer,
    "intMemberID" integer
);


ALTER TABLE public."attendancePlanned" OWNER TO gccnzcuoxvsjiv;

--
-- Name: attendancePlanned_intAttendancePlannedID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."attendancePlanned_intAttendancePlannedID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."attendancePlanned_intAttendancePlannedID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: attendancePlanned_intAttendancePlannedID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."attendancePlanned_intAttendancePlannedID_seq" OWNED BY public."attendancePlanned"."intAttendancePlannedID";


--
-- Name: callboard; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.callboard (
    "intCallboardID" integer NOT NULL,
    "strSubject" character varying(255),
    "strNote" character varying(255),
    "dtmDateTime" timestamp without time zone,
    "intPostedByMemberID" integer,
    "intEventID" integer
);


ALTER TABLE public.callboard OWNER TO gccnzcuoxvsjiv;

--
-- Name: callboard_intCallboardID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."callboard_intCallboardID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."callboard_intCallboardID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: callboard_intCallboardID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."callboard_intCallboardID_seq" OWNED BY public.callboard."intCallboardID";


--
-- Name: conflicts; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.conflicts (
    "intConflictID" integer NOT NULL,
    "dtmStartDateTime" timestamp without time zone,
    "dtmEndDateTime" timestamp without time zone,
    "intMemberID" integer
);


ALTER TABLE public.conflicts OWNER TO gccnzcuoxvsjiv;

--
-- Name: conflicts_intConflictID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."conflicts_intConflictID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."conflicts_intConflictID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: conflicts_intConflictID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."conflicts_intConflictID_seq" OWNED BY public.conflicts."intConflictID";


--
-- Name: eventSchedule; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public."eventSchedule" (
    "intEventScheduleID" integer NOT NULL,
    "tmeMondayStart" time without time zone,
    "tmeTuesdayStart" time without time zone,
    "tmeWednesdayStart" time without time zone,
    "tmeThursdayStart" time without time zone,
    "tmeFridayStart" time without time zone,
    "tmeSaturdayStart" time without time zone,
    "tmeSundayStart" time without time zone,
    "durWeekdayDuration" interval,
    "durWeekendDuration" interval,
    "intEventID" integer
);


ALTER TABLE public."eventSchedule" OWNER TO gccnzcuoxvsjiv;

--
-- Name: eventSchedule_intEventScheduleID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."eventSchedule_intEventScheduleID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."eventSchedule_intEventScheduleID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: eventSchedule_intEventScheduleID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."eventSchedule_intEventScheduleID_seq" OWNED BY public."eventSchedule"."intEventScheduleID";


--
-- Name: events; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.events (
    "intEventID" integer NOT NULL,
    "strName" character varying(255),
    "dtmDate" timestamp without time zone,
    "strLocation" character varying(255),
    "intGroupID" integer
);


ALTER TABLE public.events OWNER TO gccnzcuoxvsjiv;

--
-- Name: events_intEventID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."events_intEventID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."events_intEventID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: events_intEventID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."events_intEventID_seq" OWNED BY public.events."intEventID";


--
-- Name: groups; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.groups (
    "intGroupID" integer NOT NULL,
    "strName" character varying(255)
);


ALTER TABLE public.groups OWNER TO gccnzcuoxvsjiv;

--
-- Name: groups_intGroupID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."groups_intGroupID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."groups_intGroupID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: groups_intGroupID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."groups_intGroupID_seq" OWNED BY public.groups."intGroupID";


--
-- Name: members; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.members (
    "intMemberID" integer NOT NULL,
    "strName" character varying(255),
    "strUsername" character varying(255),
    "bytKey" bytea,
    "bytSalt" bytea,
    "strEmail" character varying(255),
    "intPhone" integer,
    "intEventID" integer
);


ALTER TABLE public.members OWNER TO gccnzcuoxvsjiv;

--
-- Name: members_intMemberID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."members_intMemberID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."members_intMemberID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: members_intMemberID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."members_intMemberID_seq" OWNED BY public.members."intMemberID";


--
-- Name: parts; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.parts (
    "intPartID" integer NOT NULL,
    "strRole" character varying(255),
    "intMemberID" integer,
    "intEventID" integer
);


ALTER TABLE public.parts OWNER TO gccnzcuoxvsjiv;

--
-- Name: parts_intPartID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."parts_intPartID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."parts_intPartID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: parts_intPartID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."parts_intPartID_seq" OWNED BY public.parts."intPartID";


--
-- Name: rehearsalParts; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public."rehearsalParts" (
    "intRehearsalPartID" integer NOT NULL,
    "dtmStartDateTime" timestamp without time zone,
    "dtmEndDateTime" timestamp without time zone,
    "intRehearsalID" integer,
    "intTypeID" integer
);


ALTER TABLE public."rehearsalParts" OWNER TO gccnzcuoxvsjiv;

--
-- Name: rehearsalparts_intRehearsalPartID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."rehearsalparts_intRehearsalPartID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."rehearsalparts_intRehearsalPartID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: rehearsalparts_intRehearsalPartID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."rehearsalparts_intRehearsalPartID_seq" OWNED BY public."rehearsalParts"."intRehearsalPartID";


--
-- Name: rehearsals; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.rehearsals (
    "intRehearsalID" integer NOT NULL,
    "dtmStartDateTime" timestamp without time zone,
    "dtmEndDateTime" timestamp without time zone,
    "strLocation" character varying(255),
    "strNotes" character varying(255),
    "intEventID" integer
);


ALTER TABLE public.rehearsals OWNER TO gccnzcuoxvsjiv;

--
-- Name: rehearsals_intRehearsalID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."rehearsals_intRehearsalID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."rehearsals_intRehearsalID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: rehearsals_intRehearsalID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."rehearsals_intRehearsalID_seq" OWNED BY public.rehearsals."intRehearsalID";


--
-- Name: tasks; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.tasks (
    "intTaskID" integer NOT NULL,
    "dtmDue" timestamp without time zone,
    "strName" character varying(255),
    "strAttachment" character varying(255),
    "intAssignedToMemberID" integer,
    "intAssignedByMemberID" integer,
    "intEventID" integer
);


ALTER TABLE public.tasks OWNER TO gccnzcuoxvsjiv;

--
-- Name: tasks_intTaskID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."tasks_intTaskID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."tasks_intTaskID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: tasks_intTaskID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."tasks_intTaskID_seq" OWNED BY public.tasks."intTaskID";


--
-- Name: types; Type: TABLE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE TABLE public.types (
    "intTypeID" integer NOT NULL,
    "strName" character varying(255),
    "intEventID" integer
);


ALTER TABLE public.types OWNER TO gccnzcuoxvsjiv;

--
-- Name: types_intTypeID_seq; Type: SEQUENCE; Schema: public; Owner: gccnzcuoxvsjiv
--

CREATE SEQUENCE public."types_intTypeID_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public."types_intTypeID_seq" OWNER TO gccnzcuoxvsjiv;

--
-- Name: types_intTypeID_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER SEQUENCE public."types_intTypeID_seq" OWNED BY public.types."intTypeID";


--
-- Name: attendanceActual intAttendanceActualID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."attendanceActual" ALTER COLUMN "intAttendanceActualID" SET DEFAULT nextval('public."attendanceActual_intAttendanceActualID_seq"'::regclass);


--
-- Name: attendancePlanned intAttendancePlannedID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."attendancePlanned" ALTER COLUMN "intAttendancePlannedID" SET DEFAULT nextval('public."attendancePlanned_intAttendancePlannedID_seq"'::regclass);


--
-- Name: callboard intCallboardID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.callboard ALTER COLUMN "intCallboardID" SET DEFAULT nextval('public."callboard_intCallboardID_seq"'::regclass);


--
-- Name: conflicts intConflictID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.conflicts ALTER COLUMN "intConflictID" SET DEFAULT nextval('public."conflicts_intConflictID_seq"'::regclass);


--
-- Name: eventSchedule intEventScheduleID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."eventSchedule" ALTER COLUMN "intEventScheduleID" SET DEFAULT nextval('public."eventSchedule_intEventScheduleID_seq"'::regclass);


--
-- Name: events intEventID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.events ALTER COLUMN "intEventID" SET DEFAULT nextval('public."events_intEventID_seq"'::regclass);


--
-- Name: groups intGroupID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.groups ALTER COLUMN "intGroupID" SET DEFAULT nextval('public."groups_intGroupID_seq"'::regclass);


--
-- Name: members intMemberID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.members ALTER COLUMN "intMemberID" SET DEFAULT nextval('public."members_intMemberID_seq"'::regclass);


--
-- Name: parts intPartID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.parts ALTER COLUMN "intPartID" SET DEFAULT nextval('public."parts_intPartID_seq"'::regclass);


--
-- Name: rehearsalParts intRehearsalPartID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."rehearsalParts" ALTER COLUMN "intRehearsalPartID" SET DEFAULT nextval('public."rehearsalparts_intRehearsalPartID_seq"'::regclass);


--
-- Name: rehearsals intRehearsalID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.rehearsals ALTER COLUMN "intRehearsalID" SET DEFAULT nextval('public."rehearsals_intRehearsalID_seq"'::regclass);


--
-- Name: tasks intTaskID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.tasks ALTER COLUMN "intTaskID" SET DEFAULT nextval('public."tasks_intTaskID_seq"'::regclass);


--
-- Name: types intTypeID; Type: DEFAULT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.types ALTER COLUMN "intTypeID" SET DEFAULT nextval('public."types_intTypeID_seq"'::regclass);


--
-- Data for Name: attendanceActual; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public."attendanceActual" ("intAttendanceActualID", "ysnDidShow", "intAttendancePlannedID", "dtmInTime", "dtmOutTime") FROM stdin;
\.


--
-- Data for Name: attendancePlanned; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public."attendancePlanned" ("intAttendancePlannedID", "intRehearsalPartID", "intMemberID") FROM stdin;
\.


--
-- Data for Name: callboard; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.callboard ("intCallboardID", "strSubject", "strNote", "dtmDateTime", "intPostedByMemberID", "intEventID") FROM stdin;
\.


--
-- Data for Name: conflicts; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.conflicts ("intConflictID", "dtmStartDateTime", "dtmEndDateTime", "intMemberID") FROM stdin;
\.


--
-- Data for Name: eventSchedule; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public."eventSchedule" ("intEventScheduleID", "tmeMondayStart", "tmeTuesdayStart", "tmeWednesdayStart", "tmeThursdayStart", "tmeFridayStart", "tmeSaturdayStart", "tmeSundayStart", "durWeekdayDuration", "durWeekendDuration", "intEventID") FROM stdin;
\.


--
-- Data for Name: events; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.events ("intEventID", "strName", "dtmDate", "strLocation", "intGroupID") FROM stdin;
\.


--
-- Data for Name: groups; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.groups ("intGroupID", "strName") FROM stdin;
\.


--
-- Data for Name: members; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.members ("intMemberID", "strName", "strEmail", "intPhone", "intEventID", "bytKey", "bytSalt", "strUsername") FROM stdin;
\.


--
-- Data for Name: parts; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.parts ("intPartID", "strRole", "intMemberID", "intEventID") FROM stdin;
\.


--
-- Data for Name: rehearsalParts; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public."rehearsalParts" ("intRehearsalPartID", "dtmStartDateTime", "dtmEndDateTime", "intRehearsalID", "intTypeID") FROM stdin;
\.


--
-- Data for Name: rehearsals; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.rehearsals ("intRehearsalID", "dtmStartDateTime", "dtmEndDateTime", "strLocation", "strNotes", "intEventID") FROM stdin;
\.


--
-- Data for Name: tasks; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.tasks ("intTaskID", "dtmDue", "strName", "strAttachment", "intAssignedToMemberID", "intAssignedByMemberID", "intEventID") FROM stdin;
\.


--
-- Data for Name: types; Type: TABLE DATA; Schema: public; Owner: gccnzcuoxvsjiv
--

COPY public.types ("intTypeID", "strName", "intEventID") FROM stdin;
\.


--
-- Name: attendanceActual_intAttendanceActualID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."attendanceActual_intAttendanceActualID_seq"', 1, false);


--
-- Name: attendancePlanned_intAttendancePlannedID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."attendancePlanned_intAttendancePlannedID_seq"', 1, false);


--
-- Name: callboard_intCallboardID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."callboard_intCallboardID_seq"', 1, false);


--
-- Name: conflicts_intConflictID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."conflicts_intConflictID_seq"', 1, false);


--
-- Name: eventSchedule_intEventScheduleID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."eventSchedule_intEventScheduleID_seq"', 1, false);


--
-- Name: events_intEventID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."events_intEventID_seq"', 1, false);


--
-- Name: groups_intGroupID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."groups_intGroupID_seq"', 1, false);


--
-- Name: members_intMemberID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."members_intMemberID_seq"', 1, false);


--
-- Name: parts_intPartID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."parts_intPartID_seq"', 1, false);


--
-- Name: rehearsalparts_intRehearsalPartID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."rehearsalparts_intRehearsalPartID_seq"', 1, false);


--
-- Name: rehearsals_intRehearsalID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."rehearsals_intRehearsalID_seq"', 1, false);


--
-- Name: tasks_intTaskID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."tasks_intTaskID_seq"', 1, false);


--
-- Name: types_intTypeID_seq; Type: SEQUENCE SET; Schema: public; Owner: gccnzcuoxvsjiv
--

SELECT pg_catalog.setval('public."types_intTypeID_seq"', 1, false);


--
-- Name: attendanceActual attendanceActual_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."attendanceActual"
    ADD CONSTRAINT "attendanceActual_pkey" PRIMARY KEY ("intAttendanceActualID");


--
-- Name: attendancePlanned attendancePlanned_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."attendancePlanned"
    ADD CONSTRAINT "attendancePlanned_pkey" PRIMARY KEY ("intAttendancePlannedID");


--
-- Name: callboard callboard_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.callboard
    ADD CONSTRAINT callboard_pkey PRIMARY KEY ("intCallboardID");


--
-- Name: conflicts conflicts_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.conflicts
    ADD CONSTRAINT conflicts_pkey PRIMARY KEY ("intConflictID");


--
-- Name: eventSchedule eventSchedule_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."eventSchedule"
    ADD CONSTRAINT "eventSchedule_pkey" PRIMARY KEY ("intEventScheduleID");


--
-- Name: events events_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.events
    ADD CONSTRAINT events_pkey PRIMARY KEY ("intEventID");


--
-- Name: groups groups_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.groups
    ADD CONSTRAINT groups_pkey PRIMARY KEY ("intGroupID");


--
-- Name: members members_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.members
    ADD CONSTRAINT members_pkey PRIMARY KEY ("intMemberID");


--
-- Name: parts parts_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.parts
    ADD CONSTRAINT parts_pkey PRIMARY KEY ("intPartID");


--
-- Name: rehearsalParts rehearsalparts_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."rehearsalParts"
    ADD CONSTRAINT rehearsalparts_pkey PRIMARY KEY ("intRehearsalPartID");


--
-- Name: rehearsals rehearsals_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.rehearsals
    ADD CONSTRAINT rehearsals_pkey PRIMARY KEY ("intRehearsalID");


--
-- Name: tasks tasks_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT tasks_pkey PRIMARY KEY ("intTaskID");


--
-- Name: types types_pkey; Type: CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.types
    ADD CONSTRAINT types_pkey PRIMARY KEY ("intTypeID");


--
-- Name: attendanceActual attendanceActual_intAttendancePlannedID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."attendanceActual"
    ADD CONSTRAINT "attendanceActual_intAttendancePlannedID_fkey" FOREIGN KEY ("intAttendancePlannedID") REFERENCES public."attendancePlanned"("intAttendancePlannedID");


--
-- Name: attendancePlanned attendancePlanned_intMemberID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."attendancePlanned"
    ADD CONSTRAINT "attendancePlanned_intMemberID_fkey" FOREIGN KEY ("intMemberID") REFERENCES public.members("intMemberID");


--
-- Name: attendancePlanned attendancePlanned_intRehearsalPartID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."attendancePlanned"
    ADD CONSTRAINT "attendancePlanned_intRehearsalPartID_fkey" FOREIGN KEY ("intRehearsalPartID") REFERENCES public."rehearsalParts"("intRehearsalPartID");


--
-- Name: callboard callboard_intEventID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.callboard
    ADD CONSTRAINT "callboard_intEventID_fkey" FOREIGN KEY ("intEventID") REFERENCES public.events("intEventID");


--
-- Name: callboard callboard_intPostedByMemberID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.callboard
    ADD CONSTRAINT "callboard_intPostedByMemberID_fkey" FOREIGN KEY ("intPostedByMemberID") REFERENCES public.members("intMemberID");


--
-- Name: conflicts conflicts_intMemberID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.conflicts
    ADD CONSTRAINT "conflicts_intMemberID_fkey" FOREIGN KEY ("intMemberID") REFERENCES public.members("intMemberID");


--
-- Name: eventSchedule eventSchedule_intEventID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."eventSchedule"
    ADD CONSTRAINT "eventSchedule_intEventID_fkey" FOREIGN KEY ("intEventID") REFERENCES public.events("intEventID");


--
-- Name: events events_intGroupID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.events
    ADD CONSTRAINT "events_intGroupID_fkey" FOREIGN KEY ("intGroupID") REFERENCES public.groups("intGroupID");


--
-- Name: members members_intEventID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.members
    ADD CONSTRAINT "members_intEventID_fkey" FOREIGN KEY ("intEventID") REFERENCES public.events("intEventID");


--
-- Name: parts parts_intEventID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.parts
    ADD CONSTRAINT "parts_intEventID_fkey" FOREIGN KEY ("intEventID") REFERENCES public.events("intEventID");


--
-- Name: parts parts_intMemberID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.parts
    ADD CONSTRAINT "parts_intMemberID_fkey" FOREIGN KEY ("intMemberID") REFERENCES public.members("intMemberID");


--
-- Name: rehearsalParts rehearsalparts_intRehearsalID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."rehearsalParts"
    ADD CONSTRAINT "rehearsalparts_intRehearsalID_fkey" FOREIGN KEY ("intRehearsalID") REFERENCES public.rehearsals("intRehearsalID");


--
-- Name: rehearsalParts rehearsalparts_intTypeID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public."rehearsalParts"
    ADD CONSTRAINT "rehearsalparts_intTypeID_fkey" FOREIGN KEY ("intTypeID") REFERENCES public.types("intTypeID");


--
-- Name: rehearsals rehearsals_intEventID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.rehearsals
    ADD CONSTRAINT "rehearsals_intEventID_fkey" FOREIGN KEY ("intEventID") REFERENCES public.events("intEventID");


--
-- Name: tasks tasks_intAssignedByMemberID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT "tasks_intAssignedByMemberID_fkey" FOREIGN KEY ("intAssignedByMemberID") REFERENCES public.members("intMemberID");


--
-- Name: tasks tasks_intAssignedToMemberID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT "tasks_intAssignedToMemberID_fkey" FOREIGN KEY ("intAssignedToMemberID") REFERENCES public.members("intMemberID");


--
-- Name: tasks tasks_intEventID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.tasks
    ADD CONSTRAINT "tasks_intEventID_fkey" FOREIGN KEY ("intEventID") REFERENCES public.events("intEventID");


--
-- Name: types types_intEventID_fkey; Type: FK CONSTRAINT; Schema: public; Owner: gccnzcuoxvsjiv
--

ALTER TABLE ONLY public.types
    ADD CONSTRAINT "types_intEventID_fkey" FOREIGN KEY ("intEventID") REFERENCES public.events("intEventID");


--
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: gccnzcuoxvsjiv
--

REVOKE ALL ON SCHEMA public FROM postgres;
REVOKE ALL ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO gccnzcuoxvsjiv;
GRANT ALL ON SCHEMA public TO PUBLIC;


--
-- Name: LANGUAGE plpgsql; Type: ACL; Schema: -; Owner: postgres
--

GRANT ALL ON LANGUAGE plpgsql TO gccnzcuoxvsjiv;


--
-- PostgreSQL database dump complete
--

