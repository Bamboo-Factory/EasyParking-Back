# Guía de Migraciones - EasyParking

## 📋 Descripción

Este documento describe cómo trabajar con las migraciones de Entity Framework Core en el proyecto EasyParking.

## 🛠️ Comandos Básicos

### Crear una nueva migración

```bash
dotnet ef migrations add NombreDeLaMigracion --project EasyParking.Infrastructure --startup-project EasyParking.API
```

**Ejemplos:**
```bash
# Agregar una nueva tabla
dotnet ef migrations add AddPaymentTable --project EasyParking.Infrastructure --startup-project EasyParking.API

# Modificar una tabla existente
dotnet ef migrations add AddPhoneNumberToUsers --project EasyParking.Infrastructure --startup-project EasyParking.API

# Agregar un índice
dotnet ef migrations add AddIndexToParkingName --project EasyParking.Infrastructure --startup-project EasyParking.API
```

### Aplicar migraciones

```bash
dotnet ef database update --project EasyParking.Infrastructure --startup-project EasyParking.API
```

**Aplicar migración específica:**
```bash
dotnet ef database update NombreDeLaMigracion --project EasyParking.Infrastructure --startup-project EasyParking.API
```

### Revertir migraciones

**Revertir última migración:**
```bash
dotnet ef migrations remove --project EasyParking.Infrastructure --startup-project EasyParking.API
```

**Revertir a migración específica:**
```bash
dotnet ef database update NombreDeLaMigracion --project EasyParking.Infrastructure --startup-project EasyParking.API
```

### Generar script SQL

```bash
dotnet ef migrations script --project EasyParking.Infrastructure --startup-project EasyParking.API
```

**Generar script desde migración específica:**
```bash
dotnet ef migrations script DesdeMigracion HastaMigracion --project EasyParking.Infrastructure --startup-project EasyParking.API
```

### Listar migraciones

```bash
dotnet ef migrations list --project EasyParking.Infrastructure --startup-project EasyParking.API
```

## 🔄 Flujo de Trabajo

### 1. Desarrollo Local

1. **Hacer cambios en las entidades**
   ```csharp
   public class User : BaseEntity
   {
       // Agregar nueva propiedad
       public string PhoneNumber { get; set; }
   }
   ```

2. **Crear migración**
   ```bash
   dotnet ef migrations add AddPhoneNumberToUsers --project EasyParking.Infrastructure --startup-project EasyParking.API
   ```

3. **Revisar la migración generada**
   - Verificar el archivo `.cs` en `EasyParking.Infrastructure/Migrations/`
   - Asegurar que los cambios son correctos

4. **Aplicar migración**
   ```bash
   dotnet ef database update --project EasyParking.Infrastructure --startup-project EasyParking.API
   ```

### 2. Entorno de Producción

1. **Generar script SQL**
   ```bash
   dotnet ef migrations script --project EasyParking.Infrastructure --startup-project EasyParking.API --output migrations.sql
   ```

2. **Revisar el script**
   - Verificar que no hay operaciones destructivas
   - Asegurar compatibilidad con datos existentes

3. **Ejecutar en producción**
   ```sql
   -- Ejecutar el script migrations.sql en SQL Server
   ```

## ⚠️ Consideraciones Importantes

### Antes de Crear una Migración

1. **Hacer backup de la base de datos**
   ```sql
   BACKUP DATABASE EasyParkingDb TO DISK = 'C:\Backups\EasyParkingDb_Backup.bak'
   ```

2. **Verificar cambios en entidades**
   - Revisar todas las propiedades modificadas
   - Verificar relaciones entre entidades
   - Comprobar configuraciones en `OnModelCreating`

3. **Probar en entorno de desarrollo**
   - Aplicar migración en base de datos de desarrollo
   - Ejecutar tests si están disponibles
   - Verificar funcionalidad de la aplicación

### Durante la Migración

