using Microsoft.EntityFrameworkCore;
using EasyParking.Core.Interfaces;
using EasyParking.Core.Entities;
using EasyParking.Infrastructure.Data;

namespace EasyParking.Infrastructure.Repositories
{
    public class ParkingRepository : BaseRepository<Parking>, IParkingRepository
    {
        public ParkingRepository(EasyParkingDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Parking>> GetByCityAsync(string city)
        {
            return await _dbSet.Where(p => p.City == city && p.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Parking>> GetByStatusAsync(ParkingStatus status)
        {
            return await _dbSet.Where(p => p.Status == status && p.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Parking>> GetAvailableParkingsAsync()
        {
            return await _dbSet.Where(p => p.Status == ParkingStatus.Active && p.AvailableSpaces > 0 && p.IsActive).ToListAsync();
        }

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

        /// <summary>
        /// Búsqueda optimizada por ubicación usando SQL nativo (más eficiente para grandes volúmenes)
        /// </summary>
        public async Task<IEnumerable<Parking>> SearchByLocationOptimizedAsync(decimal latitude, decimal longitude, double radiusKm)
        {
            // Usar SQL nativo para calcular la distancia y filtrar
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
                new Microsoft.Data.SqlClient.SqlParameter("@Latitude", latitude),
                new Microsoft.Data.SqlClient.SqlParameter("@Longitude", longitude),
                new Microsoft.Data.SqlClient.SqlParameter("@RadiusKm", radiusKm)
            };

            return await _context.Parkings
                .FromSqlRaw(sql, parameters)
                .ToListAsync();
        }

        public async Task<Parking?> GetWithSpacesAsync(int id)
        {
            return await _dbSet
                .Include(p => p.ParkingSpaces.Where(ps => ps.IsActive))
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public async Task UpdateAvailableSpacesAsync(int parkingId, int availableSpaces)
        {
            var parking = await GetByIdAsync(parkingId);
            if (parking != null)
            {
                parking.AvailableSpaces = availableSpaces;
                parking.UpdatedAt = DateTime.UtcNow;
                
                _dbSet.Update(parking);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Calcula la distancia entre dos puntos geográficos usando la fórmula de Haversine
        /// </summary>
        /// <param name="lat1">Latitud del primer punto</param>
        /// <param name="lon1">Longitud del primer punto</param>
        /// <param name="lat2">Latitud del segundo punto</param>
        /// <param name="lon2">Longitud del segundo punto</param>
        /// <returns>Distancia en kilómetros</returns>
        private static double CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const double earthRadiusKm = 6371.0; // Radio de la Tierra en kilómetros

            // Convertir decimal a double para los cálculos
            double lat1Rad = Convert.ToDouble(lat1) * Math.PI / 180.0;
            double lon1Rad = Convert.ToDouble(lon1) * Math.PI / 180.0;
            double lat2Rad = Convert.ToDouble(lat2) * Math.PI / 180.0;
            double lon2Rad = Convert.ToDouble(lon2) * Math.PI / 180.0;

            // Diferencia de coordenadas
            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            // Fórmula de Haversine
            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                      Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                      Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distancia en kilómetros
            return earthRadiusKm * c;
        }
    }
} 