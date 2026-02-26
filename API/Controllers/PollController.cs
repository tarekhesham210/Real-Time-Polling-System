using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollController : ControllerBase
    {
        private readonly PollService _pollService;

        public PollController(PollService pollService)
        {
            _pollService = pollService;
        }
        [HttpGet]
        
        public async Task<IActionResult> GetAll(int pageNumber=1,int pageSize=10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _pollService.GetAllPollsAsync(userId,pageNumber,pageSize);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode, new { error = result.Error });
            return StatusCode((int)result.StatusCode,result.Value);
        }
        [HttpGet("{id:int}")]
        
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _pollService.GetPollById(userId,id);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode, new { error = result.Error });
            return StatusCode((int)result.StatusCode,result.Value);
        }
        [HttpGet,Authorize]
        [Route("GetMyPolls")]
        public async Task<IActionResult> GetMyPolls(int pageNumber=1,int pageSize=10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var result = await _pollService.GetMyPolls(userId,pageNumber,pageSize);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode, new { error = result.Error });
            return StatusCode((int)result.StatusCode,result.Value);
        }
        [HttpPost("Create")]
        [Authorize]
        
        public async Task<IActionResult> Create([FromBody]CreatePollDTO request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await _pollService.AddPollAsync(userId,request);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode,new { error = result.Error });

            return StatusCode((int)result.StatusCode,result.Value); 
        }
        [HttpPost("Vote")]
        [Authorize]
        public async Task<IActionResult> Vote(VoteRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
                return Unauthorized();
           var result= await _pollService.VoteAsync(userId, request);
            if (!result.IsSuccess)
                return StatusCode((int)result.StatusCode,new {error= result.Error });

            return Ok();
        }
        [HttpPut("DeactivatePoll")]
        [Authorize]
        public async Task<IActionResult> DeactivatePoll(int pollId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var result=await _pollService.DeactivatePollVoting(userId, pollId);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode, new {error= result.Error });
            return Ok();

        }
        [HttpDelete("Delete"), Authorize]
        public async Task<IActionResult> DeletePoll(int pollId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId))
                return Unauthorized();
            var result=await _pollService.DeletePollAsync(userId, pollId);
            if(!result.IsSuccess)
                return StatusCode((int)result.StatusCode,new{error= result.Error });

            return StatusCode((int)result.StatusCode);
        }
    }
}
