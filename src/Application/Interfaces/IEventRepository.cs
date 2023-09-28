using Domain.Models;
using Domain.Models.Event;

namespace Application.Interfaces
{
    public interface IEventRepository
    {
        public Task<int> SaveChangesAsync();
        public Task<Domain.Models.Event.Event?> GetEventByIdAsync(Guid id);
        public Task<List<Domain.Models.Event.Event>> GetEventsByCategoryAsync(Categories categories);
        public Task CreateEventAsync(Domain.Models.Event.Event eventToCreate);
        public void DeleteEvent(Domain.Models.Event.Event eventToDelete);
        public Task<IEnumerable<Event>> GetEventsByCreatedDate(int amount);
    }
}
