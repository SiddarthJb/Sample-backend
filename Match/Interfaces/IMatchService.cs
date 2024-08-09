using Z1.Auth.Models;
using Z1.Core;
using Z1.Match.Dtos;

namespace Z1.Match.Interfaces
{
    public interface IMatchService
    {
        public Task<BaseResponse<FiltersDto>> GetFilters(User user);
        public Task<BaseResponse<bool>> SaveFilterPreferenceAsync(User user, FiltersDto filtersRequestDto);
        Task<bool> AddToMatchQueue(string userId, string latitude, string longitude);
        Task<bool> Search();
        Task<BaseResponse<bool>> Like(User user, int likeUserId);
        Task<BaseResponse<bool>> Skip(User user, int partialUserId);
        BaseResponse<CurrentMatchDto> GetCurrentMatch(User user);


    }
}
