using BePart.ActiveMQ;
using BePart.Data;
using Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BePart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
    }

    public class CatController(ICatService catService, CatDbContext context) : ApiControllerBase
    {
        private readonly ICatService _catService = catService;
        private readonly CatDbContext _context = context;

        [HttpGet]
        public IActionResult GetAllCats()
        {
            var cats = _catService.GetAllCats();
            var producer = new ActiveMQProducer("cats gotten");
            producer.SendMessage(JsonSerializer.Serialize(cats).ToString());
            return Ok(cats);
        }

        [HttpGet("{id}/id")]
        public IActionResult GetCatById(int id)
        {
            var cat = _catService.GetCatById(id);
            if (cat == null)
            {
                return NotFound();
            }
            var producer = new ActiveMQProducer("get one cat");
            producer.SendMessage(JsonSerializer.Serialize(cat).ToString());
            return Ok(cat);
        }

        [HttpGet("{name}")]
        public IActionResult GetCatByName(string name)
        {
            string query = "SELECT * FROM CAT WHERE CATNAME = '" + name + "'";
            var cat = _context.Database.ExecuteSqlRaw(query);

            return Ok(cat);
        }

        [HttpPost]
        public IActionResult AddCat([FromBody] Cat cat)
        {
            _catService.AddCat(cat);
            return CreatedAtAction(nameof(GetCatById), new { id = cat.Id }, cat);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCat(int id, [FromBody] Cat cat)
        {
            if (id != cat.Id)
            {
                return BadRequest();
            }

            var existingCat = _catService.GetCatById(id);
            if (existingCat == null)
            {
                return NotFound();
            }

            _catService.UpdateCat(cat);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCat(int id)
        {
            var cat = _catService.GetCatById(id);
            if (cat == null)
            {
                return NotFound();
            }

            _catService.DeleteCat(id);
            return NoContent();
        }
    }
}
