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
- **Despliegue:** Docker, GitHub Actions

---

## 🏗️ Arquitectura y Patrones

El proyecto sigue una estructura limpia orientada a dominio simplificado:
*   **Controladores "Flacos":** Responsables únicamente de enrutar las peticiones HTTP y orquestar llamadas a servicios.
*   **Servicios (SRP):** Contienen toda la lógica de negocio. Se prohíbe inyectar `DbContext` en los controladores; todo se delega a `ICreditoService`, `ICreditoQueryService`, etc.
*   **CQRS-lite:** Se usa Entity Framework Core para operaciones complejas de escritura (Commands) y control de migraciones. Las operaciones de solo lectura (Queries) se ejecutan a través de Dapper para un rendimiento extremo.
*   **Background Jobs:** La creación de un crédito dispara un evento encolado (con Coravel) para notificar vía SMTP sin bloquear la respuesta de la API.

*(Si deseas profundizar, revisa la documentación arquitectónica detallada en el archivo `agent.md` en la raíz del proyecto).*

---

## ⚙️ Requisitos Previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker & Docker Compose](https://www.docker.com/) (Para la base de datos PostgreSQL)
- (Opcional) Mailpit para pruebas locales de SMTP

---

## 🚀 Inicio Rápido (Local)

1. **Clona el repositorio**
   ```bash
   git clone https://github.com/jairoboneth/RegistroCreditos.Api.git
   cd RegistroCreditos.WebApi
   ```

2. **Levanta los servicios de infraestructura** (Postgres y Mailpit):
   Asegúrate de tener un contenedor de Postgres corriendo localmente en el puerto `5432` y con las credenciales configuradas en tu `appsettings.json`.

3. **Aplica las Migraciones de Base de Datos**
   El proyecto ya viene con datos semillas (*seeding*) de 10 créditos y 1 usuario de pruebas.
   ```bash
   cd RegistroCreditos.Api
   dotnet ef database update
   ```

4. **Ejecuta la API**
   ```bash
   dotnet run
   ```

5. **Prueba en Swagger**
   Abre tu navegador en `https://localhost:xxxx/swagger`.
   Utiliza el endpoint `/api/auth/login` con:
   *   **Email:** `test@empresa.com`
   *   **Password:** `Pruebas123!`
   
   Copia el token devuelto, haz clic en el botón **"Authorize"** e ingresa `Bearer {tu_token}`. ¡Estás listo para consumir los endpoints protegidos!

---

## 🧪 Pruebas Automáticas

El proyecto cuenta con un conjunto extenso de pruebas unitarias etiquetadas con Traits (`[Trait("Category", "Positive")]`, etc.).

Para ejecutar los tests:
```bash
dotnet test
```

---

## 📦 Despliegue (CI/CD)

El proyecto cuenta con:
1. Un **Dockerfile** optimizado (multi-stage build) usando imágenes alpinas de .NET 10.
2. Un pipeline en **GitHub Actions** (`.github/workflows/ci.yml`) que compila y ejecuta todas las pruebas unitarias automáticamente en cada *Push* a la rama `main`.

*(Consulta el archivo `deployment_guide.md` para ver el tutorial paso a paso sobre cómo desplegar en plataformas en la nube como Railway o Render, incluyendo la configuración SMTP de Mailgun).*
