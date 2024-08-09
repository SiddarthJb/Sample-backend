namespace Z1.Match.Dtos
{
    public class FiltersDto
    {
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? MinHeight { get; set; }
        public int? MaxHeight { get; set; }
        public int? MaxDistance { get; set; }
        public bool? isProfessionMandatory { get; set; }
        public bool? isAlcoholMandatory { get; set; }
        public bool? isSmokeMandatory { get; set; }
        public bool? isLanguagesMandatory { get; set; }
        public bool? isMaritalStatusMandatory { get; set; }
        public bool? isKidsMandatory { get; set; }
        public bool? isReligionMandatory { get; set; }
        public bool? isEducationMandatory { get; set; }
        public bool? isZodiacMandatory { get; set; }
        public bool? isInterestsMandatory { get; set; }
        public List<int>? ShowMe { get; set;}
        public List<int>? RelationshipType { get; set; }
        public List<int>? Profession { get; set; } 
        public List<int>? Education { get; set; }
        public List<int>? MaritalStatus { get; set; }
        public List<int>? Kids { get; set; }
        public List<int>? Religion { get; set; }
        public List<int>? Languages { get; set; }
        public List<int>? Alcohol { get; set; }
        public List<int>? Smoke { get; set; }
        public List<int>? Interests { get; set; }
        public List<int>? Zodiac { get; set; }
    }
}
