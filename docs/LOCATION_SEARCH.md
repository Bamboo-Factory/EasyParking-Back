# Búsqueda por Ubicación - EasyParking

## 📍 Descripción

EasyParking implementa dos métodos de búsqueda por ubicación para encontrar estacionamientos cercanos a una ubicación específica:

1. **Búsqueda en memoria** (`SearchByLocationAsync`) - Para volúmenes pequeños de datos
2. **Búsqueda optimizada** (`SearchByLocationOptimizedAsync`) - Para grandes volúmenes de datos

## 🧮 Fórmula de Distancia

Ambos métodos utilizan la **fórmula de Haversine** para calcular la distancia entre dos puntos geográficos:

```
d = 2r × arcsin(√(sin²(Δφ/2) + cos(φ₁) × cos(φ₂) × sin²(Δλ/2)))
```

Donde:
- `d` = distancia en kilómetros
- `r` = radio de la Tierra (6,371 km)
- `φ₁, φ₂` = latitudes de los puntos
- `Δφ` = diferencia de latitudes
- `Δλ` = diferencia de longitudes

## 🔗 Endpoints Disponibles

### 1. Búsqueda en Memoria

**Endpoint:** `GET /api/parkings/search/location`

**Parámetros:**
- `latitude` (decimal): Latitud del punto de búsqueda
- `longitude` (decimal): Longitud del punto de búsqueda
- `radiusKm` (double): Radio de búsqueda en kilómetros

**Ejemplo:**
```
GET /api/parkings/search/location?latitude=-12.0464&longitude=-77.0428&radiusKm=5.0
```

### 2. Búsqueda Optimizada

**Endpoint:** `GET /api/parkings/search/location/optimized`

**Parámetros:**
- `latitude` (decimal): Latitud del punto de búsqueda
- `longitude` (decimal): Longitud del punto de búsqueda
- `radiusKm` (double): Radio de búsqueda en kilómetros

**Ejemplo:**
```
GET /api/parkings/search/location/optimized?latitude=-12.0464&longitude=-77.0428&radiusKm=5.0
```

## 📊 Comparación de Métodos

| Aspecto | Búsqueda en Memoria | Búsqueda Optimizada |
|---------|-------------------|-------------------|
| **Rendimiento** | Mejor para < 1,000 registros | Mejor para > 1,000 registros |
| **Precisión** | Alta | Alta |
| **Uso de memoria** | Alto | Bajo |
| **Uso de CPU** | Alto | Bajo |
| **Ordenamiento** | Por distancia | Por distancia |
| **Filtrado** | En memoria | En base de datos |

## 🗺️ Ejemplos de Uso

### Ejemplo 1: Búsqueda en Plaza San Martín (Centro Histórico)

```bash
# Coordenadas de Plaza San Martín, Lima
curl "https://localhost:7001/api/parkings/search/location?latitude=-12.0464&longitude=-77.0428&radiusKm=2.0"
```

**Respuesta esperada:**
```json
[
  {
    "id": 1,
    "name": "Estacionamiento Plaza San Martín",
    "address": "Plaza San Martín, Centro Histórico",
    "city": "Lima",
    "latitude": -12.0464,
    "longitude": -77.0428,
    "distance": 0.0,
    "availableSpaces": 44,
    "hourlyRate": 5.00
  },
  {
    "id": 8,
    "name": "Parqueadero Plaza Mayor",
    "address": "Plaza Mayor de Lima, Centro Histórico",
    "city": "Lima",
    "latitude": -12.0464,
    "longitude": -77.0328,
    "distance": 0.8,
    "availableSpaces": 29,
    "hourlyRate": 4.50
  }
]
```

### Ejemplo 2: Búsqueda en Miraflores (Larcomar)

```bash
# Coordenadas de Larcomar, Miraflores
curl "https://localhost:7001/api/parkings/search/location?latitude=-12.1464&longitude=-77.0228&radiusKm=3.0"
```

### Ejemplo 3: Búsqueda cerca del Aeropuerto Jorge Chávez

```bash
# Coordenadas del Aeropuerto Jorge Chávez
curl "https://localhost:7001/api/parkings/search/location?latitude=-12.0219&longitude=-77.1143&radiusKm=10.0"
```

## 🔧 Implementación Técnica

### Búsqueda en Memoria

```csharp
public async Task<IEnumerable<Parking>> SearchByLocationAsync(decimal latitude, decimal longitude, double radiusKm)
{
    // Obtener todos los estacionamientos activos
    var parkings = await _dbSet
        .Where(p => p.Status == ParkingStatus.Active && p.IsActive)
        .ToListAsync();

    // Filtrar por distancia usando la fórmula de Haversine
    var nearbyParkings = parkings
        .Where(parking => CalculateDistance(latitude, longitude, parking.Latitude, parking.Longitude) <= radiusKm)
        .OrderBy(parking => CalculateDistance(latitude, longitude, parking.Latitude, parking.Longitude))
        .ToList();

    return nearbyParkings;
}
```

