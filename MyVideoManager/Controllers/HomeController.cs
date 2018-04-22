using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyVideoManager.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyVideoManager.Controllers
{
    [Route("api/[controller]")]
    public class WorkController : Controller
    {
        // GET: /<controller>/
        private readonly WorkContext _context;
        public WorkController(WorkContext context)
        {
            _context = context;

            if (_context.Works.Count() == 0)
            {
                _context.Works.Add(new Work { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Work> GetAll()
        {
            return _context.Works.ToList();
        }

        [HttpGet("{id}", Name = "GetWork")]
        public IActionResult GetById(long id)
        {
            var item = _context.Works.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Work item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.Works.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetWork", new { id = item.Id }, item);
        }

    }
}
