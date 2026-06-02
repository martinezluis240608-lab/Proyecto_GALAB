# Ajustes de UI (Jun 2026)

Este documento resume cambios visuales recientes para que sean faciles de mantener.

## 1) Pantalla principal (bienvenida y botones)

Archivo: `Views/PrincipalForm.Designer.cs`

- `lblDescripcion` se posiciona en `AjustarLayout()` entre `lblBienvenido` y la primera fila de botones.
- `lblBienvenido` se aumento a mayor tamano visual (fuente y altura).
- Botones principales con color azul claro uniforme (estilo original).
- Se incremento tamano minimo de botones y fuente para mejor legibilidad.

## 4) Tarjetas de incidencias en cero (sin BD)

Archivos:

- `Services/IncidenciaEstadisticasService.cs`
- `Views/GestionIncidenciasForm.cs`
- `Presenters/DashboardPresenter.cs`

Muestran **0** en Activas, En proceso y Resueltas hasta conectar base de datos.
Ver tambien `Docs/ROLES_Y_NAVEGACION.md`.

## 2) Historial de incidencias

Archivo: `Views/HistorialIncidenciasForm.cs`

- La tarjeta y la tabla se agrandaron para mostrar mejor contenido.
- Cabecera de tabla con azul mas bajo (suave): `Color.FromArgb(242, 248, 255)`.
- Se definieron `FillWeight` por columna para priorizar espacio en descripcion.
- Se aumento alto de filas y encabezados para mejor lectura.

## 3) Perfil (modo edicion en la misma vista)

Archivo: `Views/PerfilForm.cs`

- Se replica estructura de tres bloques:
  - Informacion general.
  - Informacion escolar.
  - Informacion de contacto.
- `Editar mis datos` habilita edicion inline.
- En edicion solo se muestran:
  - `Guardar`
  - `Cancelar`
- Se habilita cambio de foto solo en modo edicion.
- Al cancelar, se restauran valores del respaldo en memoria.

## Nota de mantenimiento

Los datos de perfil siguen en almacenamiento temporal (memoria) via `Services/PerfilUsuarioStore.cs`.
Cuando se conecte BD, reemplazar lecturas/escrituras de `PerfilUsuarioStore` por capa de datos.
