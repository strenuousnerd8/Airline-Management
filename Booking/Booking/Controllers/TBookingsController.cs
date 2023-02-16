using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Booking.RabitMQ;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TBookingsController : ControllerBase
    {
        private readonly BookingDbContext _context;
        private readonly IRabitMQProducer _rabitMQProducer;

        public TBookingsController(BookingDbContext context, IRabitMQProducer rabitMQProducer)
        {
            _context = context;
            _rabitMQProducer = rabitMQProducer;
        }

        // GET: api/TBookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TBooking>>> GetTBooking()
        {
            return await _context.TBooking.ToListAsync();
        }

        // GET: api/TBookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TBooking>> GetTBooking(long id)
        {
            var tBooking = await _context.TBooking.FindAsync(id);

            if (tBooking == null)
            {
                return NotFound();
            }

            string details = tBooking.PassengerDetails;

            _rabitMQProducer.SendProductMessage(details);

            return tBooking;
        }

        [HttpGet("[action]/{seat}")]
        public async Task<ActionResult<TBooking>> GetTBooking(int seat)
        {
            var bookSeat = await _context.TBooking.FirstOrDefaultAsync(x => x.seatNumber == seat);
            if (seat == 0)
            {
                throw new ArgumentNullException(nameof(seat));
            }

            return bookSeat;
        }

        [HttpGet("[action]/email/{email}")]
        public async Task<ActionResult<TBooking>> GetTBooking(string email)
        {
            var bookEmail = await _context.TBooking.FirstOrDefaultAsync(x => x.Email == email);
            if (email == string.Empty)
            {
                throw new ArgumentNullException(nameof(email));
            }
            string details = bookEmail.PassengerDetails;

            _rabitMQProducer.SendProductMessage(details);

            return bookEmail;
        }

        // PUT: api/TBookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTBooking(long id, TBooking tBooking)
        {
            if (id != tBooking.Pnr)
            {
                return BadRequest();
            }

            _context.Entry(tBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TBookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TBookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<TBooking>> PostTBooking(TBooking tBooking)
        {
            _context.TBooking.Add(tBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTBooking", new { id = tBooking.Pnr }, tBooking);
        }

        // DELETE: api/TBookings/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTBooking(long id)
        {
            var tBooking = await _context.TBooking.FindAsync(id);
            if (tBooking == null)
            {
                return NotFound();
            }

            _context.TBooking.Remove(tBooking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TBookingExists(long id)
        {
            return _context.TBooking.Any(e => e.Pnr == id);
        }
    }
}
