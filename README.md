# CitasApp

App de citas médicas construida con ASP.NET Core MVC (.NET 10).

## Arquitectura
Hexagonal (Ports & Adapters) dividida en tres proyectos:

- **CitasApp.Domain** — modelos e interfaces (sin dependencias externas)
- **CitasApp.Infrastructure** — repositorios JSON (implementa las interfaces del Domain)
- **CitasApp.Web** — controllers, views y configuración (MVC)

## Entidades
- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente

## Persistencia
Archivos JSON en `CitasApp.Web/data/`
- `pacientes.json`
- `medicos.json`
- `citas.json`

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
- `hexagonal` — refactorización a arquitectura multi-proyecto