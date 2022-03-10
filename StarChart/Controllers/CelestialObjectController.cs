using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public CelestialObjectController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var result = _context.CelestialObjects.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            LoadSatellites(result);

            return Ok(result);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var result = _context.CelestialObjects.Where(s => s.Name == name).ToList();

            if (result == null || result.Count == 0)
            {
                return NotFound();
            }

            result.ForEach(s =>
            {
                LoadSatellites(s);
            });

            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.CelestialObjects.ToList();

            result.ForEach(s =>
            {
                LoadSatellites(s);
            });

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new
            {
                id = celestialObject.Id
            },
            celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            // Make sure the object already exists.
            var result = _context.CelestialObjects.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            // Object was found, so update the properties.
            result.Name = celestialObject.Name;
            result.OrbitalPeriod = celestialObject.OrbitalPeriod;
            result.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var result = _context.CelestialObjects.Find(id);

            if (result == null)
            {
                return NotFound();
            }

            // Object exists so rename it.
            result.Name = name;

            _context.CelestialObjects.Update(result);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var objsToDelete = _context.CelestialObjects.Where(s => s.Id == id || s.OrbitedObjectId == id).ToList();

            if (objsToDelete == null || objsToDelete.Count == 0)
            {
                return NotFound();
            }

            // Delete the objects that where found with the given id.
            _context.CelestialObjects.RemoveRange(objsToDelete);
            _context.SaveChanges();

            return NoContent();
        }

        private void LoadSatellites(CelestialObject celestialObject)
        {
            celestialObject.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == celestialObject.Id).ToList();
        }
    }
}
