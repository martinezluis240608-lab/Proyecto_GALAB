# Roles y navegación (GALAB)

## Flujo de login

| Pestaña seleccionada | Pantalla destino | Archivo |
|----------------------|------------------|---------|
| **Estudiante** | Apartado principal (menú estudiante) | `Views/PrincipalForm.cs` |
| **Administrador** | Gestión de incidencias (panel admin) | `Views/Admin/AdminGestionIncidenciasForm.cs` |

Archivos clave:

- `Views/LoginForm.cs` — navegación según rol.
- `Presenters/LoginPresenter.cs` — valida credenciales y decide ruta.
- `Models/Usuario.cs` — autenticación temporal (sin BD).
- `Services/SesionActual.cs` — guarda usuario y rol en memoria.

### Credenciales temporales (sin BD)

- **Estudiante:** usuario y contraseña no vacíos (cualquier valor de prueba).
- **Administrador:** `admin` / `admin`.

Cuando conectes BD, cambia solo `Usuario.Autenticar` y deja el presenter igual.

## Módulo administrador

Carpeta: `Views/Admin/`

Pantallas implementadas (ver `Docs/MODULO_ADMIN.md`):

1. **Perfil** — `AdminPerfilForm` (sin carrera ni datos escolares).
2. **Gestión de incidencias** — `AdminGestionIncidenciasForm`.
3. **Gestión de usuarios** — `AdminGestionUsuariosForm`.

## Estadísticas de incidencias (tarjetas en cero)

Servicio: `Services/IncidenciaEstadisticasService.cs`

- Hoy devuelve **0** en Activas, En proceso y Resueltas.
- Usado en:
  - `Views/GestionIncidenciasForm.cs`
  - `Presenters/DashboardPresenter.cs`

Para conectar BD: implementar consultas en `ObtenerResumen()` o crear una clase nueva que implemente `IIncidenciaEstadisticasService` y registrarla donde se use.
