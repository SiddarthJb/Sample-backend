using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Net;
using Z1.Auth.Models;
using Z1.Core;
using Z1.Core.Data;
using Z1.Core.Exceptions;
using Z1.Core.Interfaces;
using Z1.Match.Models;
using Z1.Profiles.Dtos;
using Z1.Profiles.Interfaces;
using Z1.Profiles.Models;
using static Z1.Profiles.Enums;

namespace Z1.Profiles
{
    public class ProfileService : IProfileService
    {
        private readonly AppDbContext _context;
        private readonly IBlobService _blobService;

        public ProfileService(AppDbContext context, IBlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        public BaseResponse<List<InterestsDto>> GetInterests()
        {
            var response = new BaseResponse<List<InterestsDto>>();
            var interests = _context.Interests.ToList();
            response.Data = interests.Select(x => new InterestsDto()
            {
                Id = x.Id,
                Interest = x.Interest,
            }).ToList();
            return response;
        }

        public BaseResponse<List<LanguagesDto>> GetLanguages()
        {
            var response = new BaseResponse<List<LanguagesDto>>();
            var languages = _context.Languages.ToList();
            response.Data = languages.Select(x => new LanguagesDto()
            {
                Id = x.Id,
                Language = x.Language,
            }).ToList();
            return response;
        }

        public async Task<BaseResponse<CreateProfileRequestDto>> CreateProfile(CreateProfileRequestDto model, User user)
        {
            var dob = DateOnly.Parse(model.Dob);
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var age = (byte)(DateTime.Today.Year - dob.Year);

            var profile = new Profile()
            {
                UserId = user.Id,
                Name = model.Name,
                Dob = dob,
                Age = age,
                Gender = model.Gender,
                Education = model.Education,
                MaritalStatus = model.MaritalStatus,
                Kids = model.Kids,
                Religion = model.Religion,
                Profession = model.Profession,
                Alcohol = model.Alcohol,
                Smoke = model.Smoke,
                Zodiac = model.Zodiac,
                Location = geometryFactory.CreatePoint(new Coordinate(-5.9348902, 54.5881783))
            };

            profile.Languages.Clear();
            foreach (var languageId in model.LanguageIds)
            {
                var language = await _context.Languages.FindAsync(languageId);
                if (language != null)
                {
                    profile.Languages.Add(language);
                }
            }

            profile.Interests.Clear();
            foreach (var intrestId in model.InterestIds)
            {
                var interest = await _context.Interests.FindAsync(intrestId);
                if (interest != null)
                {
                    profile.Interests.Add(interest);
                }
            }


            _context.Profiles.Add(profile);

            //Initialize filters
            var filter = new Filter()
            {
                User = user,
                UserId = user.Id,
                MinAge = age - 10 < 18 ? 18 : age - 10,
                MaxAge = age + 10,
                //MinHeight = 140,
                //MaxHeight = 
                MaxDistance = 50,
            };

            _context.Filters.Add(filter);

            _context.ShowMeFilters.Add(new ShowMeFilter()
            {
                Filter = filter,
                FilterId = filter.Id,
                Value = model.ShowMe
            });

            _context.RelationshipTypeFilters.Add(new RelationshipTypeFilter()
            {
                Filter = filter,
                FilterId = filter.Id,
                Value = model.RelationshipType
            });

            await _context.SaveChangesAsync();

            var response = new BaseResponse<CreateProfileRequestDto>();
            response.Data = model;
            return response;
        }

        public async Task<BaseResponse<bool>> UpdateProfile(UpdateProfileRequestDto patch, User user)
        {
            var profile = await _context.Profiles.Include(p => p.Languages).Include(p => p.Interests)
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (profile == null)
            {
                throw new NotFoundException("Profile not found");
            }

            profile.Education = patch.Education ?? profile.Education;
            profile.MaritalStatus =  patch.MaritalStatus ?? profile.MaritalStatus;
            profile.Kids = patch.Kids ?? profile.Kids;
            profile.Zodiac = patch.Zodiac ?? profile.Zodiac;
            profile.Religion = patch.Religion ?? profile.Religion;
            profile.Profession = patch.Profession ?? profile.Profession;
            profile.Alcohol = patch.Alcohol ?? profile.Alcohol;
            profile.Smoke = patch.Smoke ?? profile.Smoke;
            profile.Bio = patch.Bio ?? profile.Bio;
            profile.Work = patch.Work ?? profile.Work;

            if (patch.LanguageIds != null)
            {
                profile.Languages.Clear();
                foreach (var languageId in patch.LanguageIds)
                {
                    var language = await _context.Languages.FindAsync(languageId);
                    if (language != null)
                    {
                        profile.Languages.Add(language);
                    }
                }
            }

            if(patch.InterestIds != null)
            {
                profile.Interests.Clear();
                foreach (var intrestId in patch.InterestIds)
                {
                    var interest = await _context.Interests.FindAsync(intrestId);
                    if (interest != null)
                    {
                        profile.Interests.Add(interest);
                    }
                }
            }

            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var response = new BaseResponse<bool>();
            response.Data = true;
            return response;
        }

        public async Task<BaseResponse<PartialChatProfileDto>> GetPartialChatProfileAsync(int matchId, User user)
        {
            var match = await _context.Matches.FindAsync(matchId);

            if(match == null)
            {
                throw new NotFoundException("MatchId not found");
            }

            var matchUserId = match.User1Id == user.Id ? match.User2Id : match.User1Id;

            var profile = _context.Profiles
                            .Include(x => x.Languages)
                            .Include(x => x.Interests)
                            .Include(x => x.Images)
                            .First(x => x.UserId == matchUserId);

            var partialProfile = new PartialChatProfileDto()
            {
                ImageUrl = _blobService.GenerateSasToken(profile.Images.First().ImageUrl),
                Age = profile.Age,
                Height = profile.Height,
                Gender = ((Gender)profile.Gender).ToString(),
                MaritalStatus = ((MaritalStatus)profile.MaritalStatus).ToString(),
                Education = ((Education)profile.Education).ToString(),
                Kids = ((Kids)profile.Kids).ToString(),
                Alcohol = ((Alcohol)profile.Alcohol).ToString(),
                Smoke = ((Smoke)profile.Smoke).ToString(),
                Religion = ((Religion)profile.Religion).ToString(),
                Profession = ((Profession)profile.Profession).ToString(),
                Languages = profile.Languages.Select(x => x.Language).ToList(),
                Interests = profile.Interests.Select(x => x.Interest).ToList(),
            };

            var response = new BaseResponse<PartialChatProfileDto>();
            response.Data = partialProfile;
            return response;
        }

        public async Task<BaseResponse<ProfileDto>> GetChatProfileAsync(int matchId, User user)
        {
            var match = await _context.Matches.FindAsync(matchId);
             
            if (match == null)
            {
                throw new NotFoundException("MatchId not found");
            }

            if ((match.IsPartial && (match.User1Id == user.Id ? match.User1Liked == true : match.User2Liked == true)) || match.IsPartial == false)
            {
                var matchUserId = match.User1Id == user.Id ? match.User2Id : match.User1Id;
                var profile = _context.Profiles
                                .Include(x => x.Languages)
                                .Include(x => x.Interests)
                                .Include(x => x.Images)
                                .First(x => x.UserId == matchUserId);

                var images = profile.Images.Where(x => x.IsActive && !x.IsBlurred)
                    .Select(x => {
                        var blobName = _blobService.GenerateSasToken(x.ImageUrl);
                        return new ImagesDto { Image = blobName, Order = x.Order };
                        })
                    .ToList();

                var partialProfile = new ProfileDto()
                {
                    Name = profile.Name,
                    Age = profile.Age,
                    Height = profile.Height,
                    Gender = ((Gender)profile.Gender).ToString(),
                    MaritalStatus = ((MaritalStatus)profile.MaritalStatus).ToString(),
                    Education = ((Education)profile.Education).ToString(),
                    Kids = ((Kids)profile.Kids).ToString(),
                    Alcohol = ((Alcohol)profile.Alcohol).ToString(),
                    Smoke = ((Smoke)profile.Smoke).ToString(),
                    Religion = ((Religion)profile.Religion).ToString(),
                    Profession = ((Profession)profile.Profession).ToString(),
                    Zodiac = ((Zodiac)profile.Zodiac).ToString(),
                    Images = images,
                    Languages = profile.Languages.Select(x => x.Language).ToList(),
                    Interests = profile.Interests.Select(x => x.Interest).ToList(),
                    Bio = profile.Bio,
                    Work = profile.Work,
                };

                var response = new BaseResponse<ProfileDto>();
                response.Data = partialProfile;
                return response;
            }
            else
            {
                throw new AppException("No access to full chat profile");
            }
        }

        public async Task<BaseResponse<ProfileDto>> GetProfileAsync(int userId, User user)
        {
            if(userId != user.Id)
            {
                throw new Core.Exceptions.UnauthorizedAccessException("No access");
            }

            var profile = _context.Profiles
                            .Include(x => x.Languages)
                            .Include(x => x.Interests)
                            .Include(x => x.Images)
                            .First(x => x.UserId == user.Id);

            var images = profile.Images.Where(x => x.IsActive && !x.IsBlurred)
                .Select(x => {
                    var blobName = _blobService.GenerateSasToken(x.ImageUrl);
                    return new ImagesDto { Image = blobName, Order = x.Order };
                }).ToList();
            
            var partialProfile = new ProfileDto()
            {
                Name = profile.Name,
                Age = profile.Age,
                Height = profile.Height,
                Gender = ((Gender)profile.Gender).ToString(),
                MaritalStatus = ((MaritalStatus)profile.MaritalStatus).ToString(),
                Education = ((Education)profile.Education).ToString(),
                Kids = ((Kids)profile.Kids).ToString(),
                Alcohol = ((Alcohol)profile.Alcohol).ToString(),
                Smoke = ((Smoke)profile.Smoke).ToString(),
                Religion = ((Religion)profile.Religion).ToString(),
                Profession = ((Profession)profile.Profession).ToString(),
                Zodiac = ((Zodiac)profile.Zodiac).ToString(),
                Images = images,
                Languages = profile.Languages.Select(x => x.Language).ToList(),
                Interests = profile.Interests.Select(x => x.Interest).ToList(),
            };

            var response = new BaseResponse<ProfileDto>();
            response.Data = partialProfile;
            return response;
        }

        public async Task<BaseResponse<bool>> UpdateImages(IFormFile imgFile, User user)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var profile = _context.Profiles.First(x => x.UserId == user.Id);
                var images = profile.Images.ToList();
      
                using (var stream = imgFile.OpenReadStream())
                {
                    var fileId = await _blobService.UploadAsync(stream);
                    var newImage = new Image
                    {
                        ProfileId = profile.Id,
                        Profile = profile,
                        ImageUrl = fileId + ".jpg",
                        Order = images.Count() + 1,
                    };
                    profile.Images.Add(newImage);
                }
                
                await _context.SaveChangesAsync();
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Data = false;
                response.Status = (int)HttpStatusCode.InternalServerError;
            }

            return response;

        }

        public async Task<BaseResponse<bool>> DeleteImageAsync(string fileId, User user)
        {
            var response = new BaseResponse<bool>();
            var profile = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (profile != null)
            {
                var image = await _context.Images.FirstOrDefaultAsync(x => x.ImageUrl == fileId && x.ProfileId == profile.Id);
                image.IsActive = false;
                await _context.SaveChangesAsync();
                response.Data = true;
                return response;
            }
            response.Data = false;
            return response;
        }

        public async Task<BaseResponse<bool>> UpdateImageOrder(List<int> newOrder, User user)
        {
            var response = new BaseResponse<bool>();
            var profile = await _context.Profiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var images = _context.Images.Where(x => x.ProfileId == profile.Id && x.IsActive)
                .OrderBy(x => x.Order).ToList();
            
            if (images != null && images.Count() > 0 )
            {
                foreach (var order in newOrder.Select((value, i) => new { i, value }))
                {
                    if(images.ElementAtOrDefault(order.i) != null)
                    {
                        images[order.i].Order = order.value;
                    }
                }

                await _context.SaveChangesAsync();
                response.Data = true;
                return response;
            }
            response.Data = false;
            return response;
        }

    }
}
