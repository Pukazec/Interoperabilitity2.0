using BePart.ActiveMQ;
using BePart.Data;
using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BePart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public abstract class ApiControllerBase : ControllerBase
    {
    }

    public class CatController(ICatService catService, CatDbContext context) : ApiControllerBase
    {
        [HttpGet]
        public IActionResult GetAllCats()
        {
            var cats = catService.GetAllCats();
            var producer = new ActiveMQProducer("cats gotten");
            producer.SendMessage(JsonSerializer.Serialize(cats).ToString());
            return Ok(cats);
        }

        [HttpGet("{id}/id")]
        public IActionResult GetCatById(int id)
        {
            var cat = catService.GetCatById(id);
            if (cat == null)
            {
                //throw new Exception("Cat not found");
                return NotFound();
            }
            var producer = new ActiveMQProducer("get one cat");
            producer.SendMessage(JsonSerializer.Serialize(cat).ToString());
            return Ok(cat);
        }

        [HttpGet("{name}/injection")]
        public IActionResult GetCatByNameInjection(string name)
        {
            // SQL injection detected!!!!!
            string query = "SELECT top 1 * FROM CAT WHERE CATNAME LIKE '%" + name + "%'";
            var cat = context.Database.SqlQueryRaw<Cat>(query).SingleOrDefault();

            return Ok(cat);
        }

        [HttpPost]
        public IActionResult AddCat([FromBody] Cat cat)
        {
            catService.AddCat(cat);
            return CreatedAtAction(nameof(GetCatById), new { id = cat.Id }, cat);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCat(int id, [FromBody] Cat cat)
        {
            if (id != cat.Id)
            {
                cat.Id = id;
            }

            var existingCat = catService.GetCatById(id);
            if (existingCat == null)
            {
                return NotFound();
            }

            cat.Id = id;
            catService.UpdateCat(cat);
            return NoContent();
        }

        [HttpGet("{name}")]
        public IActionResult GetCatByName(string name)
        {
            var cat = context.Cats.FirstOrDefault(x => x.CatName.Contains(name));

            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCat(int id)
        {
            var cat = catService.GetCatById(id);
            if (cat == null)
            {
                return NotFound();
            }

            catService.DeleteCat(id);
            return Ok(id);
        }
    }
}
