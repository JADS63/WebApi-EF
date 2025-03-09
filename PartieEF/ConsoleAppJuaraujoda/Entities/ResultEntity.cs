namespace Entities
{
    public class ResultEntity
    {
        public int Id { get; set; }
        public Result Result { get; set; }

        public ICollection<PlayerEntity> Players { get; set; } = new List<PlayerEntity>();
        public ICollection<TournamentEntity> Tournaments { get; set; } = new List<TournamentEntity>();
    }
}