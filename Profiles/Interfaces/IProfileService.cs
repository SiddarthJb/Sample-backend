﻿using Microsoft.AspNetCore.JsonPatch;
using Z1.Auth.Models;
using Z1.Core;
using Z1.Profiles.Dtos;

namespace Z1.Profiles.Interfaces
{
    public interface IProfileService
    {
        Task<BaseResponse<CreateProfileRequestDto>> CreateProfile(CreateProfileRequestDto model, User user);
        Task<BaseResponse<bool>> UpdateImages(IFormFile img, User user);
        BaseResponse<List<InterestsDto>> GetInterests();
        BaseResponse<List<LanguagesDto>> GetLanguages();
        Task<BaseResponse<PartialChatProfileDto>> GetPartialChatProfileAsync(int matchId,User user);
        Task<BaseResponse<ProfileDto>> GetChatProfileAsync(int matchId, User user);
        Task<BaseResponse<bool>> UpdateProfile(UpdateProfileRequestDto document, User user);
        Task<BaseResponse<ProfileDto>> GetProfileAsync(int userId, User user);
        Task<BaseResponse<bool>> DeleteImageAsync(string fileId, User user);
    }
}
