using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Reopsitory
{
    public interface IVoteRepository
    {
        Task<bool> AnyAsync(Expression<Func<Vote, bool>> predicate);
    }
}
