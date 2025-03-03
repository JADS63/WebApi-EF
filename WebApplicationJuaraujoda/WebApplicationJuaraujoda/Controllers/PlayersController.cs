using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Entities;
using WtaApi.Mappers;
using Dto;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplicationJuaraujoda.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
        {
            _playerService = playerService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/v1/players/{id}
        /// Récupère une joueuse par son identifiant.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            _logger.LogInformation("GetPlayerById called with id: {Id}", id);
            // Récupération de l'entité métier depuis le service
            var playerEntity = await _playerService.GetPlayerByIdAsync(id);
            if (playerEntity == null)
            {
                _logger.LogWarning("Player with id {Id} not found.", id);
                return NotFound(new { error = "Player not found." });
            }
            _logger.LogInformation("Player with id {Id} found.", id);
            // Conversion de l'entité en DTO avant de retourner la réponse
            var playerDto = PlayerMapper.ToDto(playerEntity);
            return Ok(playerDto);
        }

        /// <summary>
        /// GET /api/v1/players
        /// Retourne les joueuses paginées et triées si les paramètres sont fournis, sinon toutes.
        /// Exemple : /api/v1/players?index=0&count=3&criterium=1
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPlayers([FromQuery] PaginationDto pagination = null, [FromQuery] int? criterium = null)
        {
            List<PlayerDto> playersDto;
            int responseId;
            if (pagination != null && pagination.Index >= 0 && pagination.Count > 0 && criterium.HasValue)
            {
                // Récupération paginée et triée via le service (entités)
                var playersEntity = await _playerService.GetPlayersAsync(pagination.Index, pagination.Count, criterium.Value);
                var total = await _playerService.GetTotalCountAsync();
                playersDto = playersEntity.Select(p => PlayerMapper.ToDto(p)).ToList();
                responseId = 2;
                var response = new ApiResponseDto<List<PlayerDto>>
                {
                    Id = responseId,
                    Result = playersDto,
                    Exception = null,
                    Status = 5,
                    IsCanceled = false,
                    IsCompleted = true,
                    IsCompletedSuccessfully = true,
                    CreationOptions = 0,
                    AsyncState = null,
                    IsFaulted = false
                };
                return Ok(response);
            }
            else
            {
                // Récupération de toutes les joueuses (entités)
                var playersEntity = await _playerService.GetPlayersAsync();
                playersDto = playersEntity.Select(p => PlayerMapper.ToDto(p)).ToList();
                responseId = 1;
                var response = new ApiResponseDto<List<PlayerDto>>
                {
                    Id = responseId,
                    Result = playersDto,
                    Exception = null,
                    Status = 5,
                    IsCanceled = false,
                    IsCompleted = true,
                    IsCompletedSuccessfully = true,
                    CreationOptions = 0,
                    AsyncState = null,
                    IsFaulted = false
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// GET /api/v1/players/byname?name=na&index=0&count=10&criterium=1
        /// Récupère les joueuses dont le prénom ou le nom contient la chaîne spécifiée.
        /// </summary>
        [HttpGet("byname")]
        public async Task<IActionResult> GetPlayersByName([FromQuery] string name, [FromQuery] int index, [FromQuery] int count, [FromQuery] int criterium)
        {
            _logger.LogInformation("GetPlayersByName called with name: {Name}, index: {Index}, count: {Count}, criterium: {Criterium}", name, index, count, criterium);
            var playersEntity = await _playerService.GetPlayersByNameAsync(name, index, count, criterium);
            var total = await _playerService.GetTotalCountByNameAsync(name);
            if (playersEntity == null || !playersEntity.Any())
            {
                _logger.LogWarning("No players found with name containing '{Name}'.", name);
                return NotFound(new { error = "No players found with the specified name." });
            }
            var playersDto = playersEntity.Select(p => PlayerMapper.ToDto(p)).ToList();
            var response = new PaginatedResponseDto<PlayerDto>
            {
                TotalCount = total,
                PageIndex = index,
                CountPerPage = count,
                Items = playersDto
            };
            _logger.LogInformation("Returning {Count} players filtered by name, total {TotalCount}.", playersDto.Count, total);
            return Ok(response);
        }

        /// <summary>
        /// GET /api/v1/players/bynationality?nationality=usa&index=0&count=10&criterium=1
        /// Récupère les joueuses dont la nationalité contient la chaîne spécifiée.
        /// </summary>
        [HttpGet("bynationality")]
        public async Task<IActionResult> GetPlayersByNationality([FromQuery] string nationality, [FromQuery] int index, [FromQuery] int count, [FromQuery] int criterium)
        {
            _logger.LogInformation("GetPlayersByNationality called with nationality: {Nationality}, index: {Index}, count: {Count}, criterium: {Criterium}", nationality, index, count, criterium);
            var playersEntity = await _playerService.GetPlayersByNationalityAsync(nationality, index, count, criterium);
            var total = await _playerService.GetTotalCountByNationalityAsync(nationality);
            if (playersEntity == null || !playersEntity.Any())
            {
                _logger.LogWarning("No players found with nationality containing '{Nationality}'.", nationality);
                return NotFound(new { error = "No players found with the specified nationality." });
            }
            var playersDto = playersEntity.Select(p => PlayerMapper.ToDto(p)).ToList();
            var response = new PaginatedResponseDto<PlayerDto>
            {
                TotalCount = total,
                PageIndex = index,
                CountPerPage = count,
                Items = playersDto
            };
            _logger.LogInformation("Returning {Count} players filtered by nationality, total {TotalCount}.", playersDto.Count, total);
            return Ok(response);
        }

        /// <summary>
        /// POST /api/v1/players
        /// Ajoute une nouvelle joueuse.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostPlayer([FromBody] PlayerDto playerDto)
        {
            _logger.LogInformation("PostPlayer called.");
            if (playerDto == null)
            {
                _logger.LogWarning("Invalid player data received in PostPlayer.");
                return BadRequest(new { error = "Invalid player data." });
            }
            // Conversion du DTO en entité pour le traitement métier
            var playerEntity = PlayerMapper.ToEntity(playerDto);
            var createdPlayerEntity = await _playerService.AddPlayerAsync(playerEntity);
            _logger.LogInformation("Player created with id {Id}.", createdPlayerEntity.Id);
            // Conversion de l'entité créée en DTO pour la réponse
            var createdPlayerDto = PlayerMapper.ToDto(createdPlayerEntity);
            return CreatedAtAction(nameof(GetPlayerById), new { id = createdPlayerDto.Id }, createdPlayerDto);
        }

        /// <summary>
        /// PUT /api/v1/players?id=51
        /// Met à jour une joueuse existante.
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> PutPlayer([FromQuery] int id, [FromBody] PlayerDto playerDto)
        {
            _logger.LogInformation("PutPlayer called for id {Id}.", id);
            if (playerDto == null)
            {
                _logger.LogWarning("Invalid player data received in PutPlayer.");
                return BadRequest(new { error = "Invalid player data." });
            }
            // Conversion du DTO en entité pour mise à jour
            var updatedPlayerEntity = await _playerService.UpdatePlayerAsync(id, PlayerMapper.ToEntity(playerDto));
            if (updatedPlayerEntity == null)
            {
                _logger.LogWarning("Player with id {Id} not found for update.", id);
                return NotFound(new { error = "Player not found." });
            }
            _logger.LogInformation("Player with id {Id} updated successfully.", id);
            // Conversion de l'entité mise à jour en DTO pour la réponse
            var updatedPlayerDto = PlayerMapper.ToDto(updatedPlayerEntity);
            return Ok(updatedPlayerDto);
        }

        /// <summary>
        /// DELETE /api/v1/players?id=51
        /// Supprime une joueuse existante.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeletePlayer([FromQuery] int id)
        {
            _logger.LogInformation("DeletePlayer called for id {Id}.", id);
            var deleted = await _playerService.DeletePlayerAsync(id);
            if (!deleted)
            {
                _logger.LogWarning("Player with id {Id} not found for deletion.", id);
                return NotFound(new { error = "Player not found." });
            }
            _logger.LogInformation("Player with id {Id} deleted successfully.", id);
            return NoContent();
        }
    }
}
