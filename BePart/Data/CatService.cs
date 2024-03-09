using Dtos;

namespace BePart.Data
{
    public interface ICatService
    {
        List<Cat> GetAllCats();
        Cat GetCatById(int id);
        void AddCat(Cat cat);
        void UpdateCat(Cat cat);
        void DeleteCat(int id);
    }

    public class CatService : ICatService
    {
        private readonly List<Cat> _cats = new List<Cat>();

        public CatService()
        {
            _cats.Add(new Cat { Id = 1, Age = 3.5, Color = "Black", Name = "Shadow", Summary = "A stealthy little shadow that loves to sneak around." });
            _cats.Add(new Cat { Id = 2, Age = 2, Color = "White", Name = "Snowball", Summary = "Fluffy and white as the fresh winter snow." });
            _cats.Add(new Cat { Id = 3, Age = 4, Color = "Ginger", Name = "Tiger", Summary = "Loves to play and chase lasers." });
            _cats.Add(new Cat { Id = 4, Age = 1.5, Color = "Grey", Name = "Smoky", Summary = "A curious explorer with a love for high places." });
            _cats.Add(new Cat { Id = 5, Age = 7, Color = "Calico", Name = "Patch", Summary = "A wise old cat with a patchy coat and gentle eyes." });
            _cats.Add(new Cat { Id = 6, Age = 0.8, Color = "Tabby", Name = "Biscuit", Summary = "A playful kitten with endless energy and a penchant for mischief." });
            _cats.Add(new Cat { Id = 7, Age = 3, Color = "Siamese", Name = "Luna", Summary = "Elegant and poised, with a mysterious aura and a melodious voice." });
            _cats.Add(new Cat { Id = 8, Age = 2.5, Color = "Black and White", Name = "Oreo", Summary = "Sweet as a cookie, with a personality that's just as delightful." });
            _cats.Add(new Cat { Id = 9, Age = 4.5, Color = "Orange", Name = "Simba", Summary = "Has the heart of a lion and enjoys basking in the sun." });
            _cats.Add(new Cat { Id = 10, Age = 6, Color = "Blue", Name = "Sky", Summary = "A serene and peaceful presence, with a coat as soft as clouds." });
        }

        public List<Cat> GetAllCats() => _cats;

        public Cat GetCatById(int id) => _cats.SingleOrDefault(c => c.Id == id);

        public void AddCat(Cat cat)
        {
            cat.Id = _cats.Last().Id++;
            _cats.Add(cat);
        }

        public void UpdateCat(Cat cat)
        {
            var index = _cats.FindIndex(c => c.Id == cat.Id);
            if (index != -1)
            {
                _cats[index] = cat;
            }
        }

        public void DeleteCat(int id)
        {
            _cats.RemoveAll(c => c.Id == id);
        }
    }
}