1. **Revisar el código generado**
   ```csharp
   // En el archivo de migración
   migrationBuilder.AddColumn<string>(
       name: "PhoneNumber",
       table: "Users",
       type: "nvarchar(20)",
       maxLength: 20,
       nullable: false,
       defaultValue: "");
   ```

2. **Verificar operaciones críticas**
   - Eliminación de columnas
   - Cambios de tipo de datos
   - Modificación de restricciones

3. **Agregar datos de migración si es necesario**
   ```csharp
   migrationBuilder.InsertData(
       table: "Users",
       columns: new[] { "FirstName", "LastName", "Email" },
       values: new object[] { "Admin", "User", "admin@easyparking.com" });
   ```

### Después de la Migración

1. **Verificar integridad de datos**
   ```sql
   SELECT COUNT(*) FROM Users;
   SELECT COUNT(*) FROM Parkings;
   SELECT COUNT(*) FROM Reservations;
   ```

2. **Probar funcionalidad crítica**
   - Crear nuevos registros
   - Actualizar registros existentes
   - Ejecutar consultas complejas

3. **Actualizar documentación**
   - Actualizar este archivo si es necesario
   - Documentar cambios en el README

## 🚨 Problemas Comunes

### Error: "The migration 'X' has already been applied"

**Solución:**
```bash
# Verificar estado de migraciones
dotnet ef migrations list --project EasyParking.Infrastructure --startup-project EasyParking.API

# Si es necesario, revertir y volver a aplicar
dotnet ef database update MigracionAnterior --project EasyParking.Infrastructure --startup-project EasyParking.API
dotnet ef database update --project EasyParking.Infrastructure --startup-project EasyParking.API
```

### Error: "Cannot drop column because it is referenced by a foreign key"

**Solución:**
1. Eliminar la foreign key primero
2. Eliminar la columna
3. Crear nueva migración para agregar la nueva estructura

### Error: "The database is in use"

**Solución:**
1. Cerrar todas las conexiones a la base de datos
2. Detener la aplicación si está ejecutándose
3. Intentar la migración nuevamente

## 📁 Estructura de Archivos

```
EasyParking.Infrastructure/
└── Migrations/
    ├── 20250629064039_InitialCreate.cs
    ├── 20250629064039_InitialCreate.Designer.cs
    ├── 20250629064120_UpdateDecimalPrecision.cs
    ├── 20250629064120_UpdateDecimalPrecision.Designer.cs
    └── EasyParkingDbContextModelSnapshot.cs
```

## 🔧 Configuración Avanzada

### Migración con datos de ejemplo

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    // Crear tabla
    migrationBuilder.CreateTable(
        name: "PaymentMethods",
        columns: table => new
        {
            Id = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
            IsActive = table.Column<bool>(type: "bit", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("PK_PaymentMethods", x => x.Id);
        });

    // Insertar datos
    migrationBuilder.InsertData(
        table: "PaymentMethods",
        columns: new[] { "Name", "IsActive" },
        values: new object[,]
        {
            { "Tarjeta de Crédito", true },
            { "Tarjeta de Débito", true },
            { "Efectivo", true },
            { "Transferencia", true }
        });
}
```

### Migración con validaciones

```csharp
protected override void Up(MigrationBuilder migrationBuilder)
{
    // Agregar columna con valor por defecto
    migrationBuilder.AddColumn<string>(
        name: "Status",
        table: "Reservations",
        type: "nvarchar(20)",
        maxLength: 20,
        nullable: false,
        defaultValue: "Pending");

    // Actualizar registros existentes
    migrationBuilder.Sql("UPDATE Reservations SET Status = 'Completed' WHERE EndTime < GETUTCDATE()");
}
```

## 📞 Soporte

Si encuentras problemas con las migraciones:

1. Revisar los logs de Entity Framework
2. Verificar la configuración de conexión
3. Consultar la documentación oficial de EF Core
4. Contactar al equipo de desarrollo

## 🔗 Enlaces Útiles

- [Entity Framework Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [EF Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet/)
- [SQL Server Migration Scripts](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/#generate-a-sql-script) 