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

        private void LoadSatellites(CelestialObject celestialObject)
        {
            celestialObject.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == celestialObject.Id).ToList();
        }
    }
}
