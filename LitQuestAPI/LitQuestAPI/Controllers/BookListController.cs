﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LitQuestAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LitQuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooklistController : ControllerBase
    {

        private readonly LitquestContext _context;
        public BooklistController(LitquestContext context)
        {
            _context = context;
        }

        // GET: api/Booklist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booklist>>> GetBooklist()
        {
            return await _context.Booklists.ToListAsync();

        }

        // GET api/Booklist/1/MyList1
        [HttpGet("{userid}/{listname}")]
        public async Task<ActionResult<IEnumerable<Booklist>>> GetBooklist(int userid, string listname)
        {
            var Booklist = await _context.Booklists.Where(s => s.Listname == listname && s.Userid == userid).ToListAsync();

            if (Booklist == null)
            {
                return NotFound();
            }

            return Booklist;
        }

        // POST api/Booklist
        [HttpPost]
        public async Task<ActionResult<Booklist>> PostBooklist(Booklist Booklist)
        {
            _context.Booklists.Add(Booklist);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BooklistExists(Booklist.Listname, Booklist.Userid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBooklist", new { listname = Booklist.Listname }, Booklist);
        }

        // DELETE api/Booklist/5/MyList1
        [HttpDelete("{userid}/{listname}")]
        public async Task<IActionResult> DeleteBooklist(int userid, string listname)
        {
            var Booklist = await _context.Booklists.FindAsync(userid, listname);
            if (Booklist == null)
            {
                return NotFound();
            }

            _context.Booklists.Remove(Booklist);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BooklistExists(string listname, int userid)
        {
            return _context.Booklists.Any(e => e.Listname == listname && e.Userid == userid);
        }
    }
}
