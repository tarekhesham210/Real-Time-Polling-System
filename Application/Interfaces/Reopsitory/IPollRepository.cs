using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Reopsitory
{
    public interface IPollRepository
    {
        Task AddAsync(Poll poll);
        Task<Poll?> GetByIdAsync(int id);
        public IQueryable<Poll> GetAll();
        Task<bool> SaveAsync();
    }
}
