using Application.DTOs;
using Application.Interfaces.Reopsitory;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Application.Interfaces.Message;

namespace Application.Services
{
    public class PollService
    {
        private readonly IPollRepository _pollRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public PollService(IPollRepository pollRepository, IMapper mapper, IVoteRepository voteRepository, IMessageService messageService)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
            _voteRepository = voteRepository;
            _messageService = messageService;
        }

        public async Task<Result<PagedResult<PollResponseDTO>>> GetAllPollsAsync(string? userId,int pageNumber=1, int pageSize=10)
        {
            var pollsQuery = _pollRepository.GetAll();
            var totalCount = await pollsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var polls = await pollsQuery
                .OrderByDescending(p => p.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(poll=>new PollResponseDTO
                {
                    Id = poll.Id,
                    CreatedAt = poll.CreatedAt,
                    CreatorId=poll.CreatedById,
                    CreatorName=poll.User.FirstName+" "+poll.User.LastName,
                    Question=poll.Question,
                    TotalVotes = poll.Options.SelectMany(o => o.Votes).Count(),
                    IsActive=poll.IsActive,
                    VotedOptionId = userId == null
                        ? null
                        : poll.Options
                        .Where(o => o.Votes.Any(v => v.UserId == userId))
                        .Select(o => (int?)o.Id) 
                        .FirstOrDefault(),
                    Options =poll.Options.Select(o=>new OptionResponseDTO
                    {
                        OptionId=o.Id,
                        Text=o.Text,
                        VoteCount=o.Votes.Count,
                        Percentage= poll.Options.
                            SelectMany(opt => opt.Votes).Any()
                            ? Math.Round((double)o.Votes.Count / poll.Options.SelectMany(opt => opt.Votes).Count() * 100, 2)
                            : 0
                    }).ToList()
                })
                .ToListAsync();
              
            var pagedResult = new PagedResult<PollResponseDTO>
            {
                Items = polls,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalCount = totalCount
            };
            return Result<PagedResult<PollResponseDTO>>.Success(pagedResult);
        }

        public async Task<Result<PollResponseDTO>> GetPollById(string? userId, int pollId)
        {
            var poll = await _pollRepository.GetAll()
             .Where(p => p.Id == pollId)
             .Select(p => new PollResponseDTO
             {
                 Id = p.Id,
                 TotalVotes = p.Options.SelectMany(o => o.Votes).Count(),
                 CreatedAt = p.CreatedAt,
                 CreatorId = p.CreatedById,
                 CreatorName = p.User.FirstName + " " + p.User.LastName,
                 IsActive = p.IsActive,
                 Question = p.Question,
                 VotedOptionId = userId == null
                        ? null
                        : p.Options
                        .Where(o => o.Votes.Any(v => v.UserId == userId))
                        .Select(o => (int?)o.Id)
                        .FirstOrDefault(),
                 Options = p.Options.Select(o => new OptionResponseDTO
                 {
                     OptionId = o.Id,
                     Text=o.Text,
                     VoteCount = o.Votes.Count(),
                     Percentage = p.Options.SelectMany(opt => opt.Votes).Count() > 0
                         ? Math.Round((double)o.Votes.Count() / p.Options.SelectMany(opt => opt.Votes).Count() * 100, 2)
                         : 0
                 }).ToList()
             })
             .FirstOrDefaultAsync();

            if (poll is null)
                return Result<PollResponseDTO>.Failure("Not found", HttpStatusCode.NotFound);
            return Result<PollResponseDTO>.Success(poll, HttpStatusCode.OK);

        }
        public async Task<Result<PagedResult<UserPollsResponseDTO>>> GetMyPolls(string userId,int pageNumber=1,int pageSize=10)
        {
            var pollQuery =_pollRepository.GetAll();
            var totalCount=await pollQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var polls= await pollQuery.
                Where(p=>p.CreatedById==userId)
                .OrderByDescending(p=>p.CreatedAt)
                .Skip(pageSize * (pageNumber-1))
                .Take(pageSize)
                .Select(p=>new UserPollsResponseDTO
                {
                    Question=p.Question,
                    CreatedAt=p.CreatedAt,
                    Id=p.Id,
                    IsActive=p.IsActive,
                    TotalVotes= p.Options.SelectMany(o => o.Votes).Count()
                })
                .ToListAsync();
            var pagedResult = new PagedResult<UserPollsResponseDTO>
            {
                Items = polls,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalCount = totalCount
            };

            return Result<PagedResult<UserPollsResponseDTO>>.Success(pagedResult);

        }
        public async Task<Result<PollResponseDTO>> AddPollAsync(string userId,CreatePollDTO pollDTO)
        {
            var poll = _mapper.Map<Poll>(pollDTO);
             poll.CreatedById = userId;
            
            await _pollRepository.AddAsync(poll);
            var saved = await _pollRepository.SaveAsync();
            if (!saved)
                return Result<PollResponseDTO>.Failure("Poll Adding faild", HttpStatusCode.InternalServerError);
            var result=_mapper.Map<PollResponseDTO>(poll);
            return Result<PollResponseDTO>.Success(result, HttpStatusCode.Created);
        }

        public async Task<Result<bool>> VoteAsync(string userId, VoteRequest request)
        {
            
            var poll=await _pollRepository.GetByIdAsync(request.pollId);
            if (poll == null)
                return Result<bool>.Failure("Poll not found", HttpStatusCode.NotFound);
            var alreadyVoted=await _voteRepository.AnyAsync(x => x.UserId == userId && x.PollId==request.pollId);
            if (alreadyVoted)
                return Result<bool>.Failure("You already voted this poll", HttpStatusCode.BadRequest);
            if(!poll.IsActive )
                return Result<bool>.Failure("Voting has expired",HttpStatusCode.BadRequest);
            if (poll.CreatedById == userId)
                return Result<bool>.Failure("Poll owner cant vote", HttpStatusCode.Conflict);
            if (!poll.Options.Any(o => o.Id == request.optionId))
                return Result<bool>.Failure("Invalid Choice", HttpStatusCode.BadRequest);
            

            var option = poll.Options.First(o=>o.Id == request.optionId);
            option.Votes.Add(new Vote { UserId = userId, OptionId = request.optionId, PollId= request.pollId });
           var saved= await _pollRepository.SaveAsync();
            if (!saved)
                return Result<bool>.Failure("Voting faild",HttpStatusCode.InternalServerError);
            var updatedResults = await GetPollResultsInternal(request.pollId);

            await _messageService.SendPollUpdate(updatedResults,poll.Id);
            Console.WriteLine("--- SIGNALR SEND CALLED ---");
            return Result<bool>.Success(HttpStatusCode.OK);
        }
        public async Task<Result<bool>> DeactivatePollVoting(string userId,int pollId)
        {
            var poll=await _pollRepository.GetByIdAsync(pollId);
            if(poll is null)
                return Result<bool>.Failure("Poll not found", HttpStatusCode.NotFound);
            if (poll.CreatedById != userId)
                return Result<bool>.Failure("Forbidden", HttpStatusCode.Forbidden);
            poll.IsActive = false;
           var saved= await _pollRepository.SaveAsync();
            if (!saved)
                return Result<bool>.Failure("Deactivating faild", HttpStatusCode.InternalServerError);
            return Result<bool>.Success();
        }

        public async Task<Result<bool>> DeletePollAsync(string userId,int pollId)
        {
            var poll = await _pollRepository.GetByIdAsync(pollId);
            if (poll is null)
                return Result<bool>.Failure("Invalid poll data",HttpStatusCode.NotFound);
            if (poll.CreatedById != userId)
                return Result<bool>.Failure("UnAuthorized",HttpStatusCode.Unauthorized);
            poll.IsDeleted = true;
           var deleted= await _pollRepository.SaveAsync();
            if(!deleted)
                return Result<bool>.Failure("Deleteing faild", HttpStatusCode.InternalServerError);
            
            return Result<bool>.Success();

        }

        private async Task<PollResultsUpdateDTO> GetPollResultsInternal(int pollId)
        {
            var results = await _pollRepository.GetAll()
                .Where(p => p.Id == pollId)
                .Select(p => new PollResultsUpdateDTO
                {
                    PollId = p.Id,
                    TotalVotes = p.Options.SelectMany(o => o.Votes).Count(),
                    Options = p.Options.Select(o => new OptionResponseDTO
                    {
                        OptionId = o.Id,
                        VoteCount = o.Votes.Count(),
                        Percentage = p.Options.SelectMany(opt => opt.Votes).Count() > 0
                            ? Math.Round((double)o.Votes.Count() / p.Options.SelectMany(opt => opt.Votes).Count() * 100, 2)
                            : 0
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return results ?? new PollResultsUpdateDTO(); 
        }
    }
}
