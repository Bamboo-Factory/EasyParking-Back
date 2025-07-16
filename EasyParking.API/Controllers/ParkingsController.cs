using Microsoft.AspNetCore.Mvc;
using EasyParking.Application.Interfaces;
using EasyParking.Application.DTOs;
using EasyParking.Core.Exceptions;

namespace EasyParking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingsController : ControllerBase
    {
        private readonly IParkingService _parkingService;

        public ParkingsController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> GetAll()
        {
            try
            {
                var parkings = await _parkingService.GetAllAsync();
                return Ok(parkings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingDto>> GetById(int id)
        {
            try
            {
                var parking = await _parkingService.GetByIdAsync(id);
                if (parking == null)
                    return NotFound($"Estacionamiento con ID {id} no encontrado");

                return Ok(parking);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("city/{city}")]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> GetByCity(string city)
        {
            try
            {
                var parkings = await _parkingService.GetByCityAsync(city);
                return Ok(parkings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> GetAvailable()
        {
            try
            {
                var parkings = await _parkingService.GetAvailableParkingsAsync();
                return Ok(parkings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("search/location")]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> SearchByLocation(
            [FromQuery] decimal latitude, 
            [FromQuery] decimal longitude, 
            [FromQuery] double radiusKm)
        {
            try
            {
                if (radiusKm <= 0)
                    return BadRequest("El radio debe ser mayor a 0");

                var parkings = await _parkingService.SearchByLocationAsync(latitude, longitude, radiusKm);
                return Ok(parkings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("search/location/optimized")]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> SearchByLocationOptimized(
            [FromQuery] decimal latitude, 
            [FromQuery] decimal longitude, 
            [FromQuery] double radiusKm)
        {
            try
            {
                if (radiusKm <= 0)
                    return BadRequest("El radio debe ser mayor a 0");

                var parkings = await _parkingService.SearchByLocationOptimizedAsync(latitude, longitude, radiusKm);
                return Ok(parkings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ParkingDto>> Create(ParkingDto parkingDto)
        {
            try
            {
                var createdParking = await _parkingService.CreateAsync(parkingDto);
                return CreatedAtAction(nameof(GetById), new { id = createdParking.Id }, createdParking);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParkingDto>> Update(int id, ParkingDto parkingDto)
        {
            try
            {
                var updatedParking = await _parkingService.UpdateAsync(id, parkingDto);
                return Ok(updatedParking);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _parkingService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> ParkingExists(int id)
        {
            try
            {
                var exists = await _parkingService.ParkingExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("{id}/available-spaces")]
        public async Task<IActionResult> UpdateAvailableSpaces(int id, [FromBody] int availableSpaces)
        {
            try
            {
                await _parkingService.UpdateAvailableSpacesAsync(id, availableSpaces);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
} 