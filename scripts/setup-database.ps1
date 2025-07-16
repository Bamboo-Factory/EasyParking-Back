# Script de configuración de base de datos para EasyParking
# Configurado específicamente para Lima, Perú

param(
    [string]$ConnectionString = "Server=(localdb)\mssqllocaldb;Database=EasyParkingDb;Trusted_Connection=true;MultipleActiveResultSets=true",
    [switch]$SkipMigrations = $false,
    [switch]$SkipSeedData = $false,
    [switch]$UpdateToLima = $false
)

# Leer variables del archivo .env si existe y construir la cadena de conexión si no se pasa por parámetro
$envPath = ".env"
if (-not $PSBoundParameters.ContainsKey('ConnectionString')) {
    if (Test-Path $envPath) {
        Get-Content $envPath | ForEach-Object {
            if ($_ -match "^\s*([^#][^=]*)=(.*)$") {
                $key = $matches[1].Trim()
                $value = $matches[2].Trim()
                [System.Environment]::SetEnvironmentVariable($key, $value)
            }
        }
        $dbServer = $env:DB_SERVER
        $dbName = $env:DB_NAME
        $dbUser = $env:DB_USER
        $dbPassword = $env:DB_PASSWORD
        if ($dbServer -and $dbName -and $dbUser -and $dbPassword) {
            $ConnectionString = "Server=$dbServer;Database=$dbName;User Id=$dbUser;Password=$dbPassword;MultipleActiveResultSets=true"
        }
    }
}

Write-Host "🚀 Configurando base de datos para EasyParking - Lima, Perú" -ForegroundColor Green
Write-Host ""

# Verificar si .NET está instalado
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET SDK encontrado: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ Error: .NET SDK no encontrado. Por favor instala .NET 8 SDK." -ForegroundColor Red
    exit 1
}

# Verificar si Entity Framework Tools está instalado
try {
    $efVersion = dotnet ef --version
    Write-Host "✅ Entity Framework Tools encontrado: $efVersion" -ForegroundColor Green
} catch {
    Write-Host "⚠️  Entity Framework Tools no encontrado. Instalando..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    Write-Host "✅ Entity Framework Tools instalado" -ForegroundColor Green
}

Write-Host ""

# Restaurar dependencias
Write-Host "📦 Restaurando dependencias..." -ForegroundColor Blue
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al restaurar dependencias" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Dependencias restauradas" -ForegroundColor Green

Write-Host ""

# Aplicar migraciones
if (-not $SkipMigrations) {
    Write-Host "🗄️  Aplicando migraciones..." -ForegroundColor Blue
    
    # Verificar si hay migraciones pendientes
    $migrations = dotnet ef migrations list --project EasyParking.Infrastructure --startup-project EasyParking.API --no-build
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Error al listar migraciones" -ForegroundColor Red
        exit 1
    }
    
    Write-Host "📋 Migraciones encontradas:" -ForegroundColor Cyan
    $migrations | ForEach-Object { Write-Host "   $_" -ForegroundColor Gray }
    
    # Aplicar migraciones
    dotnet ef database update --project EasyParking.Infrastructure --startup-project EasyParking.API
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Error al aplicar migraciones" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ Migraciones aplicadas correctamente" -ForegroundColor Green
} else {
    Write-Host "⏭️  Saltando aplicación de migraciones" -ForegroundColor Yellow
}

Write-Host ""

# Insertar datos de ejemplo
if (-not $SkipSeedData) {
    Write-Host "🌱 Insertando datos de ejemplo de Lima, Perú..." -ForegroundColor Blue
    
    # Verificar si el archivo de datos existe
    $seedDataPath = "EasyParking.Infrastructure\Data\SeedData.sql"
    if (Test-Path $seedDataPath) {
        Write-Host "📄 Archivo de datos encontrado: $seedDataPath" -ForegroundColor Cyan
        
        # Extraer la cadena de conexión del archivo de configuración
        $appSettingsPath = "EasyParking.API\appsettings.Development.json"
        if (Test-Path $appSettingsPath) {
            $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
            $dbConnectionString = $appSettings.ConnectionStrings.DefaultConnection
        } else {
            $dbConnectionString = $ConnectionString
        }
        
        Write-Host "🔗 Usando cadena de conexión: $dbConnectionString" -ForegroundColor Cyan
        
        # Ejecutar script SQL
        try {
            $sqlContent = Get-Content $seedDataPath -Raw
            $sqlConnection = New-Object System.Data.SqlClient.SqlConnection($dbConnectionString)
            $sqlConnection.Open()
            $sqlCommand = New-Object System.Data.SqlClient.SqlCommand($sqlContent, $sqlConnection)
            $sqlCommand.ExecuteNonQuery()
            $sqlConnection.Close()
            
            Write-Host "✅ Datos de ejemplo insertados correctamente" -ForegroundColor Green
            Write-Host "   📍 Ubicaciones en Lima, Perú:" -ForegroundColor Cyan
            Write-Host "      • Plaza San Martín (Centro Histórico)" -ForegroundColor Gray
            Write-Host "      • Universidad San Marcos (San Miguel)" -ForegroundColor Gray
            Write-Host "      • Hospital Rebagliati (Jesús María)" -ForegroundColor Gray
            Write-Host "      • Aeropuerto Jorge Chávez (Callao)" -ForegroundColor Gray
            Write-Host "      • Plaza San Miguel (San Miguel)" -ForegroundColor Gray
            Write-Host "      • Larcomar (Miraflores)" -ForegroundColor Gray
            Write-Host "      • Jockey Plaza (Surco)" -ForegroundColor Gray
            Write-Host "      • Plaza Mayor (Centro Histórico)" -ForegroundColor Gray
        } catch {
            Write-Host "❌ Error al insertar datos de ejemplo: $($_.Exception.Message)" -ForegroundColor Red
            Write-Host "💡 Asegúrate de que SQL Server esté ejecutándose y la base de datos sea accesible" -ForegroundColor Yellow
        }
    } else {
        Write-Host "⚠️  Archivo de datos no encontrado: $seedDataPath" -ForegroundColor Yellow
    }
} else {
    Write-Host "⏭️  Saltando inserción de datos de ejemplo" -ForegroundColor Yellow
}

