namespace Application.Schemas.Responses
{
    public class VehicleForResponseDTO
    {
        public int Id { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public bool IsParked { get; set; }
        public bool IsActive { get; set; }
        public List<RequestForResponseDTO> Requests { get; set; }
    }
}