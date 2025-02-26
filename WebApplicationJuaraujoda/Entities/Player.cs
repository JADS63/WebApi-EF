namespace Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Height { get; set; }
        public DateTime BirthDate { get; set; }
        public HandPlay HandPlay { get; set; }
        public string Nationality { get; set; }
    }
    public enum HandPlay
    {
        None,
        Left,
        Right,
        LeftAndRight
    }
}
