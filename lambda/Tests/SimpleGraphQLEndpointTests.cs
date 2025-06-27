using EventSignup.Data;
using EventSignup.Data.Entities;
using EventSignup.Services;
using EventSignup.Types;
using GreenDonut;
using GreenDonut.Data;
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

        public SimpleGraphQLEndpointTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<EventSignupContext>(options =>
                options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

            services.AddLogging(builder => builder.AddConsole());

    
            services.AddScoped<IEventService, EventService>();


            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetRequiredService<EventSignupContext>();
            SeedDummyData().Wait();
        }

        private async Task SeedDummyData()
        {
            var dummyEvents = new List<EventEntity>
            {
                new EventEntity { Id = 1, Name = "life event", Date = DateTime.Now.AddDays(7), MaxAttendees = 200 },
            };

            _dbContext.Events.AddRange(dummyEvents);
            await _dbContext.SaveChangesAsync();
        }

        
        [Fact]
        public async Task GetEventsAsync_ShouldReturnPagedResult()
        {
            var eventService = _serviceProvider.GetRequiredService<IEventService>();

            var pagingArguments = new PagingArguments { First = 10 };
            var queryContext = new QueryContext<Event>();


            var result = await eventService.GetEventsAsync(pagingArguments, queryContext);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(1, result.Items.First().Id);
            Assert.Equal("life event", result.Items.First().Name);
            Assert.Equal(200, result.Items.First().MaxAttendees);
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