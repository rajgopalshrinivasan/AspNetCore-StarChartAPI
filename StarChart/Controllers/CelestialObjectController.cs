using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
    }
}
