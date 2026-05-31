# CitasApp

App de citas médicas construida con ASP.NET Core (.NET 10).

## Arquitectura
Hexagonal (Ports & Adapters) dividida en cinco proyectos:

- **CitasApp.Domain** — modelos e interfaces (sin dependencias externas)
- **CitasApp.Application** — servicios de aplicación (orquesta el Domain)
- **CitasApp.Infrastructure** — repositorios JSON y en memoria
- **CitasApp.Web** — cliente MVC para navegador
- **CitasApp.Api** — cliente API REST para cualquier dispositivo

## Flujo de dependencias
```bash
Web  → Application → Domain ← Infrastructure
Api  → Application → Domain ← Infrastructure
```

## Entidades
- **Paciente** — lista y detalle de pacientes registrados
- **Médico** — lista y detalle de médicos disponibles
- **Cita** — agenda completa y filtro por paciente

## Persistencia
Archivos JSON en `data/` dentro de cada proyecto cliente.

## Endpoints API REST
- `GET /api/pacientes` — lista de pacientes
- `GET /api/pacientes/{id}` — detalle de un paciente
- `GET /api/medicos` — lista de médicos
- `GET /api/medicos/{id}` — detalle de un médico
- `GET /api/citas` — agenda completa
- `GET /api/citas/porpaciente/{pacienteId}` — citas de un paciente

## Navegación Web (MVC)
- `/Paciente` — lista de pacientes
- `/Medico` — lista de médicos
- `/Cita` — agenda completa
- `/Cita/PorPaciente?pacienteId=1` — citas de un paciente

## Patrones GOF implementados

- **Factory** (`RepositoryFactory`) — selecciona el repositorio según el entorno (Development → JSON, Production → Memoria)
- **Decorator** (`LoggingPacienteRepository`) — agrega logging con timestamp sin modificar el repositorio original

## Requisitos
- .NET 10.0
- Visual Studio 2022

## Ramas
- `main` — estado evaluable con persistencia JSON en un solo proyecto
- `hexagonal` — arquitectura hexagonal multi-proyecto con capa de aplicación
- `Api` — API REST expuesta como segundo cliente del núcleo de negocio