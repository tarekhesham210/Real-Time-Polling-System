using Application.Interfaces.Reopsitory;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly ApplicationDbContext _context;

        public PollRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Poll poll)
        {
           await _context.Polls.AddAsync(poll);
        }

       

        public IQueryable<Poll> GetAll()
        {
           return _context.Polls.Where(p => !p.IsDeleted).AsNoTracking();
        }

        public async Task<Poll?> GetByIdAsync(int id)
        {
          return await _context.Polls.Where(p=>!p.IsDeleted).Include(p=>p.Options).FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
