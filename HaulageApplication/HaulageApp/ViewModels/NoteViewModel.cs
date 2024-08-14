using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Models;
using HaulageApp.Data;
using Microsoft.Extensions.Logging;

namespace HaulageApp.ViewModels;

public partial class NoteViewModel : ObservableObject, IQueryAttributable
{
    private readonly HaulageDbContext _context;
    private Note _note;
    private readonly ILogger<NoteViewModel> _logger;

    public NoteViewModel(HaulageDbContext notesDbContext, ILogger<NoteViewModel> logger)
     { 
         _context = notesDbContext;
         _note = new Note();
         _logger = logger;
         _logger.LogInformation("NoteViewModel instantiated without a note.");
     }

    public NoteViewModel(HaulageDbContext notesDbContext, Note note, ILogger<NoteViewModel> logger)
    {
        _note = note;
        _context = notesDbContext;
        _logger = logger;
        _logger.LogInformation("NoteViewModel instantiated with a note ID: {NoteId}", _note.Id);
    }

    public string Text
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                OnPropertyChanged();
                _logger.LogDebug("Note text changed to: {Text}", _note.Text);
            }
        }
    }

    public DateTime Date => _note.Date;
    public int Id => _note.Id;

    [RelayCommand]
    private async Task Save()
    {
        _note.Date = DateTime.Now;
        if (_note.Id == 0)
        {
            _context.note.Add(_note);
            _logger.LogInformation("Adding a new note with date: {Date}", _note.Date);
        }
        _context.SaveChanges();
        _logger.LogInformation("Note saved with ID: {NoteId}", _note.Id);
        await Shell.Current.GoToAsync($"..?saved={_note.Id}");
    }

    [RelayCommand]
    private async Task Delete()
    {
        _context.Remove(_note);
        _context.SaveChanges();
        _logger.LogInformation("Note deleted with ID: {NoteId}", _note.Id);
        await Shell.Current.GoToAsync($"..?deleted={_note.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        { 
            int noteId = int.Parse(query["load"].ToString());
            _note = _context.note.Single(n => n.Id == noteId);
            RefreshProperties();
            _logger.LogInformation("Loaded note with ID: {NoteId}", _note.Id);
        }
    }
    public void Reload()
    {
        _context.Entry(_note).Reload();
        RefreshProperties();
        _logger.LogInformation("Note reloaded with ID: {NoteId}", _note.Id);
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
        _logger.LogDebug("Properties refreshed for note ID: {NoteId}", _note.Id);
    }
}