### Búsqueda Optimizada

```csharp
public async Task<IEnumerable<Parking>> SearchByLocationOptimizedAsync(decimal latitude, decimal longitude, double radiusKm)
{
    var sql = @"
        SELECT *, 
               (6371 * acos(cos(radians(@Latitude)) * cos(radians(Latitude)) * 
                cos(radians(Longitude) - radians(@Longitude)) + 
                sin(radians(@Latitude)) * sin(radians(Latitude)))) AS Distance
        FROM Parkings 
        WHERE Status = 'Active' 
          AND IsActive = 1
          AND (6371 * acos(cos(radians(@Latitude)) * cos(radians(Latitude)) * 
              cos(radians(Longitude) - radians(@Longitude)) + 
              sin(radians(@Latitude)) * sin(radians(Latitude)))) <= @RadiusKm
        ORDER BY Distance";

    var parameters = new[]
    {
        new SqlParameter("@Latitude", latitude),
        new SqlParameter("@Longitude", longitude),
        new SqlParameter("@RadiusKm", radiusKm)
    };

    return await _context.Parkings
        .FromSqlRaw(sql, parameters)
        .ToListAsync();
}
```

## 📍 Coordenadas de Referencia

### Lima, Perú - Principales Ubicaciones

| Lugar | Latitud | Longitud | Distrito |
|-------|---------|----------|----------|
| Plaza San Martín | -12.0464 | -77.0428 | Centro Histórico |
| Plaza Mayor | -12.0464 | -77.0328 | Centro Histórico |
| Universidad San Marcos | -12.0169 | -77.0428 | San Miguel |
| Hospital Rebagliati | -12.0764 | -77.0428 | Jesús María |
| Aeropuerto Jorge Chávez | -12.0219 | -77.1143 | Callao |
| Plaza San Miguel | -12.0764 | -77.0928 | San Miguel |
| Larcomar | -12.1464 | -77.0228 | Miraflores |
| Jockey Plaza | -12.1964 | -76.9828 | Surco |

### Puntos de Interés - Lima

| Lugar | Latitud | Longitude | Descripción |
|-------|---------|-----------|-------------|
| Centro Histórico | -12.0464 | -77.0428 | Plaza San Martín y alrededores |
| Miraflores | -12.1464 | -77.0228 | Zona turística y comercial |
| San Isidro | -12.0964 | -77.0328 | Distrito financiero |
| Barranco | -12.1464 | -77.0128 | Zona bohemia y cultural |
| Chorrillos | -12.1764 | -77.0028 | Zona residencial |
| San Borja | -12.0864 | -77.0128 | Zona residencial y comercial |

## ⚡ Optimizaciones Futuras

### 1. Índices Espaciales

Para mejorar aún más el rendimiento, se pueden agregar índices espaciales:

```sql
-- Crear índice espacial (requiere SQL Server con soporte espacial)
CREATE SPATIAL INDEX IX_Parkings_Location 
ON Parkings (Location)
USING GEOMETRY_GRID;
```

### 2. Caché de Resultados

Implementar caché para búsquedas frecuentes:

```csharp
// Usar Redis o Memory Cache
var cacheKey = $"parkings_near_{latitude}_{longitude}_{radiusKm}";
var cachedResult = await _cache.GetAsync<IEnumerable<Parking>>(cacheKey);
```

### 3. Paginación

Para grandes volúmenes de resultados:

```csharp
[HttpGet("search/location")]
public async Task<ActionResult<IEnumerable<ParkingDto>>> SearchByLocation(
    [FromQuery] decimal latitude, 
    [FromQuery] decimal longitude, 
    [FromQuery] double radiusKm,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
```

## 🚨 Consideraciones

### Validación de Parámetros

- **Latitud**: Debe estar entre -90 y 90
- **Longitud**: Debe estar entre -180 y 180
- **Radio**: Debe ser mayor a 0 y menor a 50,000 km

### Rendimiento

- **Búsqueda en memoria**: Mejor para aplicaciones pequeñas
- **Búsqueda optimizada**: Mejor para aplicaciones con muchos estacionamientos
- **Considerar índices**: Para mejorar el rendimiento de consultas

### Precisión

- La fórmula de Haversine es precisa para distancias hasta 200 km
- Para distancias mayores, considerar la fórmula de Vincenty
- La precisión depende de la calidad de las coordenadas GPS

## 📞 Soporte

Para problemas con la búsqueda por ubicación:

1. Verificar que las coordenadas estén en el formato correcto
2. Asegurar que el radio sea apropiado para la búsqueda
3. Revisar los logs de la aplicación
4. Contactar al equipo de desarrollo

## 🔗 Enlaces Útiles

- [Entity Framework Core Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [EF Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet/)
- [SQL Server Migration Scripts](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/#generate-a-sql-script) 