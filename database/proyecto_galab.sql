--
-- PostgreSQL database dump
--

\restrict W7ba8pdWCtUlJLJxL0bRwGZFKNylJmz3XeNRWkd9iO1AHWdOLkYq3mLIAMa3paO

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

-- Started on 2026-06-03 13:08:46

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
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
-- TOC entry 222 (class 1259 OID 33174)
-- Name: administradores; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.administradores (
    id_administrador character varying(50) NOT NULL,
    correo character varying(120),
    telefono character varying(30),
    activo boolean DEFAULT true NOT NULL,
    nombre_completo character varying(160) DEFAULT ''::character varying NOT NULL,
    curp character varying(30),
    fecha_nacimiento character varying(30),
    genero character varying(40),
    calle character varying(160),
    colonia character varying(120),
    codigo_postal character varying(20),
    municipio character varying(120),
    estado character varying(120),
    ruta_foto_perfil text
);


ALTER TABLE public.administradores OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 33130)
-- Name: alumnos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.alumnos (
    num_control character varying(50) NOT NULL,
    semestre character varying(50),
    grupo character varying(50),
    correo character varying(150),
    nombre_completo character varying(160) DEFAULT ''::character varying NOT NULL,
    curp character varying(30),
    fecha_nacimiento character varying(30),
    genero character varying(40),
    telefono character varying(30),
    estatus character varying(40),
    carrera character varying(160),
    calle character varying(160),
    colonia character varying(120),
    codigo_postal character varying(20),
    municipio character varying(120),
    estado character varying(120),
    ruta_foto_perfil text
);


ALTER TABLE public.alumnos OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 33181)
-- Name: equipamientos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.equipamientos (
    id_serie bigint NOT NULL,
    nombre character varying(50) NOT NULL,
    descripcion text,
    tipo_equipamiento character varying(50)
);


ALTER TABLE public.equipamientos OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 33191)
-- Name: incidencias; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.incidencias (
    id_incidencia bigint NOT NULL,
    descripcion text,
    estado character varying(10),
    folio character varying(30),
    titulo character varying(160),
    quien_reporta character varying(120),
    tipo_incidencia character varying(80),
    nombre_equipo character varying(120),
    fecha_hora timestamp without time zone DEFAULT now() NOT NULL,
    ruta_evidencia text,
    creado_en timestamp without time zone DEFAULT now() NOT NULL,
    actualizado_en timestamp without time zone DEFAULT now() NOT NULL,
    descripcion_solucion text
);


ALTER TABLE public.incidencias OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 33190)
-- Name: incidencias_id_incidencia_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.incidencias_id_incidencia_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.incidencias_id_incidencia_seq OWNER TO postgres;

--
-- TOC entry 5108 (class 0 OID 0)
-- Dependencies: 224
-- Name: incidencias_id_incidencia_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.incidencias_id_incidencia_seq OWNED BY public.incidencias.id_incidencia;


--
-- TOC entry 221 (class 1259 OID 33157)
-- Name: lleva; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.lleva (
    id_alumno character varying(20) NOT NULL,
    id_materia character varying(20) NOT NULL
);


ALTER TABLE public.lleva OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 33138)
-- Name: materias; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.materias (
    id_materia character varying(20) NOT NULL,
    nombre character varying(50) NOT NULL
);


ALTER TABLE public.materias OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 33349)
-- Name: perfiles_administrador; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.perfiles_administrador (
    id_perfil bigint NOT NULL,
    usuario character varying(80),
    nombre_completo character varying(160) NOT NULL,
    curp character varying(30),
    fecha_nacimiento character varying(30),
    genero character varying(40),
    telefono character varying(30),
    correo character varying(120),
    calle character varying(160),
    colonia character varying(120),
    codigo_postal character varying(20),
    municipio character varying(120),
    estado character varying(120),
    ruta_foto_perfil text
);


ALTER TABLE public.perfiles_administrador OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 33348)
-- Name: perfiles_administrador_id_perfil_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.perfiles_administrador_id_perfil_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.perfiles_administrador_id_perfil_seq OWNER TO postgres;

--
-- TOC entry 5109 (class 0 OID 0)
-- Dependencies: 232
-- Name: perfiles_administrador_id_perfil_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.perfiles_administrador_id_perfil_seq OWNED BY public.perfiles_administrador.id_perfil;


