using System.Collections;
using CommunityToolkit.Mvvm.Input;
using HaulageApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HaulageApp.Services;

namespace HaulageApp.ViewModels;

public class AllNotesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModels.NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    private INoteService _noteService;

    public AllNotesViewModel(INoteService noteService)
    {
        _noteService = noteService;
        AllNotes = new ObservableCollection<NoteViewModel>(
            _noteService.GetItems().Select(n => new NoteViewModel(_noteService, n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsync(ViewModels.NoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("deleted"))
        {
            string noteId = query["deleted"].ToString();
            NoteViewModel matchedNote = AllNotes.Where((n) => n.Id == int.Parse(noteId)).FirstOrDefault();

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("saved"))
        {
            string noteId = query["saved"].ToString();
            NoteViewModel matchedNote = AllNotes.Where((n) => n.Id == int.Parse(noteId)).FirstOrDefault();

            // If note is found, update it
            if (matchedNote != null)
            {
                matchedNote.Reload();
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            // If note isn't found, it's new; add it.
            else
                AllNotes.Insert(0, new NoteViewModel(_noteService, _noteService.GetItem(int.Parse(noteId))));
        }
    }
}