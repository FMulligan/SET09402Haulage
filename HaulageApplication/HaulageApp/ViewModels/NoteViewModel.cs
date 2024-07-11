using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using HaulageApp.Models;
using HaulageApp.Data;

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
    
    private Note _note;
    private HaulageDbContext _context;
    public NoteViewModel(HaulageDbContext notesDbContext)
    {
        _context = notesDbContext;
        _note = new Note();
    }
    public NoteViewModel(HaulageDbContext notesDbContext, Note note)
    {
        _note = note;
        _context = notesDbContext;
    }

    [RelayCommand]
    private async Task Save()
    {
        _note.Date = DateTime.Now;
        if (_note.Id == 0)
        {
            _context.Notes.Add(_note);
        }
        _context.SaveChanges();
        await Shell.Current.GoToAsync($"..?saved={_note.Id}");
    }

    [RelayCommand]
    private async Task Delete()
    {
        _context.Remove(_note);
        _context.SaveChanges();
        await Shell.Current.GoToAsync($"..?deleted={_note.Id}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("load"))
        {
            _note = _context.Notes.Single(n => n.Id == int.Parse(query["load"].ToString()));
            RefreshProperties();
        }
    }

    public void Reload()
    {
        _context.Entry(_note).Reload();
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }
}