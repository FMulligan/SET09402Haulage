using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Data;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace HaulageApp.ViewModels
{
    public class AllNotesViewModel : IQueryAttributable
    {
        public ObservableCollection<NoteViewModel> AllNotes { get; }
        public ICommand NewCommand { get; }
        public ICommand SelectNoteCommand { get; }

        private readonly HaulageDbContext _context;
        private readonly ILogger<AllNotesViewModel> _logger;
        private readonly ILogger<NoteViewModel> _noteLogger;

        public AllNotesViewModel(HaulageDbContext notesContext, ILogger<AllNotesViewModel> logger, ILogger<NoteViewModel> noteLogger)
        {
            _context = notesContext;
            _logger = logger;
            _noteLogger = noteLogger;

            try
            {
                var notes = _context.note.ToList().Select(n => new NoteViewModel(_context, n, _noteLogger));
                AllNotes = new ObservableCollection<NoteViewModel>(notes);
                _logger.LogInformation("AllNotes initialized successfully with {Count} notes.", AllNotes.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing AllNotes.");
                AllNotes = new ObservableCollection<NoteViewModel>(); // Initialize to avoid null reference
            }

            NewCommand = new AsyncRelayCommand(NewNoteAsync);
            SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
        }

        private async Task NewNoteAsync()
        {
            _logger.LogInformation("Navigating to new note page.");
            await Shell.Current.GoToAsync(nameof(Views.NotePage));
        }

        private async Task SelectNoteAsync(NoteViewModel? note)
        {
            if (note != null)
            {
                _logger.LogInformation("Navigating to note page for note ID: {NoteId}", note.Id);
                await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Id}");
            }
            else
            {
                _logger.LogWarning("SelectNoteAsync called with null note.");
            }
        }

        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("deleted"))
            {
                string noteIdStr = query["deleted"].ToString();
                if (int.TryParse(noteIdStr, out int noteId))
                {
                    var matchedNote = AllNotes.FirstOrDefault(n => n.Id == noteId);

                    // If note exists, delete it
                    if (matchedNote != null)
                    {
                        AllNotes.Remove(matchedNote);
                        _logger.LogInformation("Note with ID {NoteId} removed from AllNotes.", noteId);
                    }
                    else
                    {
                        _logger.LogWarning("Note with ID {NoteId} not found in AllNotes.", noteId);
                    }
                }
                else
                {
                    _logger.LogError("Invalid note ID {NoteIdStr} in query for deletion.", noteIdStr);
                }
            }
            else if (query.ContainsKey("saved"))
            {
                string noteIdStr = query["saved"].ToString();
                if (int.TryParse(noteIdStr, out int noteId))
                {
                    var matchedNote = AllNotes.FirstOrDefault(n => n.Id == noteId);

                    // If note is found, update it
                    if (matchedNote != null)
                    {
                        matchedNote.Reload();
                        AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
                        _logger.LogInformation("Note with ID {NoteId} updated and moved to top of AllNotes.", noteId);
                    }
                    // If note isn't found, it's new; add it.
                    else
                    {
                        var newNote = new NoteViewModel(_context, _context.note.Single(n => n.Id == noteId), _noteLogger);
                        AllNotes.Insert(0, newNote);
                        _logger.LogInformation("New note with ID {NoteId} added to AllNotes.", noteId);
                    }
                }
                else
                {
                    _logger.LogError("Invalid note ID {NoteIdStr} in query for saving.", noteIdStr);
                }
            }
        }
    }
}
