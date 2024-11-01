using AuthLearning.Data;
using AuthLearning.DTOs;
using AuthLearning.Models;
using Microsoft.EntityFrameworkCore;


namespace AuthLearning.Services
{
    public class NoteService
    {
        private readonly ApplicationDbContext _context;

        public NoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NoteDto>> GetNotesForUser(string userId)
        {
            return await _context.Notes
                .Where(n => n.UserId == userId)
                .Include(n => n.User)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Content = n.Content,
                    UserId = n.UserId,
                    Email = n.User.Email
                })
                .ToListAsync();
        }

        public async Task<NoteDto> GetNoteById(int id, string userId)
        {
            return await _context.Notes
                .Where(n => n.Id == id && n.UserId == userId)
                .Include(n => n.User)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Content = n.Content,
                    UserId = n.UserId,
                    Email = n.User.Email
                })
                .FirstOrDefaultAsync();
        }

        public async Task<NoteDto> CreateNoteForUser(NoteCreateDto noteDto, string userId)
        {
            var note = new Note
            {
                Content = noteDto.Content,
                UserId = userId
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return await _context.Notes
                .Where(n => n.Id == note.Id)
                .Include(n => n.User)
                .Select(n => new NoteDto
                {
                    Id = n.Id,
                    Content = n.Content,
                    UserId = n.UserId,
                    Email = n.User.Email
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateNoteForUser(int id, NoteUpdateDto noteDto, string userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note == null)
                return false;

            note.Content = noteDto.Content;
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteNoteForUser(int id, string userId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note == null)
                return false;

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
