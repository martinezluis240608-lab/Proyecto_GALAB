# Módulo administrador (GALAB)

## Pantallas

| Pantalla | Archivo | Descripción |
|----------|---------|-------------|
| Gestión de incidencias | `Views/Admin/AdminGestionIncidenciasForm.cs` | Tarjetas en 0, listado, búsqueda, filtro estado, nueva/editar/ver/eliminar |
| Gestión de usuarios | `Views/Admin/AdminGestionUsuariosForm.cs` | Listado vacío hasta BD, CRUD en memoria |
| Perfil administrador | `Views/Admin/AdminPerfilForm.cs` | Igual al estudiante **sin** datos escolares |

## Navegación

- Sidebar compartido: `Views/Admin/AdminSidebar.cs`
- Login como administrador abre **Gestión de incidencias**.
- Menú lateral cambia entre las 3 pantallas sin perder sesión.

## Datos temporales (sin BD)

| Store | Archivo |
|-------|---------|
| Incidencias listado | `Services/IncidenciaListadoStore.cs` |
| Estadísticas tarjetas | `Services/IncidenciaEstadisticasService.cs` (lee del store) |
| Usuarios | `Services/UsuarioSistemaStore.cs` |
| Perfil admin | `Services/PerfilAdministradorStore.cs` |

Al registrar una incidencia desde `IncidenciaForm`, se agrega al listado y las tarjetas se actualizan.

## Conectar base de datos (futuro)

1. Reemplazar métodos en `*Store` por consultas SQL/repositorio.
2. Mantener los formularios; solo cambia la capa `Services`.
3. Credenciales reales en `Models/Usuario.Autenticar`.

## Botones con uso

- **Nueva incidencia:** abre `IncidenciaForm` y al guardar suma al listado.
- **Ver / Editar / Eliminar incidencia:** detalle, `AdminIncidenciaEditorForm`, confirmación de borrado.
- **Nuevo / Editar / Eliminar usuario:** `AdminUsuarioEditorForm` y store en memoria.
- **Editar perfil:** habilita campos; solo **Guardar** y **Cancelar**; cambio de foto en edición.
