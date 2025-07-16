using Microsoft.AspNetCore.Mvc;
using EasyParking.Application.Interfaces;
using EasyParking.Application.DTOs;
using EasyParking.Core.Exceptions;

namespace EasyParking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations([FromQuery] ReservationSearchDto? searchDto)
        {
            try
            {
                IEnumerable<ReservationDto> reservations;
                
                if (searchDto != null)
                {
                    reservations = await _reservationService.SearchReservationsAsync(searchDto);
                }
                else
                {
                    reservations = await _reservationService.GetAllReservationsAsync();
                }

                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservation(int id)
        {
            try
            {
                var reservation = await _reservationService.GetReservationByIdAsync(id);
                if (reservation == null)
                {
                    return NotFound(new { message = "Reserva no encontrada" });
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsByUser(int userId)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsByUserIdAsync(userId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("parking/{parkingId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsByParking(int parkingId)
        {
            try
            {
                var reservations = await _reservationService.GetReservationsByParkingIdAsync(parkingId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetActiveReservations()
        {
            try
            {
                var reservations = await _reservationService.GetActiveReservationsAsync();
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(CreateReservationDto createReservationDto)
        {
            try
            {
                var reservation = await _reservationService.CreateReservationAsync(createReservationDto);
                return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, UpdateReservationDto updateReservationDto)
        {
            try
            {
                var reservation = await _reservationService.UpdateReservationAsync(id, updateReservationDto);
                return Ok(reservation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DomainException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                await _reservationService.DeleteReservationAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("{id}/confirm")]
        public async Task<ActionResult<ReservationDto>> ConfirmReservation(int id)
        {
            try
            {
                var reservation = await _reservationService.ConfirmReservationAsync(id);
                return Ok(reservation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<ReservationDto>> CancelReservation(int id)
        {
            try
            {
                var reservation = await _reservationService.CancelReservationAsync(id);
                return Ok(reservation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("{id}/checkin")]
        public async Task<ActionResult<ReservationDto>> CheckInReservation(int id)
        {
            try
            {
                var reservation = await _reservationService.CheckInReservationAsync(id);
                return Ok(reservation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpPost("{id}/checkout")]
        public async Task<ActionResult<ReservationDto>> CheckOutReservation(int id)
        {
            try
            {
                var reservation = await _reservationService.CheckOutReservationAsync(id);
                return Ok(reservation);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("exists/{id}")]
        public async Task<ActionResult<bool>> ReservationExists(int id)
        {
            try
            {
                var exists = await _reservationService.ReservationExistsAsync(id);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        [HttpGet("conflict-check")]
        public async Task<ActionResult<bool>> HasConflictingReservation(
            [FromQuery] int parkingSpaceId, 
            [FromQuery] DateTime startTime, 
            [FromQuery] DateTime endTime)
        {
            try
            {
                var hasConflict = await _reservationService.HasConflictingReservationAsync(parkingSpaceId, startTime, endTime);
                return Ok(hasConflict);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
} 