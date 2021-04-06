using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestProgrammationConformit.Infrastructures;
using TestProgrammationConformit.Filter;
using TestProgrammationConformit.Wrappers;
using TestProgrammationConformit.Helpers;
using TestProgrammationConformit.Services;

namespace TestProgrammationConformit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ConformitContext _context;
        private readonly IUriService _uriService;

        public EventsController(ConformitContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> Getevents([FromQuery] PaginationFilter filter)
        {

            var route = Request.Path.Value;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Events
                
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Include(e => e.Comments)
                .ToListAsync();
            var totalRecords = await _context.Events.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Event>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);

        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.Include(e => e.Comments).FirstOrDefaultAsync(e=> e.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }
            return @event;
        }

        // PUT: api/Events/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest();
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // POST: api/Events
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Event>> DeleteEvent(int id)
        {
            string message = "ok";
            try
            {
                var @event = await _context.Events.FindAsync(id);
                if (@event == null)
                {
                    return NotFound();
                }

                _context.Events.Remove(@event);
                await _context.SaveChangesAsync();

            }  catch (DbUpdateConcurrencyException) {
                message = "error";
            }
            return Content(message);
        }

        // POST: api/Events/save-in
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("save-in")]
        public async Task<ActionResult<Event>> PostEventsList(List<Event> @events)
        {
            string message = "ok";
            try
            {
                _context.Events.AddRange(@events);
                await _context.SaveChangesAsync();

            }  catch (DbUpdateConcurrencyException)
            {
                message = "error";
            }
            return Content(message);

        }

        // PUT: api/Events/update-in
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update-in")]
        public async Task<ActionResult<Event>> PutEventsList(List<Event> @events)
        {
            string  message = "array empty";
            try
            {
                IList<Event> eventsToUpdate = new List<Event>();
                foreach (Event @event   in @events)
                {
                    _context.Entry(@event).State = EntityState.Modified;
                    var element = await _context.Events.FindAsync(@event.EventId);
                    if (element != null)
                    {
                        eventsToUpdate.Add(@event);
                    }

                }
                if(eventsToUpdate.Count > 0)
                {
                    message = "ok";
                    _context.Events.UpdateRange(eventsToUpdate);
                    await _context.SaveChangesAsync();
                }
                
            }
            catch (DbUpdateConcurrencyException) {
                message = "error";
            }
            return Content(message);

        }

        // DELETE: api/Events/delete-in/?id=1&id=2
        [HttpDelete("delete-in/")]
        public async Task<ActionResult<Event>> deleteIn([FromQuery] int[] ids)
        {
            string message = "ok";
            try
            {

                IList<Event> eventsToRemove = new List<Event>();

                foreach (int id in ids)
                {
                    var @event = await _context.Events.FindAsync(id);
                    if (@event != null)
                    {
                        eventsToRemove.Add(@event);
                    }
                }

                _context.Events.RemoveRange(eventsToRemove);
                await _context.SaveChangesAsync();

             } catch (DbUpdateConcurrencyException) {
                    message = "error";
            }
            return Content(message);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
