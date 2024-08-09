using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Z1.Auth.Models;
using Z1.Core;
using Z1.Core.Data;
using Z1.Core.Interfaces;
using Z1.Match.Dtos;
using Z1.Match.Interfaces;
using Z1.Match.Models;

namespace Z1.Match
{
    public class MatchService : IMatchService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<MatchHub> _hubContext;
        private readonly IBlobService _blobService;


        public MatchService(AppDbContext dbContext, IHubContext<MatchHub> hubContext, IBlobService blobService) {
            _context = dbContext;
            _hubContext = hubContext;
            _blobService = blobService;
        }

        public async Task<BaseResponse<FiltersDto>> GetFilters(User user)
        {
            var filter = await _context.Filters.FirstOrDefaultAsync(fp => fp.UserId == user.Id);

            IDictionary<string, IEnumerable<int>> filters = new Dictionary<string, IEnumerable<int>>();

            var filterResponse = new FiltersDto()
            {
                MinAge = filter.MinAge,
                MaxAge = filter.MaxAge,
                MaxDistance = filter.MaxDistance,
                MinHeight = filter.MinHeight,
                MaxHeight = filter.MaxHeight,
                isProfessionMandatory = filter.isProfessionMandatory,
                isAlcoholMandatory = filter.isAlcoholMandatory,
                isEducationMandatory = filter.isEducationMandatory,
                isInterestsMandatory = filter.isInterestsMandatory,
                isKidsMandatory = filter.isKidsMandatory,
                isLanguagesMandatory = filter.isLanguagesMandatory,
                isMaritalStatusMandatory = filter.isMaritalStatusMandatory,
                isReligionMandatory = filter.isReligionMandatory,
                isSmokeMandatory = filter.isSmokeMandatory,
                isZodiacMandatory = filter.isZodiacMandatory,
                ShowMe = _context.ShowMeFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                RelationshipType = _context.RelationshipTypeFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Profession = _context.ProfessionFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Education = _context.EducationFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Religion = _context.ReligionFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Languages = _context.LanguageFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                MaritalStatus = _context.MaritalStatusFilter.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Kids = _context.KidsFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Alcohol = _context.AlcoholFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Smoke = _context.SmokeFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Zodiac = _context.ZodiacFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
                Interests = _context.InterestFilters.Where(x => x.FilterId == filter.Id).Select(x => x.Value).ToList(),
            };

            var response = new BaseResponse<FiltersDto>();
            response.Status = 200;
            response.Data = filterResponse;
            return response;
        }

        public async Task<BaseResponse<bool>> SaveFilterPreferenceAsync(User user, FiltersDto filtersRequestDto)
        {
            var filter = await _context.Filters.FirstAsync(fp => fp.UserId == user.Id);

            if (filtersRequestDto.MinAge != null)
                filter.MinAge = filtersRequestDto.MinAge;
            if (filtersRequestDto.MaxAge != null)
                filter.MaxAge = filtersRequestDto.MaxAge;
            if (filtersRequestDto.MinHeight != null)
                filter.MinHeight = filtersRequestDto.MinHeight;
            if (filtersRequestDto.MinHeight != null)
                filter.MaxHeight = filtersRequestDto.MaxHeight;
            if (filtersRequestDto.MaxDistance != null)
                filter.MaxDistance = filtersRequestDto.MaxDistance;
            if (filtersRequestDto.isAlcoholMandatory != null)
                filter.isAlcoholMandatory = (bool)filtersRequestDto.isAlcoholMandatory;
            if (filtersRequestDto.isSmokeMandatory != null)
                filter.isSmokeMandatory = (bool)filtersRequestDto.isSmokeMandatory;
            if (filtersRequestDto.isEducationMandatory != null)
                filter.isEducationMandatory = (bool)filtersRequestDto.isEducationMandatory;
            if (filtersRequestDto.isInterestsMandatory != null)
                filter.isInterestsMandatory = (bool)filtersRequestDto.isInterestsMandatory;
            if (filtersRequestDto.isKidsMandatory != null)
                filter.isKidsMandatory = (bool)filtersRequestDto.isKidsMandatory;
            if (filtersRequestDto.isMaritalStatusMandatory != null)
                filter.isMaritalStatusMandatory = (bool)filtersRequestDto.isMaritalStatusMandatory;
            if (filtersRequestDto.isZodiacMandatory != null)
                filter.isZodiacMandatory = (bool)filtersRequestDto.isZodiacMandatory;
            if (filtersRequestDto.isProfessionMandatory != null)
                filter.isProfessionMandatory = (bool)filtersRequestDto.isProfessionMandatory;
            if (filtersRequestDto.isReligionMandatory != null)
                filter.isReligionMandatory = (bool)filtersRequestDto.isReligionMandatory;

            if (filtersRequestDto.ShowMe != null)
            {
                var showMeFilters = _context.ShowMeFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.ShowMeFilters.RemoveRange(showMeFilters);

                foreach (var showMe in filtersRequestDto.ShowMe)
                {
                    _context.ShowMeFilters.Add(new ShowMeFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = showMe
                    });
                }
            }

