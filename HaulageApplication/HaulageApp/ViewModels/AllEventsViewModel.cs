using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using HaulageApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HaulageApp.ViewModels;

public partial class AllEventsViewModel: ObservableObject, IQueryAttributable
{
    private readonly HaulageDbContext _context;
    private Trip? _trip;
    public ObservableCollection<Event> Events { get; } = [];

    public AllEventsViewModel(HaulageDbContext context)
    {
        _context = context;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("trip", out var value))
        {
            _trip = value as Trip;
        }
        Events.Clear();
        LoadEventsForTrip(_trip?.Id ?? 0);
    }
    
    private Event? _selectedEvent;
    public Event? SelectedEvent
    {
        get => _selectedEvent;
        set
        {
            if (_selectedEvent != value)
            {
                _selectedEvent = value;
                OnPropertyChanged();
            }  
        }
    }
    
    private async void LoadEventsForTrip(int tripId)
    {
        //fast handle if no trip
        if (tripId == 0)
            return;

        Events.Clear();
        var events = await _context.events
            .AsQueryable()
            .Where(e => e.TripId == tripId)
            .ToListAsync();

        foreach (var ev in events)
        {
            Events.Add(ev);
        }
    }
    
    [RelayCommand]
    private async Task AddEvent()
    {
        if (_trip == null)
        {
            Console.WriteLine("Null trip provided to AddEvent");
            return;
        }
        await Shell.Current.GoToAsync(nameof(Views.EventPage), new Dictionary<string, object>{{ "trip", _trip! }});
        SelectedEvent = null;  // Clear the selection after navigating
    }
    
    [RelayCommand]
    private async Task SelectEvent(object item)
    {
        if (_trip == null)
        {
            Console.WriteLine("Null trip provided to AddEvent");
            return;
        }
        await Shell.Current.GoToAsync(nameof(Views.EventPage), new Dictionary<string, object>{{ "trip", _trip! }, { "event", item }});
        SelectedEvent = null;  // Clear the selection after navigating
    }
}