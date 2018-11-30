using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var item = _context.CelestialObjects.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Satellites = _context.CelestialObjects.Where(a => a.OrbitedObjectId == item.Id).ToList();

            return Ok(item);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var items = _context.CelestialObjects.Where(a => a.Name == name).ToList();

            if (items.Count == 0)
            {
                return NotFound();
            }
            foreach (var item in items)
            {
                item.Satellites = _context.CelestialObjects.Where(a => a.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(items);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var items = _context.CelestialObjects.ToList();

            foreach (var item in items)
            {
                item.Satellites = _context.CelestialObjects.Where(a => a.OrbitedObjectId == item.Id).ToList();
            }

            return Ok(items);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)

        {
            var item = _context.CelestialObjects.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = celestialObject.Name;
            item.OrbitalPeriod = celestialObject.OrbitalPeriod;
            item.OrbitedObjectId = celestialObject.OrbitedObjectId;


            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var item = _context.CelestialObjects.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = name;
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var items = _context.CelestialObjects.Where(a => a.Id == id && a.OrbitedObjectId == id);

            if (!items.Any())
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(items);
            _context.SaveChanges();

            return NoContent();
        }

    }


}
