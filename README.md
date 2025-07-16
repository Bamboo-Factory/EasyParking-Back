# EasyParking - Sistema de Gestión de Estacionamientos

## Descripción

EasyParking es un sistema de backend desarrollado en .NET 8 que implementa Clean Architecture para la gestión y reserva de estacionamientos. El proyecto está diseñado con una estructura moderna, limpia y escalable, configurado específicamente para Lima, Perú.

## Arquitectura

El proyecto sigue los principios de Clean Architecture con las siguientes capas:

### 🏗️ Estructura del Proyecto

```
EasyParking/
├── EasyParking.API/           # Capa de presentación (Web API)
├── EasyParking.Application/   # Capa de aplicación (Casos de uso)
├── EasyParking.Core/          # Capa de dominio (Entidades y reglas de negocio)
└── EasyParking.Infrastructure/# Capa de infraestructura (Base de datos, repositorios)
```

### 📋 Capas de Clean Architecture

1. **Core (Dominio)**
   - Entidades del dominio
   - Interfaces de repositorios
   - Excepciones del dominio
   - Reglas de negocio

2. **Application (Aplicación)**
   - DTOs
   - Interfaces de servicios
   - Implementación de servicios
   - Casos de uso

3. **Infrastructure (Infraestructura)**
   - Contexto de Entity Framework
   - Implementación de repositorios
   - Unit of Work
   - Configuración de base de datos

4. **API (Presentación)**
   - Controladores REST
   - Configuración de servicios
   - Middleware

## 🚀 Tecnologías Utilizadas

- **.NET 8**
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Core Web API**
- **Swagger/OpenAPI**

## 📊 Entidades del Dominio

### User (Usuario)
- Información personal del usuario
- Roles: Customer, Admin, ParkingOwner
- Gestión de autenticación

### Parking (Estacionamiento)
- Información del estacionamiento
- Ubicación (latitud/longitud)
- Tarifas por hora y día
- Estado del estacionamiento

### ParkingSpace (Espacio de Estacionamiento)
- Espacios individuales dentro del estacionamiento
- Tipos: Standard, Disabled, Electric, Motorcycle, Large
- Estados: Available, Occupied, Reserved, Maintenance, OutOfService

### Reservation (Reserva)
- Reservas de espacios de estacionamiento
- Estados: Pending, Confirmed, Active, Completed, Cancelled, Expired
- Estados de pago: Pending, Paid, Failed, Refunded

## 🔧 Configuración

### Prerrequisitos
- .NET 8 SDK
- SQL Server (LocalDB o SQL Server Express)
- Visual Studio 2022 o VS Code

### Instalación

1. **Clonar el repositorio**
   ```bash
   git clone <repository-url>
   cd EasyParking-Back
   ```

2. **Restaurar dependencias**
   ```bash
   dotnet restore
   ```

3. **Configurar la base de datos**

   **Opción A: Usando el script automatizado (Recomendado)**
   ```powershell
   # Desde PowerShell, ejecutar:
   .\scripts\setup-database.ps1
   ```

   **Opción B: Manual**
   ```bash
   # Aplicar migraciones
   dotnet ef database update --project EasyParking.Infrastructure --startup-project EasyParking.API
   
   # Opcional: Insertar datos de ejemplo de Lima, Perú
   # Ejecutar el script: EasyParking.Infrastructure/Data/SeedData.sql
   ```

4. **Ejecutar el proyecto**
   ```bash
   dotnet run --project EasyParking.API
   ```

### 🗄️ Configuración de Base de Datos

#### Migraciones

**Crear una nueva migración:**
```bash
dotnet ef migrations add NombreDeLaMigracion --project EasyParking.Infrastructure --startup-project EasyParking.API
```

**Aplicar migraciones:**
```bash
dotnet ef database update --project EasyParking.Infrastructure --startup-project EasyParking.API
```

**Revertir última migración:**
```bash
dotnet ef migrations remove --project EasyParking.Infrastructure --startup-project EasyParking.API
```

**Generar script SQL:**
```bash
dotnet ef migrations script --project EasyParking.Infrastructure --startup-project EasyParking.API
```

#### Entornos

- **Desarrollo**: `appsettings.Development.json`
- **Producción**: `appsettings.Production.json`

**Configurar cadena de conexión para producción:**
1. Editar `appsettings.Production.json`
2. Reemplazar los valores de conexión con los de tu servidor

## 📚 Endpoints de la API

### Users
- `GET /api/users` - Obtener todos los usuarios
- `GET /api/users/{id}` - Obtener usuario por ID
- `GET /api/users/email/{email}` - Obtener usuario por email
- `POST /api/users` - Crear nuevo usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario

### Parkings
- `GET /api/parkings` - Obtener todos los estacionamientos
- `GET /api/parkings/{id}` - Obtener estacionamiento por ID
- `GET /api/parkings/city/{city}` - Obtener estacionamientos por ciudad
- `GET /api/parkings/available` - Obtener estacionamientos disponibles
- `GET /api/parkings/search/location` - Búsqueda por ubicación (en memoria)
- `GET /api/parkings/search/location/optimized` - Búsqueda por ubicación (optimizada)
- `POST /api/parkings` - Crear nuevo estacionamiento
- `PUT /api/parkings/{id}` - Actualizar estacionamiento
- `DELETE /api/parkings/{id}` - Eliminar estacionamiento

