using Microsoft.AspNetCore.Mvc;
using Z1.Auth.Models;
using Z1.Match.Dtos;
using Z1.Match.Interfaces;

namespace Z1.Match
{
    [Route("[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("get-filters")]
        public async Task<IActionResult> GetFilters()
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _matchService.GetFilters(user));
        }

        [HttpPatch("update-filters")]
        public async Task<IActionResult> UpdateFilters(FiltersDto filtersRequestDto)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _matchService.SaveFilterPreferenceAsync(user, filtersRequestDto));
        }

        [HttpPost("like/{likeUserId}/")]
        public async Task<IActionResult> Like(int likeUserId)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _matchService.Like(user, likeUserId));
        }

        [HttpPost("skip/{partialUserId}/")]
        public async Task<IActionResult> Skip(int partialUserId)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _matchService.Skip(user, partialUserId));
        }

        [HttpPost("unmatch/{matchId}/")]
        public async Task<IActionResult> Unmatch(int matchId)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _matchService.Unmatch(user, matchId));
        }

        [HttpPost("blockAndReport/")]
        public async Task<IActionResult> BlockAndReport(BlockAndReportDto model)
        {
            var user = (User)HttpContext.Items["User"];
            return Ok(await _matchService.BlockAndReport(user, model));
        }
    }
}
