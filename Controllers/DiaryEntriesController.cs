using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using WebDiaryAPI.Data;
using WebDiaryAPI.Models;

namespace WebDiaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaryEntriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DiaryEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaryEntry>>> GetDiaryEntries()
        {
            return await _context.DiaryEntries.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DiaryEntry>> GetDiaryEntry(int id)
        {
            var entry = await _context.DiaryEntries.FindAsync(id);
            if (entry == null)
                return NotFound();
            else
                return entry;
        }
        [HttpPost]
        public async Task<ActionResult<DiaryEntry>> CreateDiaryEntry(DiaryEntry entry)
        {
            entry.Id = 0;
            _context.DiaryEntries.Add(entry);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDiaryEntry), new { id = entry.Id }, entry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiaryEntry(int id, [FromBody] DiaryEntry entry)
        {
            if (id != entry.Id)
                return BadRequest();

            _context.Entry(entry).State = EntityState.Modified;

            try 
            { 
                await _context.SaveChangesAsync(); 
            } 
            catch(DbUpdateConcurrencyException)
            {
                if (!_context.DiaryEntries.Any(d => d.Id == id))
                    return NotFound();
                else throw;
            }
            
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiaryEntry(int id)
        {
            var entry = await _context.DiaryEntries.FindAsync(id);
            if (entry == null)
                return NotFound();
            _context.DiaryEntries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    
}
