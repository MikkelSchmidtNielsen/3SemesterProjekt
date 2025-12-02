namespace Application.ApplicationDto.Query
{
    public class ResourceFilterDto
    {
        public string? Name { get; set; }
        public IEnumerable<string>? Type { get; set; }
        public int? Location { get; set; }
        public bool? IsAvailable { get; set; }
        public decimal? MininumPrice { get; set; }
        public decimal? MaxinumPrice { get; set; }
    }
}
