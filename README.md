# CitasApp

App de citas médicas construida con ASP.NET Core MVC (.NET 8).

## Entidades
- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente

## Datos
En memoria. Sin base de datos.

## Navegación
Escribe manualmente en la barra del navegador:
- `/Paciente` — lista de pacientes
- `/Medico` — lista de médicos
- `/Cita` — agenda completa
- `/Cita/PorPaciente?pacienteId=1` — citas de un paciente específico

## Requisitos
- .NET 8.0
- Visual Studio 2022