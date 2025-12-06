namespace Infrastructure.InternalApiCalls.ResourceApi
{
    public class InternalResourceApiFilterDto
    {
        public string? Name { get; set; }
        public IEnumerable<string>? Type { get; set; }
        public int? Location { get; set; }
        public bool? IsAvailable { get; set; }
        public decimal? MininumPrice { get; set; }
        public decimal? MaxinumPrice { get; set; }
    }
}