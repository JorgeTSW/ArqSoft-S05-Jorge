# CitasApp

App de citas médicas construida con ASP.NET Core MVC (.NET 10).

## Arquitectura
Hexagonal (Ports & Adapters) dividida en cuatro proyectos:

- **CitasApp.Domain** — modelos e interfaces (sin dependencias externas)
- **CitasApp.Application** — servicios de aplicación (orquesta el Domain)
- **CitasApp.Infrastructure** — repositorios JSON y en memoria (implementa las interfaces del Domain)
- **CitasApp.Web** — controllers, views y configuración (MVC)

## Flujo de dependencias
```
Web → Application → Domain ← Infrastructure
```

## Entidades
- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente

## Persistencia
Archivos JSON en `CitasApp.Web/data/`
- `pacientes.json`
- `medicos.json`
- `citas.json`

También incluye `MemoriaPacienteRepository` para demostrar el swap de adapter.

## Navegación
- `/Paciente` — lista de pacientes
- `/Medico` — lista de médicos
- `/Cita` — agenda completa
- `/Cita/PorPaciente?pacienteId=1` — citas de un paciente específico

## Requisitos
- .NET 10.0
- Visual Studio 2022

## Ramas
- `main` — estado evaluable con persistencia JSON en un solo proyecto
- `hexagonal` — arquitectura hexagonal multi-proyecto con capa de aplicación