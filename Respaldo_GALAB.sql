--
-- PostgreSQL database dump
--

\restrict UA4BvADkxRcIhXwkrRqKaZTYgv6UP5kpl6hZmRbCOxgVUbmDEbdvvVSZ3Rm2RB8

-- Dumped from database version 18.1
-- Dumped by pg_dump version 18.1

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
-- Name: administradores; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.administradores (
    id_administrador character varying(20) NOT NULL,
    nombre character varying(50) NOT NULL,
    primer_apellido character varying(50) NOT NULL,
    segundo_apellido character varying(50),
    correo character varying(100) NOT NULL,
    telefono character varying(10),
    usuario character varying(50) NOT NULL,
    contrasena character varying(255) NOT NULL,
    rol character varying(30) NOT NULL,
    activo boolean NOT NULL,
    fecha_registro timestamp without time zone NOT NULL,
    CONSTRAINT administradores_correo_check CHECK (((correo)::text ~~ '%@%'::text))
);


ALTER TABLE public.administradores OWNER TO postgres;

--
-- Name: alumnos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.alumnos (
    id_alumno character varying(20) NOT NULL,
    numero_control bigint NOT NULL,
    nombre character varying(50) NOT NULL,
    primer_apellido character varying(50) NOT NULL,
    segundo_apellido character varying(50),
    semestre character varying(20) NOT NULL,
    grupo character varying(10) NOT NULL,
    correo character varying(254) NOT NULL,
    numero_asiento integer,
    telefono character varying(20),
    rol character varying(30) NOT NULL,
    contrasena character varying(255) NOT NULL,
    activo boolean NOT NULL,
    fecha_registro timestamp without time zone NOT NULL,
    CONSTRAINT alumnos_correo_check CHECK (((correo)::text ~~ '%@%'::text))
);


ALTER TABLE public.alumnos OWNER TO postgres;

--
-- Name: equipamientos; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.equipamientos (
    id_serie bigint NOT NULL,
    nombre character varying(100) NOT NULL,
    tipo_equipamiento character varying(50) NOT NULL,
    descripcion text,
    marca character varying(50),
    modelo character varying(50),
    fecha_adquisicion date,
    fecha_ultimo_mantenimiento date,
    observaciones text
);


ALTER TABLE public.equipamientos OWNER TO postgres;

--
-- Name: incidencias_id_incidencia_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.incidencias_id_incidencia_seq
    START WITH 10
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.incidencias_id_incidencia_seq OWNER TO postgres;

--
-- Name: incidencias; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.incidencias (
    id_incidencia bigint DEFAULT nextval('public.incidencias_id_incidencia_seq'::regclass) NOT NULL,
    id_serie bigint NOT NULL,
    id_alumno character varying(20) NOT NULL,
    id_administrador character varying(20),
    titulo character varying(150) NOT NULL,
    descripcion text,
    estado character varying(20) NOT NULL,
    fecha_reporte date NOT NULL,
    hora_reporte time without time zone NOT NULL,
    fecha_atencion date,
    fecha_cierre date,
    solucion text,
    evidencia_foto text,
    CONSTRAINT incidencias_estado_check CHECK (((estado)::text = ANY (ARRAY['pendiente'::text, 'en_proceso'::text, 'resuelto'::text, 'Activa'::text, 'En proceso'::text, 'Resuelta'::text])))
);


ALTER TABLE public.incidencias OWNER TO postgres;

--
-- Data for Name: administradores; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.administradores (id_administrador, nombre, primer_apellido, segundo_apellido, correo, telefono, usuario, contrasena, rol, activo, fecha_registro) FROM stdin;
4	jose	vargas	quera	admiwhehen@itsmg.edu.mx	6517739893	jose vargas		Administrador	t	2026-06-06 14:02:56.417085
\.


