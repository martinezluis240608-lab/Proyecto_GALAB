-- Si la base no existe, creala primero desde pgAdmin:
-- CREATE DATABASE proyecto_galab;
--
-- Ejecutar el resto del script conectado a la base: proyecto_galab.

CREATE SEQUENCE IF NOT EXISTS usuarios_sistema_seq START 1;

CREATE TABLE IF NOT EXISTS administradores (
    id_administrador VARCHAR(30) PRIMARY KEY,
    nombre_completo VARCHAR(160) NOT NULL DEFAULT '',
    curp VARCHAR(30),
    fecha_nacimiento VARCHAR(30),
    genero VARCHAR(40),
    telefono VARCHAR(30),
    correo VARCHAR(120),
    calle VARCHAR(160),
    colonia VARCHAR(120),
    codigo_postal VARCHAR(20),
    municipio VARCHAR(120),
    estado VARCHAR(120),
    ruta_foto_perfil TEXT,
    activo BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE IF NOT EXISTS alumnos (
    num_control VARCHAR(30) PRIMARY KEY,
    nombre_completo VARCHAR(160) NOT NULL DEFAULT '',
    curp VARCHAR(30),
    fecha_nacimiento VARCHAR(30),
    genero VARCHAR(40),
    telefono VARCHAR(30),
    correo VARCHAR(120),
    estatus VARCHAR(40),
    semestre VARCHAR(20),
    carrera VARCHAR(160),
    grupo VARCHAR(20),
    calle VARCHAR(160),
    colonia VARCHAR(120),
    codigo_postal VARCHAR(20),
    municipio VARCHAR(120),
    estado VARCHAR(120),
    ruta_foto_perfil TEXT
);

CREATE TABLE IF NOT EXISTS materias (
    id_materia VARCHAR(30) PRIMARY KEY,
    nombre VARCHAR(120) NOT NULL
);

CREATE TABLE IF NOT EXISTS lleva (
    id_alumno VARCHAR(30) NOT NULL REFERENCES alumnos(num_control) ON DELETE CASCADE,
    id_materia VARCHAR(30) NOT NULL REFERENCES materias(id_materia) ON DELETE CASCADE,
    PRIMARY KEY (id_alumno, id_materia)
);

CREATE TABLE IF NOT EXISTS equipamientos (
    id_serie BIGINT PRIMARY KEY,
    nombre VARCHAR(120) NOT NULL,
    descripcion TEXT,
    tipo_equipamiento VARCHAR(80)
);

CREATE TABLE IF NOT EXISTS incidencias (
    id_incidencia BIGSERIAL PRIMARY KEY,
    folio VARCHAR(30) UNIQUE,
    titulo VARCHAR(160),
    quien_reporta VARCHAR(120),
    tipo_incidencia VARCHAR(80),
    nombre_equipo VARCHAR(120),
    fecha_hora TIMESTAMP NOT NULL DEFAULT NOW(),
    descripcion TEXT,
    ruta_evidencia TEXT,
    estado VARCHAR(30) NOT NULL DEFAULT 'Activa',
    creado_en TIMESTAMP NOT NULL DEFAULT NOW(),
    actualizado_en TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario BIGSERIAL PRIMARY KEY,
    nombre VARCHAR(80) NOT NULL UNIQUE,
    password VARCHAR(120) NOT NULL,
    rol VARCHAR(30) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE,
    creado_en TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE IF NOT EXISTS usuarios_sistema (
    id_usuario VARCHAR(30) PRIMARY KEY DEFAULT ('USR-' || LPAD(nextval('usuarios_sistema_seq')::TEXT, 3, '0')),
    nombre_completo VARCHAR(160) NOT NULL,
    correo VARCHAR(120) NOT NULL,
    rol VARCHAR(40) NOT NULL DEFAULT 'Usuario',
    estado VARCHAR(30) NOT NULL DEFAULT 'Activo'
);

CREATE TABLE IF NOT EXISTS perfiles_usuario (
    id_perfil BIGSERIAL PRIMARY KEY,
    usuario VARCHAR(80) UNIQUE,
    nombre_completo VARCHAR(160) NOT NULL,
    correo VARCHAR(120) NOT NULL,
    rol VARCHAR(40) NOT NULL,
    carrera VARCHAR(160),
    ruta_foto_perfil TEXT,
    curp VARCHAR(30),
    fecha_nacimiento VARCHAR(30),
    genero VARCHAR(40),
    telefono VARCHAR(30),
    calle VARCHAR(160),
    colonia VARCHAR(120),
    codigo_postal VARCHAR(20),
    municipio VARCHAR(120),
    estado VARCHAR(120)
);

CREATE TABLE IF NOT EXISTS perfiles_administrador (
    id_perfil BIGSERIAL PRIMARY KEY,
    usuario VARCHAR(80) UNIQUE,
    nombre_completo VARCHAR(160) NOT NULL,
    curp VARCHAR(30),
    fecha_nacimiento VARCHAR(30),
    genero VARCHAR(40),
    telefono VARCHAR(30),
    correo VARCHAR(120),
    calle VARCHAR(160),
    colonia VARCHAR(120),
    codigo_postal VARCHAR(20),
    municipio VARCHAR(120),
    estado VARCHAR(120),
    ruta_foto_perfil TEXT
);

ALTER TABLE administradores ADD COLUMN IF NOT EXISTS correo VARCHAR(120);
ALTER TABLE administradores ADD COLUMN IF NOT EXISTS telefono VARCHAR(30);
ALTER TABLE administradores ADD COLUMN IF NOT EXISTS activo BOOLEAN NOT NULL DEFAULT TRUE;

ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS folio VARCHAR(30) UNIQUE;
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS titulo VARCHAR(160);
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS quien_reporta VARCHAR(120);
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS tipo_incidencia VARCHAR(80);
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS nombre_equipo VARCHAR(120);
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS fecha_hora TIMESTAMP NOT NULL DEFAULT NOW();
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS ruta_evidencia TEXT;
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS creado_en TIMESTAMP NOT NULL DEFAULT NOW();
ALTER TABLE incidencias ADD COLUMN IF NOT EXISTS actualizado_en TIMESTAMP NOT NULL DEFAULT NOW();

INSERT INTO usuarios (nombre, password, rol)
VALUES ('admin', 'admin', 'Administrador')
ON CONFLICT (nombre) DO NOTHING;

INSERT INTO administradores (id_administrador, nombre, correo)
VALUES ('ADM-001', 'Administrador del sistema', 'admin@itsmg.edu.mx')
ON CONFLICT (id_administrador) DO NOTHING;

ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS curp VARCHAR(30);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS fecha_nacimiento VARCHAR(30);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS genero VARCHAR(40);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS telefono VARCHAR(30);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS calle VARCHAR(160);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS colonia VARCHAR(120);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS codigo_postal VARCHAR(20);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS municipio VARCHAR(120);
ALTER TABLE perfiles_usuario ADD COLUMN IF NOT EXISTS estado VARCHAR(120);

ALTER TABLE alumnos DROP COLUMN IF EXISTS nombre;
ALTER TABLE alumnos DROP COLUMN IF EXISTS primer_apellido;
ALTER TABLE alumnos DROP COLUMN IF EXISTS segundo_apellido;
ALTER TABLE alumnos DROP COLUMN IF EXISTS numero_asiento;

ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS nombre_completo VARCHAR(160) NOT NULL DEFAULT '';
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS curp VARCHAR(30);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS fecha_nacimiento VARCHAR(30);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS genero VARCHAR(40);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS telefono VARCHAR(30);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS estatus VARCHAR(40);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS carrera VARCHAR(160);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS calle VARCHAR(160);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS colonia VARCHAR(120);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS codigo_postal VARCHAR(20);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS municipio VARCHAR(120);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS estado VARCHAR(120);
ALTER TABLE alumnos ADD COLUMN IF NOT EXISTS ruta_foto_perfil TEXT;
