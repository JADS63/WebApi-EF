using Dto;
using Stub;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PlayerServiceStub : IPlayerService
    {
        private readonly List<PlayerDto> _players;

        public PlayerServiceStub()
        {
            _players = StubTennis.GetPlayers();
        }

        public IEnumerable<PlayerDto> GetPlayers()
        {
            return _players;
        }

        public IEnumerable<PlayerDto> GetPlayers(int index, int count)
        {
            return _players.Skip(index).Take(count);
        }
    }
}
