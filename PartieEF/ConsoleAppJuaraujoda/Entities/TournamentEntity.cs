namespace Entities
{
    public class TournamentEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }

        public ICollection<ResultEntity> Results { get; set; } = new List<ResultEntity>();
    }
}