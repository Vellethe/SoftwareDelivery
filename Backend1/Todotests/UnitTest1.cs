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
    public void AddNote()
    {
        // Arrange
        var controller = new NotesController(_context);
        var newNote = new Note { Text = "New Note", IsDone = false };

        // Act
        var result = controller.PostNote(newNote);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ActionResult<Note>>(result);
    }


    //Fråga om denna nästa lektion på plats

    //[Fact]
    //public void AddNote_ShouldAddANewNote()
    //{
    //    // Arrange
    //    var controller = new NotesController(_context);

    //    // Act
    //    var result = controller.PostNote(new Note { Text = "New Note", IsDone = false });

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.IsType<OkResult>(result);
    //}

    [Fact]
    public void DeleteNote()
    {
        // Arrange
        var notesController = new NotesController(_context);

        // Act
        notesController.DeleteNote(4);

        // Assert
        var deletedNote = _context.Notes.Find(4);
        Assert.Null(deletedNote);
        Assert.Equal(5, _context.Notes.Count());
    }

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


