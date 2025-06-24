using EventSignup.Models;
using EventSignup.Services;

namespace EventSignup.GqlTypes;

[ExtendObjectType(typeof(Mutation))]
public class EventTypeMutation
{
    public async Task<EventResult> CreateEvent(EventInput input, [Service] IDatabaseService databaseService)
    {
        var eventItem = new Event{
            Name = input.Name,
            Date =input.Date,
            MaxAttendees = input.MaxAttendees
        };

        var createdEvent = await databaseService.CreateEventAsync(eventItem);

        return new EventResult
        {
            Success = true,
            Message = "Event created successfully",
            Event = createdEvent
        };
    }

    public async Task<EventResult> UpdateEvent(int id, EventInput input, [Service] IDatabaseService databaseService)
    {
        var validationResult = await ValidateEventRequest(id, databaseService);
        
        if (!validationResult.Success)
        {
            return validationResult;
        }

        return await UpdateEventInTheDatabase(input, validationResult.Event!, databaseService);
    }

    public async Task<EventResult> DeleteEvent(int id, [Service] IDatabaseService databaseService)
    {
        var validationResult = await ValidateEventRequest(id, databaseService);
        
        if (!validationResult.Success)
        {
            return validationResult;
        }

        await databaseService.DeleteEventAsync(id);

        return new EventResult
        {
            Success = true,
            Message = "Event deleted successfully"
        };
    }

    private async Task<EventResult> UpdateEventInTheDatabase(EventInput input, Event eventItem, IDatabaseService databaseService)
    {
        eventItem.Name = input.Name;
        eventItem.Date = input.Date;
        eventItem.MaxAttendees = input.MaxAttendees;

        var updatedEvent = await databaseService.UpdateEventAsync(eventItem);

        return new EventResult
        {
            Success = true,
            Message = "Event updated successfully",
            Event = updatedEvent
        };
    }

    private async Task<EventResult> ValidateEventRequest(int id, IDatabaseService databaseService)
    {
        var eventItem = await databaseService.GetEventByIdAsync(id);

        if (eventItem == null)
        {
            return new EventResult { Success = false, Message = "Event not found" };
        }
        return new EventResult { Success = true, Message = "Event found", Event = eventItem };
    }
}


public class EventInput
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int MaxAttendees { get; set; }
}

public class EventResult
{
    public bool Success { get; set; }
    public string Message { get; set; }  = string.Empty;
    public Event? Event { get; set; } 
}