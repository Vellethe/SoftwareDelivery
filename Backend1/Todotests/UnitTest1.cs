using APIWithDatabase.Controllers;
using Backend.Data;
using Backend1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class NotesControllerTests : IDisposable
{
    private readonly TodoContext _context;
    private readonly Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

    public NotesControllerTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<TodoContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TodosDatabase;Integrated Security=True;MultipleActiveResultSets=true")
            .Options;

        _context = new TodoContext(dbContextOptions);
        _transaction = _context.Database.BeginTransaction();

        _context.Notes.AddRange(
            new Note { Text = "Note 1", IsDone = false },
            new Note { Text = "Note 2", IsDone = true },
            new Note { Text = "Note 3", IsDone = true },
            new Note { Text = "Note 4", IsDone = false },
            new Note { Text = "Note 5", IsDone = true },
            new Note { Text = "Note 6", IsDone = true }
        );
        _context.SaveChanges();
    }

    [Fact]
    public void PostNote_AddsNoteToDatabase()
    {
        // Arrange
        var controller = new NotesController(_context);

        // Act
        var result = controller.PostNote(new Note { Text = "Test Note", IsDone = false });

        // Assert
        Assert.IsType<OkResult>(result);

        // Check that the note has been added to the database
        Assert.Equal(2, _context.Notes.Count());
        Assert.Equal("Test Note", _context.Notes.First().Text);
    }

    [Fact]
    public void ClearCompletedNotes_RemovesCompletedNotes()
    {
        // Arrange
        var controller = new NotesController(_context);

        // Act
        var result = controller.ClearCompletedNotes();

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Check that completed notes have been removed from the database
        Assert.Equal(2, _context.Notes.Count());
        Assert.Equal("Note 2", _context.Notes.First().Text);
    }

    [Fact]
    public void GetRemaining_ReturnsNumberOfRemainingNotes()
    {
        // Arrange
        var controller = new NotesController(_context);

        // Act
        var result = controller.GetRemaining();

        // Assert
        Assert.IsType<OkObjectResult>(result);

        // Check that the correct number of remaining notes is returned
        var value = (result as OkObjectResult)?.Value;
        Assert.NotNull(value);
        Assert.Equal(2, Convert.ToInt32(value));
    }

    public void Dispose()
    {
        // Roll back the transaction to undo changes made during tests
        _transaction.Rollback();

        // Release resources
        _transaction.Dispose();
        _context.Dispose();
    }
}
