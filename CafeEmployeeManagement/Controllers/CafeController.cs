
using CafeEmployeeManagement.Data;
using CafeEmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeEmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CafeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CafeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cafe>>> GetCafes(string location = null)
        {
            IQueryable<Cafe> query = _context.Cafes.Include(c => c.Employees);

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(c => c.Location == location);
            }

            var cafes = await query.ToListAsync();
            var result = cafes.Select(c => new
            {
                c.Name,
                c.Description,
                Employees = c.Employees.Count,
                c.Logo,
                c.Location,
                c.Id
            }).OrderByDescending(c => c.Employees).ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Cafe>> PostCafe(Cafe cafe)
        {
            _context.Cafes.Add(cafe);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCafes), new { id = cafe.Id }, cafe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCafe(Guid id, Cafe cafe)
        {
            if (id != cafe.Id)
            {
                return BadRequest();
            }

            _context.Entry(cafe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CafeExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCafe(Guid id)
        {
            var cafe = await _context.Cafes.Include(c => c.Employees).FirstOrDefaultAsync(c => c.Id == id);
            if (cafe == null)
            {
                return NotFound();
            }

            _context.Cafes.Remove(cafe);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool CafeExists(Guid id)
        {
            return _context.Cafes.Any(e => e.Id == id);
        }
    }
}