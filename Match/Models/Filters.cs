using Z1.Auth.Models;
using Z1.Core.Data;

namespace Z1.Match.Models
{
    public class Filter : AuditableEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? MinHeight { get; set; }
        public int? MaxHeight { get; set; }
        public int? MaxDistance { get; set; }
        public bool isProfessionMandatory { get; set; }
        public bool isAlcoholMandatory { get; set; }
        public bool isSmokeMandatory { get; set; }
        public bool isLanguagesMandatory { get; set; }
        public bool isMaritalStatusMandatory { get; set; }
        public bool isKidsMandatory { get; set; }
        public bool isReligionMandatory { get; set; }
        public bool isEducationMandatory { get; set; }
        public bool isZodiacMandatory { get; set; }
        public bool isInterestsMandatory { get; set; }
    }

    public class FilterModel
    {
        public int Id { get; set; }
        public int FilterId { get; set; }
        public Filter Filter { get; set; }
        public int Value { get; set; }
    }

    public class ShowMeFilter : FilterModel { }
    public class RelationshipTypeFilter : FilterModel { }
    public class ProfessionFilter: FilterModel { }
    public class ReligionFilter : FilterModel { }
    public class AlcoholFilter : FilterModel { }
    public class SmokeFilter : FilterModel { }
    public class EducationFilter : FilterModel { }
    public class InterestsFilter : FilterModel { }
    public class LanguagesFilter : FilterModel { }
    public class MaritalStatusFilter : FilterModel { }
    public class KidsFilter : FilterModel { }
    public class ZodiacFilter : FilterModel { }
}