--
-- Data for Name: alumnos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.alumnos (id_alumno, numero_control, nombre, primer_apellido, segundo_apellido, semestre, grupo, correo, numero_asiento, telefono, rol, contrasena, activo, fecha_registro) FROM stdin;
1	7738548954	rosabel	lopez	sarabi	6	B	correonxsdk@institucion.edu.mx	7	8662494930	Estudiante		t	2026-06-06 13:34:44.335936
2	87283827	flor	lopez	bautista	4	H	dummynjdnjf@test.com	6	7829430430	Estudiante		t	2026-06-06 13:39:36.75713
3	763457467	rosa	cruz	vargas	4	J	dummhdhay@test.com	9	8628390445	Estudiante		t	2026-06-06 13:39:36.75713
4	3747847	rosendo	ros	quiro	6	J	correojnaan@institucion.edu.mx	3	7577889009	Estudiante		t	2026-06-06 14:16:33.606208
\.


--
-- Data for Name: equipamientos; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.equipamientos (id_serie, nombre, tipo_equipamiento, descripcion, marca, modelo, fecha_adquisicion, fecha_ultimo_mantenimiento, observaciones) FROM stdin;
726763	muse	Hardware y software	\N	\N	\N	\N	\N	\N
7637463	mause	Hardware y software	\N	\N	\N	\N	\N	\N
6536	mause	Hardware y software	\N	\N	\N	\N	\N	\N
72634	ruter	Hardware y software	\N	\N	\N	\N	\N	\N
767754	mause	Hardware y software	\N	\N	\N	\N	\N	\N
76374785	teclado	Hardware y software	\N	\N	\N	\N	\N	\N
6274783	PC	Hardware y software	\N	\N	\N	\N	\N	\N
674337	silla	Infraestructura	\N	\N	\N	\N	\N	\N
7676	mesa	Infraestructura	\N	\N	\N	\N	\N	\N
\.


--
-- Data for Name: incidencias; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.incidencias (id_incidencia, id_serie, id_alumno, id_administrador, titulo, descripcion, estado, fecha_reporte, hora_reporte, fecha_atencion, fecha_cierre, solucion, evidencia_foto) FROM stdin;
18	7676	3	4	mesa descompuesta	la mesa ya no sube ni baja 	Resuelta	2026-06-06	14:00:00	2026-06-06	\N	la mesa se compodra lo mas pronto posible	\N
\.


--
-- Name: incidencias_id_incidencia_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.incidencias_id_incidencia_seq', 18, true);


--
-- Name: administradores administradores_correo_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administradores
    ADD CONSTRAINT administradores_correo_key UNIQUE (correo);


--
-- Name: administradores administradores_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administradores
    ADD CONSTRAINT administradores_pkey PRIMARY KEY (id_administrador);


--
-- Name: administradores administradores_usuario_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administradores
    ADD CONSTRAINT administradores_usuario_key UNIQUE (usuario);


--
-- Name: alumnos alumnos_correo_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumnos
    ADD CONSTRAINT alumnos_correo_key UNIQUE (correo);


--
-- Name: alumnos alumnos_numero_control_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumnos
    ADD CONSTRAINT alumnos_numero_control_key UNIQUE (numero_control);


--
-- Name: alumnos alumnos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.alumnos
    ADD CONSTRAINT alumnos_pkey PRIMARY KEY (id_alumno);


--
-- Name: equipamientos equipamientos_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.equipamientos
    ADD CONSTRAINT equipamientos_pkey PRIMARY KEY (id_serie);


--
-- Name: incidencias incidencias_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incidencias
    ADD CONSTRAINT incidencias_pkey PRIMARY KEY (id_incidencia);


--
-- Name: incidencias fk_incidencia_administrador; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incidencias
    ADD CONSTRAINT fk_incidencia_administrador FOREIGN KEY (id_administrador) REFERENCES public.administradores(id_administrador) ON UPDATE CASCADE;


--
-- Name: incidencias fk_incidencia_alumno; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incidencias
    ADD CONSTRAINT fk_incidencia_alumno FOREIGN KEY (id_alumno) REFERENCES public.alumnos(id_alumno);


--
-- Name: incidencias fk_incidencia_equipamiento; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.incidencias
    ADD CONSTRAINT fk_incidencia_equipamiento FOREIGN KEY (id_serie) REFERENCES public.equipamientos(id_serie);


--
-- PostgreSQL database dump complete
--

\unrestrict UA4BvADkxRcIhXwkrRqKaZTYgv6UP5kpl6hZmRbCOxgVUbmDEbdvvVSZ3Rm2RB8

