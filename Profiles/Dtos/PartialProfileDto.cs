namespace Z1.Profiles.Dtos
{
    public class PartialChatProfileDto
    {
        public string Work { get; set; }
        public string ImageUrl { get; set; }
        public byte Age { get; set; }
        public Int16 Height { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Education { get; set; }
        public string Kids { get; set; }
        public string Zodiac { get; set; }
        public string Alcohol { get; set; }
        public string Smoke { get; set; }
        public string Religion { get; set; }
        public string Profession { get; set; }
        public List<string> Languages { get; set; }
        public List<string> Interests { get; set; }
    }

    public class ProfileDto : PartialChatProfileDto
    {
        public string Name { get; set; }
        public List<ImagesDto> Images { get; set; }
        public string Bio {  get; set; }
    }
}
