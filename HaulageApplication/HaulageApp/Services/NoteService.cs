using HaulageApp.Models;
using HaulageApp.Data;

namespace HaulageApp.Services;

public class NoteService:INoteService
{
    public HaulageDbContext _dbContext;
    public NoteService(HaulageDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public int DeleteItem(Note item)
    {
        _dbContext.Init();
        return _dbContext.connection.Delete(item);
    }

    public List<Note> GetItems()
    {
        _dbContext.Init();
        return _dbContext.connection.Table<Note>().ToList();
    }

    public Note GetItem(int id)
    {
        _dbContext.Init();
        return  _dbContext.connection.Table<Note>().Where(i => i.Id == id).FirstOrDefault();
    }

    public int SaveItem(Note item)
    {
        _dbContext.Init();
        if (item.Id != 0)
            return _dbContext.connection.Update(item);
        else
            return _dbContext.connection.Insert(item);
    }
}