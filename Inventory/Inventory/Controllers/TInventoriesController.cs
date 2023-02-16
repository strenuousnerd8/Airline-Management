using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TInventoriesController : ControllerBase
    {
        private readonly IRPCServer _rpcServer;
        private readonly InventoryDbContext _context;

        public TInventoriesController(InventoryDbContext context, IRPCServer rpcServer)
        {
            _context = context;
            _rpcServer = rpcServer;
        }

        // GET: api/TInventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TInventory>>> GetTInventory()
        {
            return await _context.TInventory.ToListAsync();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<TInventory>>> ResponseService()
        {
            _rpcServer.Consume();
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

        // PUT: api/TInventories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTInventory(int id, TInventory tInventory)
        {
            if (id != tInventory.FlightNumber)
            {
                return BadRequest();
            }

            _context.Entry(tInventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TInventoryExists(id))
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

        // POST: api/TInventories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<TInventory>> PostTInventory(TInventory tInventory)
        {
            _context.TInventory.Add(tInventory);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TInventoryExists(tInventory.FlightNumber))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTInventory", new { id = tInventory.FlightNumber }, tInventory);
        }

        // DELETE: api/TInventories/5
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTInventory(int id)
        {
            var tInventory = await _context.TInventory.FindAsync(id);
            if (tInventory == null)
            {
                return NotFound();
            }

            _context.TInventory.Remove(tInventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TInventoryExists(int id)
        {
            return _context.TInventory.Any(e => e.FlightNumber == id);
        }
    }
}
