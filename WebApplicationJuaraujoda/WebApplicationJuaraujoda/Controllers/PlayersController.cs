using Microsoft.AspNetCore.Mvc;
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
        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        /// <summary>
        /// GET /api/v1/players/{id}
        /// Récupère une joueuse par son identifiant.
        /// </summary>
        [HttpGet("{id:int}")]
        public IActionResult GetPlayerById(int id)
        {
            var player = _playerService.GetPlayerById(id);
            if (player == null)
                return NotFound(new { error = "Player not found." });
            return Ok(PlayerMapper.ToDto(player));
        }

        /// <summary>
        /// GET /api/v1/players
        /// Si les paramètres pagination (index, count) et criterium sont fournis,
        /// retourne une réponse paginée et triée.
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
                var response = new PaginatedResponse<PlayerDto>
                {
                    TotalCount = total,
                    PageIndex = pagination.Index,
                    CountPerPage = pagination.Count,
                    Items = playersDto
                };
                return Ok(response);
            }
            else
            {
                var players = _playerService.GetPlayers();
                var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
                return Ok(playersDto);
            }
        }

        /// <summary>
        /// GET /api/v1/players/byname?name=na&index=0&count=10&criterium=1
        /// Récupère les joueuses dont le prénom ou le nom contient la chaîne spécifiée.
        /// </summary>
        [HttpGet("byname")]
        public IActionResult GetPlayersByName([FromQuery] string name, [FromQuery] int index, [FromQuery] int count, [FromQuery] int criterium)
        {
            var players = _playerService.GetPlayersByName(name, index, count, criterium);
            var total = _playerService.GetTotalCountByName(name);
            if (players == null || !players.Any())
                return NotFound(new { error = "No players found with the specified name." });
            var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
            var response = new PaginatedResponse<PlayerDto>
            {
                TotalCount = total,
                PageIndex = index,
                CountPerPage = count,
                Items = playersDto
            };
            return Ok(response);
        }

        /// <summary>
        /// GET /api/v1/players/bynationality?nationality=usa&index=0&count=10&criterium=1
        /// Récupère les joueuses dont la nationalité contient la chaîne spécifiée.
        /// </summary>
        [HttpGet("bynationality")]
        public IActionResult GetPlayersByNationality([FromQuery] string nationality, [FromQuery] int index, [FromQuery] int count, [FromQuery] int criterium)
        {
            var players = _playerService.GetPlayersByNationality(nationality, index, count, criterium);
            var total = _playerService.GetTotalCountByNationality(nationality);
            if (players == null || !players.Any())
                return NotFound(new { error = "No players found with the specified nationality." });
            var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
            var response = new PaginatedResponse<PlayerDto>
            {
                TotalCount = total,
                PageIndex = index,
                CountPerPage = count,
                Items = playersDto
            };
            return Ok(response);
        }

        /// <summary>
        /// POST /api/v1/players
        /// Ajoute une nouvelle joueuse.
        /// </summary>
        [HttpPost]
        public IActionResult PostPlayer([FromBody] PlayerDto playerDto)
        {
            if (playerDto == null)
                return BadRequest(new { error = "Invalid player data." });
            var playerEntity = PlayerMapper.ToEntity(playerDto);
            var createdPlayer = _playerService.AddPlayer(playerEntity);
            return CreatedAtAction(nameof(GetPlayerById), new { id = createdPlayer.Id }, PlayerMapper.ToDto(createdPlayer));
        }

        /// <summary>
        /// PUT /api/v1/players?id=51
        /// Met à jour une joueuse existante.
        /// </summary>
        [HttpPut]
        public IActionResult PutPlayer([FromQuery] int id, [FromBody] PlayerDto playerDto)
        {
            if (playerDto == null)
                return BadRequest(new { error = "Invalid player data." });
            var updatedPlayer = _playerService.UpdatePlayer(id, PlayerMapper.ToEntity(playerDto));
            if (updatedPlayer == null)
                return NotFound(new { error = "Player not found." });
            return Ok(PlayerMapper.ToDto(updatedPlayer));
        }

        /// <summary>
        /// DELETE /api/v1/players?id=51
        /// Supprime une joueuse existante.
        /// </summary>
        [HttpDelete]
        public IActionResult DeletePlayer([FromQuery] int id)
        {
            var deleted = _playerService.DeletePlayer(id);
            if (!deleted)
                return NotFound(new { error = "Player not found." });
            return NoContent();
        }
    }
}
