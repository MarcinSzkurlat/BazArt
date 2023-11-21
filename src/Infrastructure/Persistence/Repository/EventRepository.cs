using Application.Interfaces;
using Domain.Models.Event;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly BazArtDbContext _dbContext;

        public EventRepository(BazArtDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Event?> GetEventByIdAsync(Guid id)
        {
            return await _dbContext.Events
                .Include(e => e.Category)
                .Include(e => e.CreatedBy)
                .Include(e => e.EventDetail)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        
        public async Task<IEnumerable<Event>> GetEventsByCategoryAsync(string categoryName, int pageNumber, int pageSize)
        {
            return await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Category)
                .Where(e => e.Category.Name.ToLower() == categoryName.ToLower())
                .OrderBy(x => x.EventDetail.Created)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task CreateEventAsync(Event eventToCreate)
        {
            eventToCreate.Category = await _dbContext.Categories.FindAsync(eventToCreate.CategoryId);
            eventToCreate.CreatedBy = await _dbContext.Users.FindAsync(eventToCreate.CreatedById);

            await _dbContext.Events.AddAsync(eventToCreate);
        }

        public void DeleteEvent(Event eventToDelete)
        {
            _dbContext.Events.Remove(eventToDelete);
        }

        public async Task<IEnumerable<Event>> GetEventsByCreatedDate(int amount)
        {
            return await _dbContext.Events
                .AsNoTracking()
                .Include(x => x.Category)
                .OrderByDescending(x => x.EventDetail.Created)
                .Take(amount)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid id, int pageNumber, int pageSize)
        {
            return await _dbContext.Events
                .AsNoTracking()
                .Include(x => x.Category)
                .Where(x => x.CreatedById == id)
                .OrderBy(x => x.EventDetail.Created)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetEventsBySearchQueryAsync(string searchQuery)
        {
            return await _dbContext.Events
                .AsNoTracking()
                .Where(x => x.Name.ToUpper().Contains(searchQuery.ToUpper()) ||
                            x.Description.ToUpper().Contains(searchQuery.ToUpper()))
                .ToListAsync();
        }

        public async Task<int> GetEventsQuantityByCategoryAsync(string categoryName)
        {
            return await _dbContext.Events
                .CountAsync(x => x.Category.Name.ToUpper() == categoryName.ToUpper());
        }

        public async Task<int> GetEventsQuantityByUserIdAsync(Guid id)
        {
            return await _dbContext.Events
                .CountAsync(x => x.CreatedById == id);
        }
    }
}
