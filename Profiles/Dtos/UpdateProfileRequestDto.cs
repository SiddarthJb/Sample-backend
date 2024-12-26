namespace Z1.Profiles.Dtos
{
    public class UpdateProfileRequestDto
    {
        public int? MaritalStatus { get; set; }
        public int? Kids { get; set; }
        public int? Alcohol { get; set; }
        public int? Smoke { get; set; }
        public int? Religion { get; set; }
        public int? Profession { get; set; }
        public int? Education { get; set; }
        public List<int>? LanguageIds { get; set; }
        public List<int>? InterestIds { get; set; }
        public int? Zodiac { get; set; }
        public string? Bio {  get; set; }
        public string? Work { get; set; }
    }

    public class BulkImageUploadDTO
    {
        public IList<IFormFile> Images { get; set; }
    }
}
