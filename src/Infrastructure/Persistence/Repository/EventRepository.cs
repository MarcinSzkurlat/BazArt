﻿using Application.Interfaces;
using Domain.Models;
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

        public async Task<Event>? GetEventByIdAsync(Guid id)
        {
            return await _dbContext.Events
                .Include(e => e.Category)
                .Include(e => e.CreatedBy)
                .Include(e => e.EventDetail)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        
        public async Task<List<Event>> GetEventsByCategoryAsync(Categories categories)
        {
            return await _dbContext.Events
                .AsNoTracking()
                .Include(e => e.Category)
                .Where(e => e.Category.Name == categories.ToString())
                .ToListAsync();
        }

        public async Task CreateEventAsync(Event eventToCreate)
        {
            await _dbContext.Events.AddAsync(eventToCreate);
        }

        public void DeleteEvent(Event eventToDelete)
        {
            _dbContext.Events.Remove(eventToDelete);
        }
    }
}