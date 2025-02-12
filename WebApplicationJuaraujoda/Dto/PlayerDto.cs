namespace Dto
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public float Height { get; set; }
        public string? Nationality { get; set; }
        public HandPlay HandPlay { get; set; }

    }
}
