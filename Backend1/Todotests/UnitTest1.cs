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
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid()) // Use a unique database name for each test
            .Options;

        _context = new TodoContext(dbContextOptions);

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
    public void AddNote_ShouldAddANewNote()
    {
        // Arrange
        var controller = new NotesController(_context);

        // Act
        var result = controller.PostNote(new Note { Text = "New Note", IsDone = false });

        // Assert
        Assert.NotNull(result);

        // Assuming you want to check for a specific result type
        Assert.IsType<OkResult>(result);
    }

    //[Fact]
    //public void AddNote_ShouldAddANewNote()
    //{
    //    // Arrange
    //    var controller = new NotesController(_context);

    //    // Act
    //    var result = controller.AddNote(new Note { Text = "New Note", IsDone = false });

    //    // Assert
    //    Assert.NotNull(result);

    //    var addedNoteObject = controller.GetNoteByText("New Note");
    //    Assert.NotNull(addedNoteObject);

    //    var addedNote = (Note)addedNoteObject;

    //    Assert.Equal("New Note", addedNote.Text);
    //    Assert.False(addedNote.IsDone);
    //}

    [Fact]
    public void ClearCompletedNotes()
    {
        // Arrange
        var controller = new NotesController(_context);

        // Act
        var result = controller.ClearCompletedNotes();

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Equal(2, _context.Notes.Count()); // Adjust expected count
        Assert.Equal("Note 1", _context.Notes.First().Text);
    }

    [Fact]
    public void ReturnsNumberOfRemainingNotes()
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
        Assert.Equal(2, Convert.ToInt32(value)); // Adjust expected count
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}


