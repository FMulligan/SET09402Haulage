using System.ComponentModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.ViewModels;

public partial class EventViewModel : ObservableObject, IQueryAttributable
{
    private readonly HaulageDbContext _context;
    private Event? _event;
    private Trip? _trip;
    private string _timestamp;

    public EventViewModel(HaulageDbContext context)
    {
        _context = context;
        _timestamp = String.Empty;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        try
        {
            //Set trip
            if (query.TryGetValue("trip", out var value))
            {
                _trip = value as Trip;
            }
            
            //Set event
            if (query.TryGetValue("event", out var item))
            {
                _event = item as Event;
                EventType = _event.EventType;
                TimestampString = _event?.Timestamp?.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture) ?? String.Empty;
            }
        }catch (Exception ex)
        {
            // Log exception and re-throw
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    private string? _eventType;
    public string? EventType
    {
        get => _eventType;
        set 
        {
            if (_eventType != value)
            {
                _eventType = value;
                OnPropertyChanged();
            }  
        }
    }
    
    public string TimestampString
    {
        get => _timestamp;
        set 
        {
            if (_timestamp != value)
            {
                _timestamp = value;
                OnPropertyChanged();
            }  
        }
    }

    [RelayCommand]
    public async Task Save()
    {
        //Check if EventType is null
        if (string.IsNullOrWhiteSpace(EventType))
        {
            await Shell.Current.DisplayAlert("Error", "Invalid Input, Event type must be specified", "Confirm");
            return;
        }
        try
        {
            if (_event == null)
            {
                // Create a new event
                _event = new Event
                {
                    TripId = _trip?.Id ?? 0,
                    EventType = EventType,
                    Timestamp = DateTime.Now
                };
                _context.events.Add(_event);
                await _context.SaveChangesAsync();
            }
            else
            {
                //Update an event
                try
                {
                    _context.events
                        .AsQueryable()
                        .Where(e => e.Id == _event.Id)
                        .ExecuteUpdate(x => 
                            x.SetProperty(e => e.EventType, e => EventType)
                        );
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            await _context.Entry(_event).ReloadAsync();
            await Shell.Current.GoToAsync($"..?saved={_event.Id}");
        }
        catch (Exception ex)
        {
            // Handle save exceptions
            Console.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Failed to save event", "Confirm");
        }
    }

    [RelayCommand]
    public async Task Delete()
    {
        if (_event == null)
        {
            await Shell.Current.DisplayAlert("Error", "Cannot delete a new event", "Confirm");
            return;
        }

        try
        {
            _context.events.Remove(_event);
            await _context.SaveChangesAsync();
            await Shell.Current.GoToAsync($"..?deleted={_event.Id}");
        }
        catch (Exception ex)
        {
            // Handle delete exceptions
            Console.WriteLine($"Error: {ex.Message}");
            await Shell.Current.DisplayAlert("Error", "Failed to delete event", "Confirm");
        }
    }

}