using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Booking;

namespace Booking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchFlightController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public SearchFlightController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: api/TInventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TInventory>>> GetTInventory()
        {
            return await _context.TInventory.ToListAsync();
        }

        // GET: api/TInventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TInventory>> GetTInventory(int id)
        {
            var tInventory = await _context.TInventory.FindAsync(id);

            if (tInventory == null)
            {
                return NotFound();
            }

            return tInventory;
        }

        [HttpGet("[action]/{from}/Origin")]
        public async Task<ActionResult<TInventory>> SearchByOrigin(string from)
        {
            var searchFrom = await _context.TInventory.FirstOrDefaultAsync(x => x.LocFrom == from);

            if (from == string.Empty)
            {
                throw new ArgumentNullException(nameof(from));
            }

            return searchFrom;
        }

        [HttpGet("[action]/{to}/Destination")]
        public async Task<ActionResult<TInventory>> SearchByDestination(string to)
        {
            var searchTo = await _context.TInventory.FirstOrDefaultAsync(x => x.LocTo == to);

            if (to == string.Empty)
            {
                throw new ArgumentNullException(nameof(to));
            }

            return searchTo;
        }

        private bool TInventoryExists(int id)
        {
            return _context.TInventory.Any(e => e.FlightNumber == id);
        }
    }
}
