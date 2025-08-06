namespace Application.Schemas.Responses
{
    public class RequestForResponseDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string RequestedBy { get; set; }
    }
}