### Reservations
- `GET /api/reservations` - Obtener todas las reservas
- `GET /api/reservations/{id}` - Obtener reserva por ID
- `GET /api/reservations/user/{userId}` - Obtener reservas por usuario
- `GET /api/reservations/parking/{parkingId}` - Obtener reservas por estacionamiento
- `GET /api/reservations/active` - Obtener reservas activas
- `POST /api/reservations` - Crear nueva reserva
- `PUT /api/reservations/{id}` - Actualizar reserva
- `DELETE /api/reservations/{id}` - Eliminar reserva
- `POST /api/reservations/{id}/confirm` - Confirmar reserva
- `POST /api/reservations/{id}/cancel` - Cancelar reserva
- `POST /api/reservations/{id}/checkin` - Check-in de reserva
- `POST /api/reservations/{id}/checkout` - Check-out de reserva

## 🗺️ Ubicaciones en Lima, Perú

### Estacionamientos Disponibles

| Estacionamiento | Ubicación | Distrito | Coordenadas |
|----------------|-----------|----------|-------------|
| Plaza San Martín | Centro Histórico | Lima | -12.0464, -77.0428 |
| Universidad San Marcos | Av. Universitaria | San Miguel | -12.0169, -77.0428 |
| Hospital Rebagliati | Av. Rebagliati | Jesús María | -12.0764, -77.0428 |
| Aeropuerto Jorge Chávez | Av. Elmer Faucett | Callao | -12.0219, -77.1143 |
| Plaza San Miguel | Av. La Marina | San Miguel | -12.0764, -77.0928 |
| Larcomar | Malecón de la Reserva | Miraflores | -12.1464, -77.0228 |
| Jockey Plaza | Av. Javier Prado Este | Surco | -12.1964, -76.9828 |
| Plaza Mayor | Centro Histórico | Lima | -12.0464, -77.0328 |

### Ejemplos de Búsqueda por Ubicación

**Búsqueda en Plaza San Martín:**
```
GET /api/parkings/search/location?latitude=-12.0464&longitude=-77.0428&radiusKm=2.0
```

**Búsqueda en Miraflores:**
```
GET /api/parkings/search/location?latitude=-12.1464&longitude=-77.0228&radiusKm=3.0
```

**Búsqueda cerca del Aeropuerto:**
```
GET /api/parkings/search/location?latitude=-12.0219&longitude=-77.1143&radiusKm=10.0
```

## 🔍 Características Principales

### Gestión de Usuarios
- Registro y autenticación de usuarios
- Diferentes roles de usuario
- Validación de datos únicos (email, placa)

### Gestión de Estacionamientos
- CRUD completo de estacionamientos
- Búsqueda por ubicación y ciudad
- Control de espacios disponibles
- Gestión de tarifas

### Sistema de Reservas
- Creación de reservas con validación de conflictos
- Estados de reserva y pago
- Check-in y check-out
- Cálculo automático de tarifas

### Búsqueda por Ubicación
- Búsqueda en memoria para volúmenes pequeños
- Búsqueda optimizada para grandes volúmenes
- Fórmula de Haversine para cálculos precisos
- Ordenamiento por distancia

### Validaciones de Negocio
- Verificación de disponibilidad de espacios
- Validación de horarios sin conflictos
- Control de estados de reservas
- Validación de datos únicos

## 🛠️ Desarrollo

### Agregar una nueva entidad

1. **Crear la entidad en Core**
   ```csharp
   public class NewEntity : BaseEntity
   {
       // Propiedades
   }
   ```

2. **Crear la interfaz del repositorio en Core**
   ```csharp
   public interface INewEntityRepository : IRepository<NewEntity>
   {
       // Métodos específicos
   }
   ```

3. **Crear DTOs en Application**
   ```csharp
   public class NewEntityDto
   {
       // Propiedades del DTO
   }
   ```

4. **Crear el servicio en Application**
   ```csharp
   public interface INewEntityService
   {
       // Métodos del servicio
   }
   ```

5. **Implementar el repositorio en Infrastructure**
   ```csharp
   public class NewEntityRepository : BaseRepository<NewEntity>, INewEntityRepository
   {
       // Implementación
   }
   ```

6. **Crear el controlador en API**
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class NewEntitiesController : ControllerBase
   {
       // Endpoints
   }
   ```

7. **Crear migración**
   ```bash
   dotnet ef migrations add AddNewEntity --project EasyParking.Infrastructure --startup-project EasyParking.API
   dotnet ef database update --project EasyParking.Infrastructure --startup-project EasyParking.API
   ```

## 📝 Patrones Utilizados

- **Repository Pattern**: Abstracción del acceso a datos
- **Unit of Work**: Gestión de transacciones
- **DTO Pattern**: Transferencia de datos entre capas
- **Dependency Injection**: Inversión de dependencias
- **Clean Architecture**: Separación de responsabilidades

## 🔒 Seguridad

- Validación de datos de entrada
- Manejo de excepciones personalizado
- CORS configurado para desarrollo
- Hash de contraseñas con SHA256

## 🚀 Próximas Mejoras

- [ ] Autenticación JWT
- [ ] Autorización basada en roles
- [ ] Logging estructurado
- [ ] Caché con Redis
- [ ] Tests unitarios y de integración
- [ ] Documentación con Swagger
- [ ] Migración a PostgreSQL
- [ ] Implementación de paginación
- [ ] Filtros avanzados de búsqueda
- [ ] Notificaciones por email
- [ ] Integración con mapas (Google Maps, OpenStreetMap)
- [ ] Sistema de pagos en línea
- [ ] Aplicación móvil

## 📄 Licencia

Este proyecto está bajo la Licencia MIT.

## 👥 Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📞 Contacto

Para preguntas o soporte, contacta al equipo de desarrollo. 