--
-- TOC entry 231 (class 1259 OID 33334)
-- Name: perfiles_usuario; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.perfiles_usuario (
    id_perfil bigint NOT NULL,
    usuario character varying(80),
    nombre_completo character varying(160) NOT NULL,
    correo character varying(120) NOT NULL,
    rol character varying(40) NOT NULL,
    carrera character varying(160),
    ruta_foto_perfil text,
    curp character varying(30),
    fecha_nacimiento character varying(30),
    genero character varying(40),
    telefono character varying(30),
    calle character varying(160),
    colonia character varying(120),
    codigo_postal character varying(20),
    municipio character varying(120),
    estado character varying(120)
);


ALTER TABLE public.perfiles_usuario OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 33333)
-- Name: perfiles_usuario_id_perfil_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.perfiles_usuario_id_perfil_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.perfiles_usuario_id_perfil_seq OWNER TO postgres;

--
-- TOC entry 5110 (class 0 OID 0)
-- Dependencies: 230
-- Name: perfiles_usuario_id_perfil_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.perfiles_usuario_id_perfil_seq OWNED BY public.perfiles_usuario.id_perfil;


--
-- TOC entry 228 (class 1259 OID 33304)
-- Name: usuarios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.usuarios (
    id_usuario bigint NOT NULL,
    nombre character varying(80) NOT NULL,
    password character varying(120) NOT NULL,
    rol character varying(30) NOT NULL,
    activo boolean DEFAULT true NOT NULL,
    creado_en timestamp without time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.usuarios OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 33303)
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.usuarios_id_usuario_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.usuarios_id_usuario_seq OWNER TO postgres;

--
-- TOC entry 5111 (class 0 OID 0)
-- Dependencies: 227
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.usuarios_id_usuario_seq OWNED BY public.usuarios.id_usuario;


--
-- TOC entry 226 (class 1259 OID 33302)
-- Name: usuarios_sistema_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.usuarios_sistema_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.usuarios_sistema_seq OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 33320)
-- Name: usuarios_sistema; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.usuarios_sistema (
    id_usuario character varying(30) DEFAULT ('USR-'::text || lpad((nextval('public.usuarios_sistema_seq'::regclass))::text, 3, '0'::text)) NOT NULL,
    nombre_completo character varying(160) NOT NULL,
    correo character varying(120) NOT NULL,
    rol character varying(40) DEFAULT 'Usuario'::character varying NOT NULL,
    estado character varying(30) DEFAULT 'Activo'::character varying NOT NULL
);


ALTER TABLE public.usuarios_sistema OWNER TO postgres;

--
-- TOC entry 4899 (class 2604 OID 33194)
-- Name: incidencias id_incidencia; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incidencias ALTER COLUMN id_incidencia SET DEFAULT nextval('public.incidencias_id_incidencia_seq'::regclass);


--
-- TOC entry 4910 (class 2604 OID 33352)
-- Name: perfiles_administrador id_perfil; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.perfiles_administrador ALTER COLUMN id_perfil SET DEFAULT nextval('public.perfiles_administrador_id_perfil_seq'::regclass);


--
-- TOC entry 4909 (class 2604 OID 33337)
-- Name: perfiles_usuario id_perfil; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.perfiles_usuario ALTER COLUMN id_perfil SET DEFAULT nextval('public.perfiles_usuario_id_perfil_seq'::regclass);


--
-- TOC entry 4903 (class 2604 OID 33307)
-- Name: usuarios id_usuario; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios ALTER COLUMN id_usuario SET DEFAULT nextval('public.usuarios_id_usuario_seq'::regclass);


--
-- TOC entry 5091 (class 0 OID 33174)
-- Dependencies: 222
-- Data for Name: administradores; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.administradores (id_administrador, correo, telefono, activo, nombre_completo, curp, fecha_nacimiento, genero, calle, colonia, codigo_postal, municipio, estado, ruta_foto_perfil) FROM stdin;
ADM-001	admin@itsmg.edu.mx	\N	t		\N	\N	\N	\N	\N	\N	\N	\N	\N
admin	admin@itsmg.edu.mx	876738982928	t	jnjnjsjnjJNSJK	MKMDKMKFRK	kk762738	ndjfem	7y73hjsndj	nsmksmkK	87887	anjanjn	jnjanjnaj	
\.


