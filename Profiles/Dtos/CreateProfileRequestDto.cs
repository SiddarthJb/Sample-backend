namespace Z1.Profiles.Dtos
{
    public class CreateProfileRequestDto
    {
        public required string Name { get; set; }
        public required string Dob { get; set; }
        public required int Gender { get; set; }
        public required int ShowMe { get; set; }
        public required int RelationshipType { get; set; }
        public required int MaritalStatus { get; set; }
        public required int Kids { get; set; }
        public required int Alcohol { get; set; }
        public required int Smoke { get; set; }
        public required int Religion { get; set; }
        public required int Profession { get; set; }
        public required int Education { get; set; }
        public required List<int> LanguageIds { get; set; }
        public required List<int> InterestIds { get; set; }
        public required int Zodiac { get; set; }
        //public required Point Location { get; set; }
    }
}