            if (filtersRequestDto.RelationshipType != null)
            {
                var relationshipTypeFilters = _context.RelationshipTypeFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.RelationshipTypeFilters.RemoveRange(relationshipTypeFilters);

                foreach (var relationshipType in filtersRequestDto.RelationshipType)
                {
                    _context.RelationshipTypeFilters.Add(new RelationshipTypeFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = relationshipType
                    });
                }
            }

            if (filtersRequestDto.Alcohol != null)
            {
                var alcoholFilters = _context.AlcoholFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.AlcoholFilters.RemoveRange(alcoholFilters);

                foreach (var alcohol in filtersRequestDto.Alcohol)
                {
                    _context.AlcoholFilters.Add(new AlcoholFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = alcohol
                    });
                }
            }

            if (filtersRequestDto.Smoke != null)
            {
                var smokeFilters = _context.SmokeFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.SmokeFilters.RemoveRange(smokeFilters);

                foreach (var smoke in filtersRequestDto.Smoke)
                {
                    _context.SmokeFilters.Add(new SmokeFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = smoke
                    });
                }
            }

            if (filtersRequestDto.MaritalStatus != null)
            {
                var maritalStatusFilters = _context.MaritalStatusFilter.Where(x => x.FilterId == filter.Id).ToList();
                _context.MaritalStatusFilter.RemoveRange(maritalStatusFilters);

                foreach (var maritalStatus in filtersRequestDto.MaritalStatus)
                {
                    _context.MaritalStatusFilter.Add(new MaritalStatusFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = maritalStatus
                    });
                }
            }

            if (filtersRequestDto.Kids != null)
            {
                var kidsFilters = _context.KidsFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.KidsFilters.RemoveRange(kidsFilters);

                foreach (var kids in filtersRequestDto.Kids)
                {
                    _context.KidsFilters.Add(new KidsFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = kids
                    });
                }
            }

            if (filtersRequestDto.Profession != null)
            {
                var professionFilters = _context.ProfessionFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.ProfessionFilters.RemoveRange(professionFilters);

                foreach (var profession in filtersRequestDto.Profession)
                {
                    _context.ProfessionFilters.Add(new ProfessionFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = profession
                    });
                }
            }

            if (filtersRequestDto.Languages != null)
            {
                var languageFilters = _context.LanguageFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.LanguageFilters.RemoveRange(languageFilters);

                foreach (var language in filtersRequestDto.Languages)
                {
                    _context.LanguageFilters.Add(new LanguagesFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = language
                    });
                }
            }

            if (filtersRequestDto.Religion != null)
            {
                var religionFilters = _context.ReligionFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.ReligionFilters.RemoveRange(religionFilters);

                foreach (var religion in filtersRequestDto.Religion)
                {
                    _context.ReligionFilters.Add(new ReligionFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = religion
                    });
                }
            }

            if (filtersRequestDto.Education != null)
            {
                var educationFilters = _context.EducationFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.EducationFilters.RemoveRange(educationFilters);

                foreach (var education in filtersRequestDto.Education)
                {
                    _context.EducationFilters.Add(new EducationFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = education
                    });
                }
            }

            if (filtersRequestDto.Zodiac != null)
            {
                var zodiacFilters = _context.ZodiacFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.ZodiacFilters.RemoveRange(zodiacFilters);

                foreach (var zodiac in filtersRequestDto.Zodiac)
                {
                    _context.ZodiacFilters.Add(new ZodiacFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = zodiac
                    });
                }
            }

            if (filtersRequestDto.Interests != null)
            {
                var interestFilters = _context.InterestFilters.Where(x => x.FilterId == filter.Id).ToList();
                _context.InterestFilters.RemoveRange(interestFilters);

                foreach (var interest in filtersRequestDto.Interests)
                {
                    _context.InterestFilters.Add(new InterestsFilter
                    {
                        Filter = filter,
                        FilterId = filter.Id,
                        Value = interest
                    });
                }
            }

            await _context.SaveChangesAsync();

            var response = new BaseResponse<bool>();
            response.Data = true;
            return response;
        }

        public async Task<bool> Search()
        {
            var isPremiumUser = true;
            var notMatched = true;

            var timeNow = DateTime.UtcNow;
            var noMatchUsers = _context.MatchQueue.Where(user => user.RequestTime.AddMinutes(5) < timeNow && user.Tries > 0);

            if (noMatchUsers.Count() > 0)
            {
                foreach (var noMatchUser in noMatchUsers)
                {
                    await _hubContext.Clients.User(noMatchUser.Id.ToString()).SendAsync("MatchNotFound", noMatchUser.Id.ToString());
                    _context.MatchQueue.Remove(noMatchUser);
                    _context.SaveChanges();
                }
            }

            var allUsers = await _context.MatchQueue.ToListAsync();

            if (allUsers.Count < 2)
            {
                noMatchUsers = _context.MatchQueue.Where(user => user.RequestTime.AddMinutes(5) < timeNow);

                if (noMatchUsers.Count() > 0)
                {
                    await _hubContext.Clients.User(noMatchUsers.First().Id.ToString()).SendAsync("MatchNotFound", noMatchUsers.First().Id.ToString());
                    _context.MatchQueue.Remove(noMatchUsers.First());
                    _context.SaveChanges();
                }

                return false;
            }

            foreach (var user in allUsers)
            {
                var userProfile = _context.Profiles.FirstOrDefault(x => x.UserId == user.UserId);
                var userFilters = _context.Filters.First(x => x.UserId == user.UserId);
                var userShowMeFilter = _context.ShowMeFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userRelationshipTypeFilter = _context.RelationshipTypeFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userProfessionFilter = _context.ProfessionFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userReligionFilter = _context.ReligionFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userSmokeFilter = _context.SmokeFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userAlcoholFilter = _context.AlcoholFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userMaritalStatusFilter = _context.MaritalStatusFilter.Where(x => x.FilterId == userFilters.Id).ToList();
                var userKidsFilter = _context.KidsFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userEducationFilter = _context.EducationFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userZodiacFilter = _context.ZodiacFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userLanguageFilter = _context.LanguageFilters.Where(x => x.FilterId == userFilters.Id).ToList();
                var userInterestFilter = _context.InterestFilters.Where(x => x.FilterId == userFilters.Id).ToList();

                var usersNearby = _context.MatchQueue
                                  .Where(s => s.UserId != user.UserId &
                                  s.Location.IsWithinDistance(user.Location, (double)userFilters.MaxDistance * 1000));

                if (usersNearby.Any())
                {
                    var allMatches = new Dictionary<int, MatchQueue>();

                    foreach (var match in usersNearby)
                    {
                        var score = 100;
                        var matchProfile = _context.Profiles.First(x => x.UserId == match.UserId);
                        var matchFilters = _context.Filters.First(x => x.UserId == match.UserId);
                        var matchShowMeFilter = _context.ShowMeFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchRelationshipTypeFilter = _context.RelationshipTypeFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchProfessionFilter = _context.ProfessionFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchReligionFilter = _context.ReligionFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchSmokeFilter = _context.SmokeFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchAlcoholFilter = _context.AlcoholFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchMaritalStatusFilter = _context.MaritalStatusFilter.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchKidsFilter = _context.KidsFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchEducationFilter = _context.EducationFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchZodiacFilter = _context.ZodiacFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchLanguageFilter = _context.LanguageFilters.Where(x => x.FilterId == matchFilters.Id).ToList();
                        var matchInterestFilter = _context.InterestFilters.Where(x => x.FilterId == matchFilters.Id).ToList();

                        if (matchProfile.Age > userFilters.MaxAge || matchProfile.Age < userFilters.MinAge
                            || userProfile.Age > matchFilters.MaxAge || userProfile.Age < matchFilters.MinAge)
                            continue;

                        if (!userShowMeFilter.Any(x => x.Value == matchProfile.Gender) || !matchShowMeFilter.Any(x => x.Value == userProfile.Gender))
                            continue;

                        //relationship check        
                        if (!userRelationshipTypeFilter.Any(x => matchRelationshipTypeFilter.Select(y => y.Value).Contains(x.Value)))
                            score -= 20;

                        var filterscore = filterScorer(userProfile.Profession, userFilters.isProfessionMandatory, userProfessionFilter, matchProfile.Profession, matchFilters.isProfessionMandatory, matchProfessionFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        filterscore = filterScorer(userProfile.Religion, userFilters.isReligionMandatory, userReligionFilter, matchProfile.Religion, matchFilters.isReligionMandatory, matchReligionFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        filterscore = filterScorer(userProfile.Alcohol, userFilters.isAlcoholMandatory, userAlcoholFilter, matchProfile.Alcohol, matchFilters.isAlcoholMandatory, matchAlcoholFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        filterscore = filterScorer(userProfile.Smoke, userFilters.isSmokeMandatory, userSmokeFilter, matchProfile.Smoke, matchFilters.isSmokeMandatory, matchSmokeFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        filterscore = filterScorer(userProfile.Kids, userFilters.isKidsMandatory, userKidsFilter, matchProfile.Kids, matchFilters.isKidsMandatory, matchKidsFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        var userLanguageIds = userProfile.Languages.Select(x => x.Id).ToList();
                        var matchLanguageIds = matchProfile.Languages.Select(x => x.Id).ToList();
                        filterscore = filterListScorer(userLanguageIds, userFilters.isLanguagesMandatory, userLanguageFilter, matchLanguageIds, matchFilters.isLanguagesMandatory, matchLanguageFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        filterscore = filterScorer(userProfile.Education, userFilters.isEducationMandatory, userEducationFilter, matchProfile.Education, matchFilters.isEducationMandatory, matchEducationFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        filterscore = filterScorer(userProfile.MaritalStatus, userFilters.isMaritalStatusMandatory, userMaritalStatusFilter, matchProfile.MaritalStatus, matchFilters.isMaritalStatusMandatory, matchMaritalStatusFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        filterscore = filterScorer(userProfile.Zodiac, userFilters.isZodiacMandatory, userZodiacFilter, matchProfile.Zodiac, matchFilters.isZodiacMandatory, matchZodiacFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        var userIntrestIds =  userProfile.Interests.Select(x => x.Id).ToList();
                        var matchIntrestIds = matchProfile.Interests.Select(x => x.Id).ToList();
                        filterscore = filterListScorer(userIntrestIds, userFilters.isInterestsMandatory, userInterestFilter, matchIntrestIds, matchFilters.isInterestsMandatory, matchInterestFilter);

                        if (filterscore == -1)
                        {
                            continue;
                        }
                        else
                        {
                            score += filterscore;
                        }

                        allMatches.Add(score, match);
                    }

                    if (allMatches.Count == 0)
                    {
                        user.Tries += 1;
                        await _context.SaveChangesAsync();
                        return false;
                    }

                    var matchUser = allMatches.MaxBy(x => x.Key >= 100);

                    if (matchUser.Key >= 100)
                    {
                        var match = new Matches()
                        {
                            User1Id = user.UserId,
                            User2Id = matchUser.Value.UserId,
                            IsPartial = true,
                            IsActive = true,
                        };

                        await _context.Matches.AddAsync(match);

                        _context.MatchQueue.Remove(user);
                        _context.MatchQueue.Remove(matchUser.Value);
                        await _context.SaveChangesAsync();

                        await _hubContext.Clients.User(user.UserId.ToString()).SendAsync("MatchFound", matchUser.Value.UserId.ToString(), match.Id);
                        await _hubContext.Clients.User(matchUser.Value.UserId.ToString()).SendAsync("MatchFound", user.UserId.ToString(), match.Id);

                        return true;
                    }
                    else
                    {
                        user.Tries += 1;

                    }

                    await _context.SaveChangesAsync();

                }
                else
                {
                    user.Tries += 1;
                    await _context.SaveChangesAsync();
                }
            }

            return false;
        }

        public async Task<BaseResponse<bool>> Like(User user, int matchId)
        {
            var currentMatch = _context.Matches.FirstOrDefault(x => x.Id == matchId && x.Skipped == 0);
            BaseResponse<bool> response = new();
            response.Data = false;

            if (currentMatch != null){
                if (currentMatch.User1Id == user.Id)
                {
                    if (currentMatch.User2Liked == true)
                    {
                        currentMatch.IsPartial = false;
                        currentMatch.User1Liked = true;
                        currentMatch.User1LikedTime = DateTime.Now;
                        await _hubContext.Clients.User(currentMatch.User2Id.ToString()).SendAsync("Match");
                        await _hubContext.Clients.User(currentMatch.User1Id.ToString()).SendAsync("Match");
                        response.Data = true;
                    }
                    else
                    {
                        currentMatch.User1Liked = true;
                        currentMatch.User1LikedTime = DateTime.Now;
                        await _hubContext.Clients.User(currentMatch.User2Id.ToString()).SendAsync("UserLiked", user.Id);
                        response.Data = true;
                    }
                }
                else if (currentMatch.User2Id == user.Id)
                {
                    if(currentMatch.User1Liked == true)
                    {
                        currentMatch.IsPartial = false;
                        currentMatch.User2Liked = true;
                        currentMatch.User2LikedTime = DateTime.Now;
                        await _hubContext.Clients.User(currentMatch.User2Id.ToString()).SendAsync("Match");
                        await _hubContext.Clients.User(currentMatch.User1Id.ToString()).SendAsync("Match");
                        response.Data = true;
                    }
                    else
                    {
                        currentMatch.User2Liked = true;
                        currentMatch.User2LikedTime = DateTime.Now;
                        await _hubContext.Clients.User(currentMatch.User1Id.ToString()).SendAsync("UserLiked");
                        response.Data = true;
                    }
                }
            }
             _context.SaveChanges();
             return response;
        }

        public async Task<BaseResponse<bool>> Skip(User user, int matchId)
        {
            var currentMatch = _context.Matches.FirstOrDefault(x => x.Id == matchId && x.Skipped == 0);
            BaseResponse<bool> response = new();

            if (currentMatch != null)
            {
                currentMatch.Skipped = user.Id; 
                currentMatch.IsActive = false;
                await _context.SaveChangesAsync();

                await _hubContext.Clients.User(currentMatch.User1Id.ToString()).SendAsync("Skip");
                await _hubContext.Clients.User(currentMatch.User2Id.ToString()).SendAsync("Skip");
                response.Data = true;
            }
            else
            {
                response.Data = false;
            }

            return response;
        }

        public BaseResponse<CurrentMatchDto> GetCurrentMatch(User user)
        {
            var currentMatch = _context.Matches.FirstOrDefault(x => (x.User1Id == user.Id || x.User2Id == user.Id) 
            && x.IsPartial && x.IsActive && x.CreatedAt.AddDays(1) > DateTime.Now );

            var response = new BaseResponse<CurrentMatchDto>();

            if (currentMatch != null) {
                var matchUserId = currentMatch.User1Id == user.Id ? currentMatch.User2Id : currentMatch.User1Id;

                var matchProfile = _context.Profiles.Include(x => x.Images).FirstOrDefault(x => x.UserId == matchUserId);

                if (matchProfile != null) {
                    response.Data = new CurrentMatchDto()
                    {
                        MatchId = currentMatch.Id,
                        MatchUserId = matchUserId,
                        ProfilePic = _blobService.GenerateSasToken(matchProfile.Images.First().ImageUrl),
                        CreatedAt = currentMatch.CreatedAt
                    };

                    return response;
                }

                return response;

            }

            return response;
        }

        //helper

        public async Task<bool> AddToMatchQueue(string userId, string latitude, string longitude)
        {
            if (!_context.MatchQueue.Any(x => x.UserId.ToString() == userId))
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

                var lati = double.Parse(latitude);
                var longi = double.Parse(longitude);

                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var location = geometryFactory.CreatePoint(new Coordinate(longi, lati));


                var addUser = new MatchQueue() { UserId = user.Id, User = user, RequestTime = DateTime.UtcNow, Location = location };
                _context.MatchQueue.Add(addUser);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;    
        }

        static int filterScorer(int userDetail, bool userIsMandatory, IEnumerable<FilterModel> userFilterModel, int matchDetail, bool matchIsMandatory, IEnumerable<FilterModel> matchFilterModel)
        {
            var points = 0;

            if (userFilterModel.Count() > 0)
            {
                if (userIsMandatory)
                {
                    if (userFilterModel.Any(x => x.Value == matchDetail))
                    {
                        if (userFilterModel.Count() > 0)
                        {
                            if (matchIsMandatory)
                            {
                                // user filter set mandatory - match filter set mandatory
                                if (userFilterModel.Any(x => x.Value == matchDetail))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points = -1;
                                }

                            }
                            else
                            {
                                // user filter set mandatory - match filter set
                                if (matchFilterModel.Any(x => x.Value == userDetail))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points += 10;
                                }
                            }
                        }
                        else
                        {
                            // user filter set mandatory - match filter not set
                            points += 10;
                        }
                    }
                    else
                    {
                        points = -1;
                    }
                }
                else
                {
                    if (matchFilterModel.Count() > 0)
                    {
                        if (matchIsMandatory)
                        {
                            // user filter set - match filter set mandatory
                            if (matchFilterModel.Any(x => x.Value == userDetail))
                            {
                                if (userFilterModel.Any(x => x.Value == matchDetail))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points += 10;
                                }
                            }
                            else
                            {
                                points = -1;
                            }
                        }
                        else
                        {
                            // user filter set - match filter set
                            if (userFilterModel.Any(x => x.Value == matchDetail))
                            {

                                if (userFilterModel.Any(x => x.Value == userDetail))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points += 10;
                                }
                            }
                            else
                            {
                                if (matchFilterModel.Any(x => x.Value == userDetail))
                                {
                                    points += 10;
                                }
                            }

                        }
                    }
                    else
                    {
                        // user filter set - match fiters not set
                        if (matchFilterModel.Any(x => x.Value == matchDetail))
                        {
                            points += 10;
                        }

                    }

                }
            }
            else
            {
                if (matchFilterModel.Count() > 0)
                {
                    if (matchIsMandatory)
                    {
                        // user filter not set - match filter set mandatory
                        if (matchFilterModel.Any(x => x.Value == userDetail))
                        {
                            points += 20;
                        }
                        else
                        {
                            points = -1;
                        }
                    }
                    else
                    {
                        // user filter not set - match filter set
                        if (matchFilterModel.Any(x => x.Value == userDetail))
                        {
                            points += 10;
                        }
                    }
                }
            }

            return points;
        }

        static int filterListScorer(List<int> userDetail, bool userIsMandatory, IEnumerable<FilterModel> userFilterModel, List<int> matchDetail, bool matchIsMandatory, IEnumerable<FilterModel> matchFilterModel)
        {
            var points = 0;

            if (userFilterModel.Count() > 0)
            {
                if (userIsMandatory)
                {
                    if (userFilterModel.Any(x => matchDetail.Contains(x.Value)))
                    {
                        if (userFilterModel.Count() > 0)
                        {
                            if (matchIsMandatory)
                            {
                                // user filter set mandatory - match filter set mandatory
                                if (userFilterModel.Any(x => matchDetail.Contains(x.Value)))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points = -1;
                                }

                            }
                            else
                            {
                                // user filter set mandatory - match filter set
                                if (matchFilterModel.Any(x => userDetail.Contains(x.Value)))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points += 10;
                                }
                            }
                        }
                        else
                        {
                            // user filter set mandatory - match filter not set
                            points += 10;
                        }
                    }
                    else
                    {
                        points = -1;
                    }
                }
                else
                {
                    if (matchFilterModel.Count() > 0)
                    {
                        if (matchIsMandatory)
                        {
                            // user filter set - match filter set mandatory
                            if (matchFilterModel.Any(x => userDetail.Contains(x.Value)))
                            {
                                if (userFilterModel.Any(x => matchDetail.Contains(x.Value)))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points += 10;
                                }
                            }
                            else
                            {
                                points = -1;
                            }
                        }
                        else
                        {
                            // user filter set - match filter set
                            if (userFilterModel.Any(x => matchDetail.Contains(x.Value)))
                            {

                                if (userFilterModel.Any(x => userDetail.Contains(x.Value)))
                                {
                                    points += 20;
                                }
                                else
                                {
                                    points += 10;
                                }
                            }
                            else
                            {
                                if (matchFilterModel.Any(x => userDetail.Contains(x.Value)))
                                {
                                    points += 10;
                                }
                            }

                        }
                    }
                    else
                    {
                        // user filter set - match fiters not set
                        if (matchFilterModel.Any(x => matchDetail.Contains(x.Value)))
                        {
                            points += 10;
                        }

                    }

                }
            }
            else
            {
                if (matchFilterModel.Count() > 0)
                {
                    if (matchIsMandatory)
                    {
                        // user filter not set - match filter set mandatory
                        if (matchFilterModel.Any(x => userDetail.Contains(x.Value)))
                        {
                            points += 20;
                        }
                        else
                        {
                            points = -1;
                        }
                    }
                    else
                    {
                        // user filter not set - match filter set
                        if (matchFilterModel.Any(x => userDetail.Contains(x.Value)))
                        {
                            points += 10;
                        }
                    }
                }
            }

            return points;
        }
    }
}
