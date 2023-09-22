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
        private readonly int _minCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Min();
        private readonly int _maxCategoryValue = (int)Enum.GetValues(typeof(Categories)).Cast<Categories>().Max();

        public Seeder(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedAsync()
        {
            await _dbContext.Database.MigrateAsync();

            if (_dbContext.Events.Any()) return;
            
            var users = GetUsers();
            var events = GetEvents();
            var products = GetProducts();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].CreatedEvents.Add(events[i]);
                users[i].OwnedProducts.Add(products[i]);
            }

            await _dbContext.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();
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
                .RuleFor(p => p.CategoryId, f => f.Random.Int(_minCategoryValue, _maxCategoryValue));

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
                .RuleFor(p => p.CategoryId, f => f.Random.Int(_minCategoryValue, _maxCategoryValue));

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
                .RuleFor(p => p.CategoryId, f => f.Random.Int(_minCategoryValue, _maxCategoryValue));

            var products = productFaker.Generate(RecordsAmount);

            return products;
        }
    }
}
