using Dto;
using Entities;

namespace Extensions
{
    public static class PlayerExtensions
    {
        public static Player ToPlayer(this PlayerDto dto)
        {
            return new Player
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Height = dto.Height,
                Nationality = dto.Nationality
            };
        }

        public static PlayerDto ToPlayerDto(this Player player)
        {
            return new PlayerDto
            {
                Id = player.Id,
                FirstName = player.FirstName,
                LastName = player.LastName,
                BirthDate = player.BirthDate,
                Height = player.Height,
                Nationality = player.Nationality,
                HandPlay = Dto.HandPlay.None 
            };
        }
    }
}
