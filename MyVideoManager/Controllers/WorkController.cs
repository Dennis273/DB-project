using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVideoManager.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyVideoManager.Controllers
{
    /// <summary>
    /// The Works API class
    /// </summary>
    [Authorize]
    [Route("[controller]")]
    public class WorkController : Controller
    {
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
        /// <summary>
        /// Get All works.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<Work> GetAll()
        {
            return _context.Works.ToList();
        }

        /// <summary>
        /// Get  a specific work
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
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

        /// <summary>
        /// Create a work
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Update a specific work
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <remarks>Should send the whole work object, or use PATCH</remarks>
        /// <returns></returns>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Work item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var work = _context.Works.FirstOrDefault(t => t.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            
            work.Name = item.Name;
            work.Episode = item.Episode;
             
            _context.Works.Update(item);
            _context.SaveChanges();
            return new NoContentResult();
        }
        /// <summary>
        /// Delete a specific work
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Authorize(Roles = "Administer")]
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var work = _context.Works.FirstOrDefault(t => t.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            _context.Works.Remove(work);
            _context.SaveChanges();
            return new NoContentResult();
        }

    }
}
