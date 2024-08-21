using HaulageApp.Common;
using Moq;
using HaulageApp.Data;
using HaulageApp.Models;
using HaulageApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HaulageAppTests;

public class EditExpenseTest
{
    private readonly Mock<HaulageDbContext> _mockContext;
    private readonly EditExpenseViewModel _viewModel;
    private readonly Mock<DbSet<Expense>> _mockExpense;

    public EditExpenseTest()
    {
        _mockContext = new Mock<HaulageDbContext>();
        _mockExpense = new Mock<DbSet<Expense>>();
        Mock<INavigationService> mockNavigationService = new();
        _viewModel = new EditExpenseViewModel(_mockContext.Object, mockNavigationService.Object);
    }

    [Fact]
    public void ApplyQueryAttributesShouldAssignTripAndExpense()
    {
        var trip = new Trip { Id = 1, Driver = 2 };
        var expense = new Expense { Id = 2, Amount = 100 };
        var query = new Dictionary<string, object> { { "trip", trip }, { "expense", expense } };
        _viewModel.ApplyQueryAttributes(query);
        Assert.Equal(_viewModel.Amount, 100);
    }

    [Fact] public async Task SaveShouldAddNewExpenseWhenExpenseIsNull()
    {
      var trip = new Trip { Id = 1, Driver = 2 }; 
      _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "trip", trip } }); 
      _viewModel.Amount = 200;
      _mockContext.Setup(e => e.expense).Returns(_mockExpense.Object);
      await _viewModel.Save();
      _mockExpense.Verify(c => c.Add(It.Is<Expense>(e => e.Amount == 200 && e.Trip == 1)), Times.Once); 
      _mockContext.Verify(c => c.SaveChanges(), Times.Once); 
    }

    [Fact]
    public async Task SaveShouldUpdateExistingExpenseWhenExpenseIsNotNull()
    {
        var expense = new Expense { Id = 2, Amount = 100 };
        _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "expense", expense } });
        _viewModel.Amount = 150;
        _mockContext.Setup(e => e.expense).Returns(_mockExpense.Object);
        _mockContext.Setup(e => e.Entry(expense)).Returns(new Mock<FakeEntityEntry<Expense>>().Object);
        await _viewModel.Save();
        _mockExpense.Verify(c => c.AsQueryable(), Times.Once);
    }

    [Fact] public async Task DeleteShouldRemoveExpense() 
    { 
      var expense = new Expense { Id = 2, Amount = 100m };
      _viewModel.ApplyQueryAttributes(new Dictionary<string, object> { { "expense", expense } });
      _mockContext.Setup(e => e.expense).Returns(_mockExpense.Object);
      await _viewModel.Delete();
      _mockExpense.Verify(c => c.AsQueryable(), Times.Once);
      }
}

public class FakeEntityEntry<TEntity>() : EntityEntry<TEntity>(null) where TEntity : class;