using Entities;
using Stub;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PlayerServiceStub : IPlayerService
    {
        private readonly List<Player> _players;

        public PlayerServiceStub()
        {
            _players = StubTennis.GetPlayers();
        }

        public IEnumerable<Player> GetPlayers()
        {
            return _players;
        }

        public IEnumerable<Player> GetPlayers(int index, int count)
        {
            return _players.Skip(index).Take(count);
        }
    }
}