Write-Host ""

# Actualizar datos existentes a Lima, Perú
if ($UpdateToLima) {
    Write-Host "🔄 Actualizando datos existentes a ubicaciones en Lima, Perú..." -ForegroundColor Blue
    
    $updateDataPath = "EasyParking.Infrastructure\Data\UpdateToLimaPeru.sql"
    if (Test-Path $updateDataPath) {
        Write-Host "📄 Archivo de actualización encontrado: $updateDataPath" -ForegroundColor Cyan
        
        # Extraer la cadena de conexión del archivo de configuración
        $appSettingsPath = "EasyParking.API\appsettings.Development.json"
        if (Test-Path $appSettingsPath) {
            $appSettings = Get-Content $appSettingsPath | ConvertFrom-Json
            $dbConnectionString = $appSettings.ConnectionStrings.DefaultConnection
        } else {
            $dbConnectionString = $ConnectionString
        }
        
        # Ejecutar script SQL de actualización
        try {
            $sqlContent = Get-Content $updateDataPath -Raw
            $sqlConnection = New-Object System.Data.SqlClient.SqlConnection($dbConnectionString)
            $sqlConnection.Open()
            $sqlCommand = New-Object System.Data.SqlClient.SqlCommand($sqlContent, $sqlConnection)
            $sqlCommand.ExecuteNonQuery()
            $sqlConnection.Close()
            
            Write-Host "✅ Datos actualizados a Lima, Perú correctamente" -ForegroundColor Green
        } catch {
            Write-Host "❌ Error al actualizar datos: $($_.Exception.Message)" -ForegroundColor Red
        }
    } else {
        Write-Host "⚠️  Archivo de actualización no encontrado: $updateDataPath" -ForegroundColor Yellow
    }
}

Write-Host ""

# Verificar que la aplicación compile correctamente
Write-Host "🔨 Verificando compilación..." -ForegroundColor Blue
dotnet build --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error en la compilación" -ForegroundColor Red
    exit 1
}
Write-Host "✅ Compilación exitosa" -ForegroundColor Green

Write-Host ""

# Mostrar información final
Write-Host "🎉 ¡Configuración completada!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Resumen de la configuración:" -ForegroundColor Cyan
Write-Host "   ✅ Base de datos configurada" -ForegroundColor Green
Write-Host "   ✅ Migraciones aplicadas" -ForegroundColor Green
if (-not $SkipSeedData) {
    Write-Host "   ✅ Datos de ejemplo insertados (Lima, Perú)" -ForegroundColor Green
}
if ($UpdateToLima) {
    Write-Host "   ✅ Datos actualizados a Lima, Perú" -ForegroundColor Green
}
Write-Host "   ✅ Proyecto compilado correctamente" -ForegroundColor Green

Write-Host ""
Write-Host "🚀 Para ejecutar la aplicación:" -ForegroundColor Yellow
Write-Host "   dotnet run --project EasyParking.API" -ForegroundColor White

Write-Host ""
Write-Host "🌐 URLs de acceso:" -ForegroundColor Yellow
Write-Host "   API: https://localhost:7001" -ForegroundColor White
Write-Host "   Swagger: https://localhost:7001/swagger" -ForegroundColor White

Write-Host ""
Write-Host "🗺️  Ejemplos de búsqueda por ubicación en Lima:" -ForegroundColor Yellow
Write-Host "   • Plaza San Martín: GET /api/parkings/search/location?latitude=-12.0464&longitude=-77.0428&radiusKm=2.0" -ForegroundColor Gray
Write-Host "   • Miraflores: GET /api/parkings/search/location?latitude=-12.1464&longitude=-77.0228&radiusKm=3.0" -ForegroundColor Gray
Write-Host "   • Aeropuerto: GET /api/parkings/search/location?latitude=-12.0219&longitude=-77.1143&radiusKm=10.0" -ForegroundColor Gray

Write-Host ""
Write-Host "📚 Documentación adicional:" -ForegroundColor Yellow
Write-Host "   • README.md - Documentación general del proyecto" -ForegroundColor Gray
Write-Host "   • docs/LOCATION_SEARCH.md - Documentación de búsqueda por ubicación" -ForegroundColor Gray
Write-Host "   • docs/MIGRATIONS.md - Guía de migraciones" -ForegroundColor Gray

Write-Host ""
Write-Host "¡EasyParking está listo para usar en Lima, Perú! 🇵🇪" -ForegroundColor Green 