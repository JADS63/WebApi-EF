using Dto;
using Entities;

namespace WtaApi.Mappers
{
    public static class PlayerMapper
    {
        public static PlayerDto ToDto(Player player)
        {
            if (player == null)
                return null;
            return new PlayerDto
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                Height = player.Height,
                BirthDate = player.BirthDate,
                HandPlay = (Dto.HandPlay)(int)player.HandPlay,
                Nationality = player.Nationality
            };
        }

        public static Player ToEntity(PlayerDto dto)
        {
            if (dto == null)
                return null;
            return new Player
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Height = dto.Height,
                BirthDate = dto.BirthDate,
                HandPlay = (Entities.HandPlay)(int)dto.HandPlay,
                Nationality = dto.Nationality
            };
        }
    }
}
