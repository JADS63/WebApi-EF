using Dto;
using System.Collections.Generic;

namespace Services
{
    public interface IPlayerService
    {
        IEnumerable<PlayerDto> GetPlayers();
        IEnumerable<PlayerDto> GetPlayers(int index, int count);
    }
}
