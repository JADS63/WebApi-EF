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
                HandPlay = (Entities.HandPlay)player.HandPlay, 
                Nationality = player.Nationality
            };
        }

        public static Player ToEntity(PlayerDto playerDto)
        {
            if (playerDto == null)
                return null;

            return new Player
            {
                Id = playerDto.Id,
                FirstName = playerDto.FirstName,
                LastName = playerDto.LastName,
                Height = playerDto.Height,
                BirthDate = playerDto.BirthDate,
                HandPlay = (Entities.HandPlay)playerDto.HandPlay, 
                Nationality = playerDto.Nationality
            };
        }
    }
}