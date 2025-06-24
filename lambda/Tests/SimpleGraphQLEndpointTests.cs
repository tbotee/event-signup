using EventSignup.Data;
using EventSignup.GqlTypes;
using EventSignup.Models;
using EventSignup.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace EventSignup.Tests
{
    public class SimpleGraphQLEndpointTests : IAsyncDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly EventSignupContext _dbContext;
        private readonly EventTypeQuery _eventQuery;

        public SimpleGraphQLEndpointTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<EventSignupContext>(options =>
                options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

            services.AddLogging(builder => builder.AddConsole());

    
            services.AddScoped<IDatabaseService, DatabaseService>();


            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<EventSignupContext>();
            _eventQuery = new EventTypeQuery();
            SeedDummyData().Wait();
        }

        private async Task SeedDummyData()
        {
            var dummyEvents = new List<Event>
            {
                new Event { Id = 1, Name = "Lego life event", Date = DateTime.Now.AddDays(7), MaxAttendees = 200 },
            };

            _dbContext.Events.AddRange(dummyEvents);
            await _dbContext.SaveChangesAsync();
        }

        
        [Fact]
        public async Task ListEvents_ShouldReturnAllEvents()
        {
            var databaseService = _serviceProvider.GetRequiredService<IDatabaseService>();

            var events = await _eventQuery.ListEvents(databaseService);

            Assert.NotNull(events);
            Assert.Single(events);

            var eventList = events.ToList();
            
            Assert.Contains(eventList, e => e.Name == "Lego life event");
        }

        [Fact]
        public async Task ListEvents_ShouldUpdateEvent()
        {
            var databaseService = _serviceProvider.GetRequiredService<IDatabaseService>();

            var eventToUpdate = await databaseService.GetEventByIdAsync(1);

            eventToUpdate!.Name = "Lego live event updated";

            var updatedEvent = await databaseService.UpdateEventAsync(eventToUpdate);

            Assert.Equal("Lego live event updated", updatedEvent.Name);
        }

        public async ValueTask DisposeAsync()
        {
            if (_dbContext != null)
            {
                _dbContext.Database.EnsureDeleted();
                await _dbContext.DisposeAsync();
            }

            if (_serviceProvider != null)
            {
                _serviceProvider.Dispose();
            }
        }
    }
} 