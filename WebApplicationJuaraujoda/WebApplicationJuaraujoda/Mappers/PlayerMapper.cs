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
                // Conversion explicite : on convertit Entities.HandPlay en int puis en Dto.HandPlay.
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
                // Conversion explicite : on convertit Dto.HandPlay en int puis en Entities.HandPlay.
                HandPlay = (Entities.HandPlay)(int)dto.HandPlay,
                Nationality = dto.Nationality
            };
        }
    }
}
