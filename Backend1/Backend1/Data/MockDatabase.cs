using Backend.Data;
using Backend1.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend1.Data
{
    public class MockDatabase
    {
        public static TodoContext CreateMockDatabase()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "MockDatabase")
                .Options;

            var mockDatabase = new TodoContext(options);

            // Seed the mock database with some initial data if needed
            var initialData = new List<Note>
            {
                new Note { Id = 1, Text = "Task 1", IsDone = false },
                new Note { Id = 2, Text = "Task 2", IsDone = true },
                new Note { Id = 3, Text = "Task 3", IsDone = true },
                new Note { Id = 4, Text = "Task 4", IsDone = false },
                new Note { Id = 5, Text = "Task 5", IsDone = true },
                new Note { Id = 6, Text = "Task 6", IsDone = true },
                new Note { Id = 7, Text = "Task 7", IsDone = false },
            };

            mockDatabase.Notes.AddRange(initialData);
            mockDatabase.SaveChanges();

            return mockDatabase;
        }
    }
}