--
-- TOC entry 5088 (class 0 OID 33130)
-- Dependencies: 219
-- Data for Name: alumnos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.alumnos (num_control, semestre, grupo, correo, nombre_completo, curp, fecha_nacimiento, genero, telefono, estatus, carrera, calle, colonia, codigo_postal, municipio, estado, ruta_foto_perfil) FROM stdin;
767272627	kkkmk	njnjnka	correo@institucion.edu.mx	anguie	jsnjnakmakmks	767363	MUJER	76767688992	njnjna	knnjkmakmk	knavbhaj	yyvyaio,o	plpalaimamg	st	wrtqu	
87438	7	a	correo@institucion.edu.mx	angela	jjshdjmkmam	76747838	jnjsj	878789398	Activo	jnjjaj	jnjankmka	kmakmak	6767	m a a	jndjende	
\.


--
-- TOC entry 5092 (class 0 OID 33181)
-- Dependencies: 223
-- Data for Name: equipamientos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.equipamientos (id_serie, nombre, descripcion, tipo_equipamiento) FROM stdin;
\.


--
-- TOC entry 5094 (class 0 OID 33191)
-- Dependencies: 225
-- Data for Name: incidencias; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.incidencias (id_incidencia, descripcion, estado, folio, titulo, quien_reporta, tipo_incidencia, nombre_equipo, fecha_hora, ruta_evidencia, creado_en, actualizado_en, descripcion_solucion) FROM stdin;
6	se rompio una pata	Activa	INC-2026-0001	REGISTRAR INCIDENCIA	mesa	Infraestructura	Pendiente	2026-06-03 00:47:00	Ningún archivo seleccionado	2026-06-03 00:48:00.016551	2026-06-03 00:48:00.016551	\N
7	quebrada	Activa	INC-2026-0007	silla	hbjnajj	Infraestructura	Pendiente	2026-06-03 00:54:00	Ningún archivo seleccionado	2026-06-03 00:54:42.495062	2026-06-03 00:54:42.495062	\N
8	la pta de la silla se quebro y ya no tiene pata	Resuelta	INC-2026-0008	silla sin una pata	admin	Infraestructura	Pendiente	2026-06-03 08:40:00	C:\\Users\\admin\\OneDrive\\Imágenes\\Edit a mobile app sc.png	2026-06-03 08:41:43.599594	2026-06-03 08:50:22.019164	\N
9	pc no prende y esta rrayado 	Activa	INC-2026-0009	pc	admin	Hardware y software	Pendiente	2026-06-03 09:54:00	Ningún archivo seleccionado	2026-06-03 09:56:07.340902	2026-06-03 09:56:07.340902	
\.


--
-- TOC entry 5090 (class 0 OID 33157)
-- Dependencies: 221
-- Data for Name: lleva; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.lleva (id_alumno, id_materia) FROM stdin;
\.


--
-- TOC entry 5089 (class 0 OID 33138)
-- Dependencies: 220
-- Data for Name: materias; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.materias (id_materia, nombre) FROM stdin;
\.


--
-- TOC entry 5102 (class 0 OID 33349)
-- Dependencies: 233
-- Data for Name: perfiles_administrador; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.perfiles_administrador (id_perfil, usuario, nombre_completo, curp, fecha_nacimiento, genero, telefono, correo, calle, colonia, codigo_postal, municipio, estado, ruta_foto_perfil) FROM stdin;
\.


--
-- TOC entry 5100 (class 0 OID 33334)
-- Dependencies: 231
-- Data for Name: perfiles_usuario; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.perfiles_usuario (id_perfil, usuario, nombre_completo, correo, rol, carrera, ruta_foto_perfil, curp, fecha_nacimiento, genero, telefono, calle, colonia, codigo_postal, municipio, estado) FROM stdin;
\.


--
-- TOC entry 5097 (class 0 OID 33304)
-- Dependencies: 228
-- Data for Name: usuarios; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.usuarios (id_usuario, nombre, password, rol, activo, creado_en) FROM stdin;
2	87438	87438	Estudiante	t	2026-06-03 09:58:21.046001
\.


--
-- TOC entry 5098 (class 0 OID 33320)
-- Dependencies: 229
-- Data for Name: usuarios_sistema; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.usuarios_sistema (id_usuario, nombre_completo, correo, rol, estado) FROM stdin;
\.


--
-- TOC entry 5112 (class 0 OID 0)
-- Dependencies: 224
-- Name: incidencias_id_incidencia_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.incidencias_id_incidencia_seq', 9, true);


