# Registro de Créditos Web API 🚀

Una API REST moderna, escalable y construida en **.NET 10** para la gestión y registro de créditos. 
Este proyecto fue diseñado con arquitectura **CQRS-lite**, separando claramente las lecturas y escrituras, y asegurando alta calidad a través de principios SOLID y un extenso conjunto de pruebas.

---

## 🛠️ Stack Tecnológico

- **Framework:** .NET 10
- **Base de Datos:** PostgreSQL
- **ORM (Escritura):** Entity Framework Core 10 (Code-First)
- **Micro-ORM (Lectura):** Dapper
- **Background Jobs:** Coravel (Envío de correos asíncronos)
- **Autenticación:** JWT (JSON Web Tokens)
- **Seguridad:** BCrypt (Hashing de contraseñas)
- **Validación:** FluentValidation
- **Pruebas:** xUnit, Moq, FluentAssertions
- **Despliegue:** Docker, GitHub Actions, Docker Compose

---

## 🏗️ Arquitectura y Patrones

El proyecto sigue una estructura limpia orientada a dominio simplificado:
*   **Controladores "Flacos":** Responsables únicamente de enrutar las peticiones HTTP y orquestar llamadas a servicios.
*   **Servicios (SRP):** Contienen toda la lógica de negocio. Se prohíbe inyectar DbContext en los controladores; todo se delega a ICreditoService, ICreditoQueryService, etc.
*   **CQRS-lite:** Se usa Entity Framework Core para operaciones complejas de escritura (Commands) y control de migraciones. Las operaciones de solo lectura (Queries) se ejecutan a través de Dapper para un rendimiento extremo.
*   **Background Jobs:** La creación de un crédito dispara un evento encolado (con Coravel) para notificar vía Mailgun (Producción) o SMTP/Mailpit (Desarrollo) sin bloquear la respuesta de la API.

*(Si deseas profundizar, revisa la documentación arquitectónica detallada en el archivo gent.md en la raíz del proyecto).*

---

## 📋 Requisitos Previos

- [Docker & Docker Compose](https://www.docker.com/) (Recomendado para correr todo el entorno localmente)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (Si deseas ejecutar/desarrollar por fuera de Docker)

---

## 🐳 Ejecución Completa con Docker Compose (Recomendado)

La forma más rápida y limpia de levantar todo el ecosistema (Base de Datos, Simulador de Correos y API Backend) es usando Docker Compose.

1. **Clona el repositorio**
   `ash
   git clone https://github.com/jairoboneth/RegistroCreditos.Api.git
   cd RegistroCreditos.WebApi
   `

2. **Levanta todo el stack**
   Esto compilará la API y levantará PostgreSQL y Mailpit.
   `ash
   docker-compose up -d --build
   `

3. **Aplica las Migraciones (Solo la primera vez)**
   Dado que Docker levanta una base de datos en blanco, necesitas ejecutar las migraciones para crear las tablas e insertar los datos semilla (10 créditos y 1 usuario de pruebas).
   `ash
   cd RegistroCreditos.Api
   dotnet ef database update
   cd ..
   `

4. **Accede a los servicios**
   - **Backend API (Swagger):** [http://localhost:8080/swagger](http://localhost:8080/swagger)
   - **Mailpit (Simulador de correos):** [http://localhost:8025](http://localhost:8025)

   > **¡Importante!** Usa las siguientes credenciales en Swagger (/api/auth/login) para obtener tu token JWT:
   > *   **Email:** 	est@empresa.com
   > *   **Password:** Pruebas123!

---

## 💻 Ejecución Manual sin Docker Compose

Si prefieres correr el proyecto directamente con el SDK de .NET:

1. Levanta únicamente los servicios de infraestructura (Postgres en el puerto 5432 y Mailpit en el puerto 1025).
2. Entra a la carpeta del backend y restaura los paquetes:
   `ash
   cd RegistroCreditos.Api
   dotnet restore
   dotnet ef database update
   `
3. Ejecuta la aplicación:
   `ash
   dotnet run
   `

---

## 🧪 Pruebas Automáticas

El proyecto cuenta con un conjunto extenso de pruebas unitarias. Para ejecutar los tests:
`ash
dotnet test
`

---

## 🚀 Despliegue (CI/CD)

El proyecto cuenta con:
1. Un **Dockerfile** optimizado (multi-stage build) usando imágenes alpinas de .NET 10.
2. Un pipeline en **GitHub Actions** (.github/workflows/ci.yml) que compila y ejecuta todas las pruebas unitarias automáticamente en cada *Push* a la rama main.

*(Consulta el archivo deployment_guide.md para ver el tutorial paso a paso sobre cómo desplegar en plataformas en la nube como Railway o Render).*