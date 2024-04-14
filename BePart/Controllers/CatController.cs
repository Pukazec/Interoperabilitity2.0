using BePart.ActiveMQ;
using BePart.Data;
using Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BePart.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CatController : ControllerBase
    {
        private readonly ICatService _catService;

        public CatController(ICatService catService)
        {
            _catService = catService;
        }

        [HttpGet]
        public IActionResult GetAllCats()
        {
            var cats = _catService.GetAllCats();
            var producer = new ActiveMQProducer("get all cats");
            producer.SendMessage(JsonSerializer.Serialize(cats).ToString());
            return Ok(cats);
        }

        [HttpGet("{id}")]
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
