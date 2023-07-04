using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Genetics.Server.Dal;
using Genetics.Shared.Models;

namespace Genetics.Server.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalContext _context;

        public AnimalsController(AnimalContext context)
        {
            _context = context;
        }

        [HttpGet("GetAnimal")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimal()
        {
          if (_context.Animal == null)
          {
              return NotFound();
          }
            return await _context.Animal.ToListAsync();
        }

        [HttpGet("GetAnimal/{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
          if (_context.Animal == null)
          {
              return NotFound();
          }
            var animal = await _context.Animal.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return animal;
        }

        [HttpPut("UpdateAnimal/{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, Animal animal)
        {
            if (id != animal.AnimalId)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("AddAnimal")]
        public async Task<ActionResult<Animal>> AddAnimal(Animal animal)
        {
          if (_context.Animal == null)
          {
              return Problem("Entity set 'AnimalContext.Animal'  is null.");
          }
            _context.Animal.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnimal", new { id = animal.AnimalId }, animal);
        }

        [HttpPost]
        [Route("AddAnimalMasive")]
        public async Task<IActionResult> AddAnimalMasive([FromBody]List<Animal> lstAnimal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var animals = new List<Animal>();

                foreach (var item in lstAnimal)
                {
                    var animal = new Animal
                    {
                        Name = item.Name,
                        Breed = item.Breed,
                        BirthDate = item.BirthDate,
                        Sex = item.Sex,
                        Price = item.Price,
                        Status = item.Status
                    };

                    animals.Add(animal);
                }

                _context.Animal.AddRange(animals);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("DeleteAnimal/{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            if (_context.Animal == null)
            {
                return NotFound();
            }
            var animal = await _context.Animal.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animal.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnimalExists(int id)
        {
            return (_context.Animal?.Any(e => e.AnimalId == id)).GetValueOrDefault();
        }
    }
}
