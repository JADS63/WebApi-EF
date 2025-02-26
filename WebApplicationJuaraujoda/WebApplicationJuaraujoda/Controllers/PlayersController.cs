using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Entities;
using WtaApi.Mappers;
using Dto;
using System;
using System.Linq;
using System.Collections.Generic;

namespace WebApplicationJuaraujoda.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayersController> _logger;
        private IPlayerService @object;

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
        public IActionResult GetPlayerById(int id)
        {
            _logger.LogInformation("GetPlayerById called with id: {Id}", id);
            var player = _playerService.GetPlayerById(id);
            if (player == null)
            {
                _logger.LogWarning("Player with id {Id} not found.", id);
                return NotFound(new { error = "Player not found." });
            }
            _logger.LogInformation("Player with id {Id} found.", id);
            return Ok(PlayerMapper.ToDto(player));
        }

        /// <summary>
        /// GET /api/v1/players
        /// Si les paramètres pagination (index, count) et criterium sont fournis, retourne une réponse paginée et triée.
        /// Sinon, retourne toutes les joueuses.
        /// Exemple : /api/v1/players?index=0&count=3&criterium=1
        /// </summary>
        [HttpGet]
        public IActionResult GetPlayers([FromQuery] PaginationDto pagination = null, [FromQuery] int? criterium = null)
        {
            if (pagination != null && pagination.Index >= 0 && pagination.Count > 0 && criterium.HasValue)
            {
                var players = _playerService.GetPlayers(pagination.Index, pagination.Count, criterium.Value);
                var total = _playerService.GetTotalCount();
                var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
                var response = new ApiResponse<List<PlayerDto>>
                {
                    id = 2,
                    result = playersDto,
                    exception = null,
                    status = 5,
                    isCanceled = false,
                    isCompleted = true,
                    isCompletedSuccessfully = true,
                    creationOptions = 0,
                    asyncState = null,
                    isFaulted = false
                };
                return Ok(response);
            }
            else
            {
                var players = _playerService.GetPlayers();
                var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
                var response = new ApiResponse<List<PlayerDto>>
                {
                    id = 1,
                    result = playersDto,
                    exception = null,
                    status = 5,
                    isCanceled = false,
                    isCompleted = true,
                    isCompletedSuccessfully = true,
                    creationOptions = 0,
                    asyncState = null,
                    isFaulted = false
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// GET /api/v1/players/byname?name=na&index=0&count=10&criterium=1
        /// Récupère les joueuses dont le prénom ou le nom contient la chaîne spécifiée.
        /// </summary>
        [HttpGet("byname")]
        public IActionResult GetPlayersByName([FromQuery] string name, [FromQuery] int index, [FromQuery] int count, [FromQuery] int criterium)
        {
            _logger.LogInformation("GetPlayersByName called with name: {Name}, index: {Index}, count: {Count}, criterium: {Criterium}", name, index, count, criterium);
            var players = _playerService.GetPlayersByName(name, index, count, criterium);
            var total = _playerService.GetTotalCountByName(name);
            if (players == null || !players.Any())
            {
                _logger.LogWarning("No players found with name containing '{Name}'.", name);
                return NotFound(new { error = "No players found with the specified name." });
            }
            var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
            var response = new PaginatedResponse<PlayerDto>
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
        public IActionResult GetPlayersByNationality([FromQuery] string nationality, [FromQuery] int index, [FromQuery] int count, [FromQuery] int criterium)
        {
            _logger.LogInformation("GetPlayersByNationality called with nationality: {Nationality}, index: {Index}, count: {Count}, criterium: {Criterium}", nationality, index, count, criterium);
            var players = _playerService.GetPlayersByNationality(nationality, index, count, criterium);
            var total = _playerService.GetTotalCountByNationality(nationality);
            if (players == null || !players.Any())
            {
                _logger.LogWarning("No players found with nationality containing '{Nationality}'.", nationality);
                return NotFound(new { error = "No players found with the specified nationality." });
            }
            var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
            var response = new PaginatedResponse<PlayerDto>
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
        public IActionResult PostPlayer([FromBody] PlayerDto playerDto)
        {
            _logger.LogInformation("PostPlayer called.");
            if (playerDto == null)
            {
                _logger.LogWarning("Invalid player data received in PostPlayer.");
                return BadRequest(new { error = "Invalid player data." });
            }
            var playerEntity = PlayerMapper.ToEntity(playerDto);
            var createdPlayer = _playerService.AddPlayer(playerEntity);
            _logger.LogInformation("Player created with id {Id}.", createdPlayer.Id);
            return CreatedAtAction(nameof(GetPlayerById), new { id = createdPlayer.Id }, PlayerMapper.ToDto(createdPlayer));
        }

        /// <summary>
        /// PUT /api/v1/players?id=51
        /// Met à jour une joueuse existante.
        /// </summary>
        [HttpPut]
        public IActionResult PutPlayer([FromQuery] int id, [FromBody] PlayerDto playerDto)
        {
            _logger.LogInformation("PutPlayer called for id {Id}.", id);
            if (playerDto == null)
            {
                _logger.LogWarning("Invalid player data received in PutPlayer.");
                return BadRequest(new { error = "Invalid player data." });
            }
            var updatedPlayer = _playerService.UpdatePlayer(id, PlayerMapper.ToEntity(playerDto));
            if (updatedPlayer == null)
            {
                _logger.LogWarning("Player with id {Id} not found for update.", id);
                return NotFound(new { error = "Player not found." });
            }
            _logger.LogInformation("Player with id {Id} updated successfully.", id);
            return Ok(PlayerMapper.ToDto(updatedPlayer));
        }

        /// <summary>
        /// DELETE /api/v1/players?id=51
        /// Supprime une joueuse existante.
        /// </summary>
        [HttpDelete]
        public IActionResult DeletePlayer([FromQuery] int id)
        {
            _logger.LogInformation("DeletePlayer called for id {Id}.", id);
            var deleted = _playerService.DeletePlayer(id);
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
