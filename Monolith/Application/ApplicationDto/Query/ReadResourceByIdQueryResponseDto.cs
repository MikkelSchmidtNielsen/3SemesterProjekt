namespace Application.ApplicationDto.Query
{
    public class ReadResourceByIdQueryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public bool IsAvailable { get; set; }
        public int Location { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
