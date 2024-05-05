using Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BePart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SerializationController(CatDbContext context) : Controller
    {
        [HttpGet("Serialize")]
        public IActionResult SerializeAllCats()
        {
            var cats = context.Cats.OrderBy(c => EF.Functions.Random()).First();
            try
            {
                new SerializationService().SerializeCats(cats);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Deserialize")]
        public IActionResult DeserializeCats()
        {
            try
            {
                return Ok(new SerializationService().DeserializeCats());
            }
            catch
            {
                return BadRequest();
            }
        }
    }

    public class SerializationService
    {
        const string fileName = "testic.dat";

        public void SerializeCats(Cat cat)
        {
            using var stream = File.Open(fileName, FileMode.Create);
            using var binaryWriter = new BinaryWriter(stream, Encoding.UTF8, false);

            binaryWriter.Write(cat.Id);
            binaryWriter.Write(cat.Age);
            binaryWriter.Write(cat.CatName);
            binaryWriter.Write(cat.Color);
            binaryWriter.Write(cat.Summary ?? "");
        }


        public Cat DeserializeCats()
        {
            using var stream = File.Open(fileName, FileMode.Open);
            using var binaryReader = new BinaryReader(stream, Encoding.UTF8, false);

            if (!DeserializationValidator.IsAllowedType(typeof(Cat)))
            {
                throw new Exception("Invalid data type.");
            }

            var catId = binaryReader.ReadInt32();
            var catAge = binaryReader.ReadDouble();
            var catName = binaryReader.ReadString();
            var catColor = binaryReader.ReadString();
            var catSummary = binaryReader.ReadString();

            if (!DeserializationValidator.IsAllowedType(catId.GetType()) ||
                !DeserializationValidator.IsAllowedType(catAge.GetType()) ||
                !DeserializationValidator.IsAllowedType(catName.GetType()) ||
                !DeserializationValidator.IsAllowedType(catColor.GetType()) ||
                !DeserializationValidator.IsAllowedType(catSummary.GetType()))
            {
                throw new Exception("Invalid data type.");
            }

            return new Cat
            {
                Id = catId,
                Age = catAge,
                CatName = catName,
                Color = catColor,
                Summary = catSummary,
            };
        }
    }

    public static class DeserializationValidator
    {
        private static readonly HashSet<Type> _allowedTypes = new HashSet<Type>
        {
            typeof(Cat),
            typeof(int),
            typeof(string),
            typeof(double),
            typeof(float),
            typeof(bool),
        };

        public static bool IsAllowedType(Type type)
        {
            return _allowedTypes.Contains(type);
        }
    }
}
