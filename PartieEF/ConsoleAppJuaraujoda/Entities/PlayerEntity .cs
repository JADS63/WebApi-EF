namespace Entities
{
    public class PlayerEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public float Height { get; set; }
        public string Nationality { get; set; }
        public HandplayEntity Handplay { get; set; }

        public ICollection<ResultEntity> Results { get; set; } = new List<ResultEntity>();
    }
}