using Bogus;
using Domain.Models;
using Domain.Models.Event;
using Domain.Models.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class Seeder
    {
        private const int RecordsAmount = 15;
        private readonly BazArtDbContext _dbContext;
        private readonly List<Category> _categories;

        public Seeder(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
            _categories = GetCategories();
        }

        public async Task SeedAsync()
        {
            await _dbContext.Database.MigrateAsync();

            if (_dbContext.Categories.Any()) return;
            
            var users = GetUsers();
            var events = GetEvents();
            var products = GetProducts();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].CreatedEvents.Add(events[i]);
                users[i].OwnedProducts.Add(products[i]);
            }

            await _dbContext.AddRangeAsync(_categories);
            await _dbContext.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();
        }

        private List<Category> GetCategories()
        {
            var categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Painting",
                    Description = "Painting is a visual art form where pigments or colors are applied to a surface, typically a canvas or paper, using various techniques to create images, convey emotions, or express ideas.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e9/Soleil_levant_Claude_Monet.jpg"
                },
                new Category()
                {
                    Name = "Sculpture",
                    Description = "Sculpture is a three-dimensional art form in which artists manipulate materials like stone, wood, metal, or clay to create physical, often sculptural, representations of objects, people, or ideas.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/b/bf/6829_-_Claudio_%28Museo_Pio-Clementino%29_-_Foto_Giovanni_Dall%27Orto%2C_10_june_2011.jpg"
                },
                new Category()
                {
                    Name = "Photography",
                    Description = "Photography is the art and technology of capturing and recording images using light-sensitive materials or digital sensors, allowing for the creation of still or moving visual representations of scenes, subjects, and moments.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/e/e4/Stourhead_garden.jpg"
                },
                new Category()
                {
                    Name = "Hand Made",
                    Description = "Handmade art refers to creative works produced by skilled individuals using manual techniques and traditional craftsmanship, typically without the aid of automated or mass-production processes. It emphasizes the personal touch and unique qualities of each piece.",
                    ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/0/07/Fish1.jpg"
                }
            };

            return categories;
        }

        private List<User> GetUsers()
        {
            var userAddressFaker = new Faker<UserAddress>()
                .RuleFor(p => p.Country, f => f.Address.Country())
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.Street, f => f.Address.StreetName())
                .RuleFor(p => p.HouseNumber, f => f.Random.Int(1, 200))
                .RuleFor(p => p.PostalCode, f => f.Address.ZipCode());

            var usersAddresses = userAddressFaker.Generate(RecordsAmount);

            var userFaker = new Faker<User>()
                .RuleFor(p => p.StageName, f => f.Name.FindName())
                .RuleFor(p => p.Description, f => f.Name.JobDescriptor())
                .RuleFor(p => p.Category, f => f.PickRandom(_categories));

            var users = userFaker.Generate(RecordsAmount);

            for (int i = 0; i < users.Count; i++)
            {
                users[i].Address = usersAddresses[i];
            }

            return users;
        }

        private List<Event> GetEvents()
        {
            var eventDetailFaker = new Faker<EventDetail>()
                .RuleFor(p => p.Country, f => f.Address.Country())
                .RuleFor(p => p.City, f => f.Address.City())
                .RuleFor(p => p.Street, f => f.Address.StreetName())
                .RuleFor(p => p.HouseNumber, f => f.Random.Int(1, 200))
                .RuleFor(p => p.PostalCode, f => f.Address.ZipCode())
                .RuleFor(p => p.StartingDate, f => f.Date.Soon(10))
                .RuleFor(p => p.EndingDate, f => f.Date.Soon(20, DateTime.Today).AddDays(10));

            var eventsDetail = eventDetailFaker.Generate(RecordsAmount);


            var eventsFaker = new Faker<Event>()
                .RuleFor(p => p.Name, f => f.Name.JobTitle())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Category, f => f.PickRandom(_categories));

            var events = eventsFaker.Generate(RecordsAmount);

            for (int i = 0; i < events.Count; i++)
            {
                events[i].EventDetail = eventsDetail[i];
            }

            return events;
        }

        private List<Product> GetProducts()
        {
            var productFaker = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Lorem.Text())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                .RuleFor(p => p.IsForSell, f => f.Random.Bool(0.7F))
                .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Category, f => f.PickRandom(_categories));

            var products = productFaker.Generate(RecordsAmount);

            return products;
        }
    }
}
