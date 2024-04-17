using Dtos;
using Microsoft.EntityFrameworkCore;

namespace BePart
{
    public class CatDbContext : DbContext
    {
        public CatDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Cat> Cats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cat>().HasData(
                new Cat { Id = 1, Age = 3.5, Color = "Black", CatName = "Shadow", Summary = "A stealthy little shadow that loves to sneak around." },
                new Cat { Id = 2, Age = 2, Color = "White", CatName = "Snowball", Summary = "Fluffy and white as the fresh winter snow." },
                new Cat { Id = 3, Age = 4, Color = "Ginger", CatName = "Tiger", Summary = "Loves to play and chase lasers." },
                new Cat { Id = 4, Age = 1.5, Color = "Grey", CatName = "Smoky", Summary = "A curious explorer with a love for high places." },
                new Cat { Id = 5, Age = 7, Color = "Calico", CatName = "Patch", Summary = "A wise old cat with a patchy coat and gentle eyes." },
                new Cat { Id = 6, Age = 0.8, Color = "Tabby", CatName = "Biscuit", Summary = "A playful kitten with endless energy and a penchant for mischief." },
                new Cat { Id = 7, Age = 3, Color = "Siamese", CatName = "Luna", Summary = "Elegant and poised, with a mysterious aura and a melodious voice." },
                new Cat { Id = 8, Age = 2.5, Color = "Black and White", CatName = "Oreo", Summary = "Sweet as a cookie, with a personality that's just as delightful." },
                new Cat { Id = 9, Age = 4.5, Color = "Orange", CatName = "Simba", Summary = "Has the heart of a lion and enjoys basking in the sun." },
                new Cat { Id = 10, Age = 6, Color = "Blue", CatName = "Sky", Summary = "A serene and peaceful presence, with a coat as soft as clouds." }
            );
        }
    }
}
