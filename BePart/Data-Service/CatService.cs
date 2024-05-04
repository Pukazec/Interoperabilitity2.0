using Dtos;

namespace BePart.Data
{
    public interface ICatService
    {
        List<Cat> GetAllCats();
        Cat GetCatById(int id);
        Cat GetCatByName(string name);
        IList<Cat> OlderThan(int age);
        void AddCat(Cat cat);
        void UpdateCat(Cat cat);
        void DeleteCat(int id);
    }

    public class CatService : ICatService
    {
        private readonly CatDbContext _context;

        public CatService(CatDbContext context)
        {
            _context = context;
        }

        public List<Cat> GetAllCats() => _context.Cats.ToList();

        public Cat GetCatById(int id) => _context.Cats.SingleOrDefault(c => c.Id == id);

        public Cat GetCatByName(string name) => _context.Cats.Single(c => c.CatName == name);

        public void AddCat(Cat cat)
        {
            _context.Cats.Add(cat);
            _context.SaveChanges();
        }

        public void UpdateCat(Cat cat)
        {
            var existingCat = _context.Cats.Single(c => c.Id == cat.Id);
            existingCat.CatName = cat.CatName;
            existingCat.Color = cat.Color;
            existingCat.Summary = cat.Summary;
            existingCat.Age = cat.Age;
            _context.SaveChanges();
        }

        public void DeleteCat(int id)
        {
            var cat = _context.Cats.Single(x => x.Id == id);
            _context.Cats.Remove(cat);
            _context.SaveChanges();
        }

        public IList<Cat> OlderThan(int age) => _context.Cats.Where(x => x.Age > age).ToList();
    }
}
