namespace Application.ApplicationDto.Command.Responses
{
    public class CreateResourceUIResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int Location { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
