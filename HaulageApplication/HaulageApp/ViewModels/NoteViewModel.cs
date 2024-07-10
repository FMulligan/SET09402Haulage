using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using HaulageApp.Services;
using HaulageApp.Models;

namespace HaulageApp.ViewModels;

public partial class NoteViewModel : ObservableObject, IQueryAttributable
{
    public string Text
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime Date => _note.Date;
    public int Id => _note.Id;
    
    private Models.Note _note;
    private INoteService _noteService;
    public NoteViewModel(INoteService noteService)
    {
        _noteService = noteService;
        _note = new Models.Note();
    }
    public NoteViewModel(INoteService noteService, Note note)
    {
        _note = note;
        _noteService = noteService;
    }

    [RelayCommand]
    private async Task Save()
    {
        _note.Date = DateTime.Now;
        _noteService.SaveItem(_note);
        await Shell.Current.GoToAsync($"..?saved={_note.Id}");
    }

    [RelayCommand]
    private async Task Delete()
    {
        _noteService.DeleteItem(_note);
        await Shell.Current.GoToAsync($"..?deleted={_note.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = _noteService.GetItem(int.Parse(query["load"].ToString()));
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _note = _noteService.GetItem(_note.Id);
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}