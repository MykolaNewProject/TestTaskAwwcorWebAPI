using Microsoft.EntityFrameworkCore;


namespace TestTaskAwwcor
{

    // Контекст для работы с БД
    public class AppDbContext: DbContext
    {
        public DbSet<Ad> Ads => Set<Ad>();

        public AppDbContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Ads.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ad>().HasData(
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Car", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Bike", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Boat", Price = 225000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Moto", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Fleet", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "VW", Price = 1550000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "BMW", Price = 1700000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Audi", Price = 1800000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Lada", Price = 380000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "фффф", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Bммм", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "ыпява", Price = 225000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "ячрывo", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Fнывко", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Vсчты", Price = 1550000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Bичаны", Price = 1700000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "фунрчи", Price = 1800000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "ярыкен", Price = 380000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Car", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Bike", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Boat", Price = 225000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Moto", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Fleet", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "VW", Price = 1550000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "BMW", Price = 1700000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Audi", Price = 1800000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Lada", Price = 380000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "фффф", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Bммм", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "ыпява", Price = 225000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "ячрывo", Price = 750000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Fнывко", Price = 360000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Vсчты", Price = 1550000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "Bичаны", Price = 1700000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "фунрчи", Price = 1800000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now },
                new Ad { Id = Guid.NewGuid().ToString(), Title = "ярыкен", Price = 380000, Description = "1gdn4ydjvydnb4hdykgnsr", DateOfCreation = DateTime.Now }
            );
        }
    }
}