--
-- TOC entry 5113 (class 0 OID 0)
-- Dependencies: 232
-- Name: perfiles_administrador_id_perfil_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.perfiles_administrador_id_perfil_seq', 1, false);


--
-- TOC entry 5114 (class 0 OID 0)
-- Dependencies: 230
-- Name: perfiles_usuario_id_perfil_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.perfiles_usuario_id_perfil_seq', 1, false);


--
-- TOC entry 5115 (class 0 OID 0)
-- Dependencies: 227
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.usuarios_id_usuario_seq', 2, true);


--
-- TOC entry 5116 (class 0 OID 0)
-- Dependencies: 226
-- Name: usuarios_sistema_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.usuarios_sistema_seq', 1, false);


--
-- TOC entry 4918 (class 2606 OID 33180)
-- Name: administradores administradores_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administradores
    ADD CONSTRAINT administradores_pkey PRIMARY KEY (id_administrador);


--
-- TOC entry 4912 (class 2606 OID 33384)
-- Name: alumnos alumnos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumnos
    ADD CONSTRAINT alumnos_pkey PRIMARY KEY (num_control);


--
-- TOC entry 4920 (class 2606 OID 33189)
-- Name: equipamientos equipamientos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.equipamientos
    ADD CONSTRAINT equipamientos_pkey PRIMARY KEY (id_serie);


--
-- TOC entry 4922 (class 2606 OID 33364)
-- Name: incidencias incidencias_folio_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incidencias
    ADD CONSTRAINT incidencias_folio_key UNIQUE (folio);


--
-- TOC entry 4924 (class 2606 OID 33199)
-- Name: incidencias incidencias_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incidencias
    ADD CONSTRAINT incidencias_pkey PRIMARY KEY (id_incidencia);


--
-- TOC entry 4916 (class 2606 OID 33163)
-- Name: lleva lleva_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.lleva
    ADD CONSTRAINT lleva_pkey PRIMARY KEY (id_alumno, id_materia);


--
-- TOC entry 4914 (class 2606 OID 33144)
-- Name: materias materias_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.materias
    ADD CONSTRAINT materias_pkey PRIMARY KEY (id_materia);


--
-- TOC entry 4936 (class 2606 OID 33358)
-- Name: perfiles_administrador perfiles_administrador_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.perfiles_administrador
    ADD CONSTRAINT perfiles_administrador_pkey PRIMARY KEY (id_perfil);


--
-- TOC entry 4938 (class 2606 OID 33360)
-- Name: perfiles_administrador perfiles_administrador_usuario_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.perfiles_administrador
    ADD CONSTRAINT perfiles_administrador_usuario_key UNIQUE (usuario);


--
-- TOC entry 4932 (class 2606 OID 33345)
-- Name: perfiles_usuario perfiles_usuario_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.perfiles_usuario
    ADD CONSTRAINT perfiles_usuario_pkey PRIMARY KEY (id_perfil);


--
-- TOC entry 4934 (class 2606 OID 33347)
-- Name: perfiles_usuario perfiles_usuario_usuario_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.perfiles_usuario
    ADD CONSTRAINT perfiles_usuario_usuario_key UNIQUE (usuario);


--
-- TOC entry 4926 (class 2606 OID 33319)
-- Name: usuarios usuarios_nombre_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_nombre_key UNIQUE (nombre);


--
-- TOC entry 4928 (class 2606 OID 33317)
-- Name: usuarios usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (id_usuario);


--
-- TOC entry 4930 (class 2606 OID 33332)
-- Name: usuarios_sistema usuarios_sistema_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios_sistema
    ADD CONSTRAINT usuarios_sistema_pkey PRIMARY KEY (id_usuario);


--
-- TOC entry 4939 (class 2606 OID 33386)
-- Name: lleva lleva_id_alumno_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.lleva
    ADD CONSTRAINT lleva_id_alumno_fkey FOREIGN KEY (id_alumno) REFERENCES public.alumnos(num_control);


--
-- TOC entry 4940 (class 2606 OID 33169)
-- Name: lleva lleva_id_materia_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.lleva
    ADD CONSTRAINT lleva_id_materia_fkey FOREIGN KEY (id_materia) REFERENCES public.materias(id_materia);


-- Completed on 2026-06-03 13:08:46

--
-- PostgreSQL database dump complete
--

\unrestrict W7ba8pdWCtUlJLJxL0bRwGZFKNylJmz3XeNRWkd9iO1AHWdOLkYq3mLIAMa3paO

