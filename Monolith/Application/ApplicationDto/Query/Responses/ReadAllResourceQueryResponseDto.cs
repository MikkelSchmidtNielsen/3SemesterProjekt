namespace Application.ApplicationDto.Query.Responses
{
    public class ReadAllResourceQueryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Location { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
