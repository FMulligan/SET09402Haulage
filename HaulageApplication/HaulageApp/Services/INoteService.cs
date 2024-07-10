using HaulageApp.Models;

namespace HaulageApp.Services;

public interface INoteService
{
    List<Note> GetItems();
    Note GetItem(int id);
    int SaveItem(Note item);
    int DeleteItem(Note item);
}