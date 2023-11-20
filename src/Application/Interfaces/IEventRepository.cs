using Domain.Models.Event;

namespace Application.Interfaces
{
    public interface IEventRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<Event?> GetEventByIdAsync(Guid id);
        public Task<List<Event>> GetEventsByCategoryAsync(string categoryName);
        public Task CreateEventAsync(Event eventToCreate);
        public void DeleteEvent(Event eventToDelete);
        public Task<IEnumerable<Event>> GetEventsByCreatedDate(int amount);
        public Task<IEnumerable<Event>> GetEventsByUserIdAsync(Guid id);
        public Task<IEnumerable<Event>> GetEventsBySearchQueryAsync(string searchQuery);
    }
}
