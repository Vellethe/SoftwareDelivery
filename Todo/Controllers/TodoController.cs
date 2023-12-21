using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Models;
using Todo.Data;

namespace APIWithDatabase.Controllers
{
    [ApiController]
    [Route("")]
    public class NotesController : ControllerBase
    {
        private readonly TodoContext _database;

        public NotesController(TodoContext database)
        {
            _database = database;
        }

        [HttpGet("notes")]
        public ActionResult<IEnumerable<Note>> GetNotes(bool? completed)
        {
            IQueryable<Note> query = _database.Notes;

            if (completed.HasValue)
            {
                query = query.Where(n => n.IsDone == completed.Value);
            }

            var notes = query.ToList();

            return Ok(notes);
        }


        [HttpPost("notes")]
        public ActionResult<Note> PostNote(Note note)
        {
            if (_database.Notes == null)
            {
                return NotFound();
            }
            _database.Notes.Add(note);
            _database.SaveChanges();

            return Ok();
        }

        [HttpGet("remaining")]
        public ActionResult GetRemaining()
        {
            var remaining = _database.Notes.Count(n => !n.IsDone);
            return Ok(remaining);
        }

        [HttpGet("/notes/{id}")]
        public ActionResult<Note> GetNote(int id)
        {
            if (_database.Notes == null)
            {
                return NotFound();
            }
            var note = _database.Notes.Find(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        [HttpPut("/notes/{id}")]
        public ActionResult<Note> UpdateStatus(int id, [FromBody] Note updatedNote)
        {
            var note = _database.Notes.Find(id);

            if (note == null)
            {
                return NotFound();
            }

            note.IsDone = updatedNote.IsDone;
            //_database.Notes.Update(note);
            _database.Entry(note).State = EntityState.Modified;
            _database.SaveChanges();

            return NoContent();
        }

        [HttpDelete("/notes/{id}")]
        public ActionResult DeleteNote(int id)
        {
            var note = _database.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            _database.Notes.Remove(note);
            _database.SaveChanges();

            return NoContent();
        }

        [HttpPost("toggle-all")]
        public ActionResult ToggleAllNotes()
        {
            var allNotes = _database.Notes.ToList();
            bool toggleToCompleted = allNotes.Any(n => !n.IsDone);

            foreach (var note in allNotes)
            {
                note.IsDone = toggleToCompleted;
                _database.Entry(note).State = EntityState.Modified;
            }

            _database.SaveChanges();

            return Ok(allNotes);
        }


        [HttpPost("clear-completed")]
        public ActionResult ClearCompletedNotes()
        {
            var completedNotes = _database.Notes.Where(n => n.IsDone).ToList();
            _database.Notes.RemoveRange(completedNotes);
            _database.SaveChanges();

            return NoContent();
        }

        [HttpGet("{completed}")]
        public ActionResult GetCompletedNotes(bool? completed)
        {
            IQueryable<Note> query = _database.Notes;

            if (completed.HasValue)
            {
                query = query.Where(n => n.IsDone == completed.Value);
            }

            var notes = query.ToList();

            return Ok(notes);
        }
    }
}
