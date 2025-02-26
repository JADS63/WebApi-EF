using Entities;
using System.Collections.Generic;

namespace Services
{
    public interface IPlayerService
    {
        IEnumerable<Player> GetPlayers();
        IEnumerable<Player> GetPlayers(int index, int count);
    }
}
