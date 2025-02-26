using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Linq;
using WtaApi.Mappers;
using Dto;
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

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            try
            {
                var players = _playerService.GetPlayers();
                if (players == null || !players.Any())
                    return NotFound(new { error = "Aucun joueur trouvé." });
                var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
                var response = new ApiResponse<List<PlayerDto>>
                {
                    result = playersDto,
                    id = 1,
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
            catch (ArgumentNullException argEx)
            {
                return BadRequest(new { error = "Erreur de paramètre : " + argEx.Message });
            }
            catch (InvalidOperationException invOpEx)
            {
                return StatusCode(500, new { error = "Erreur opérationnelle : " + invOpEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erreur interne du serveur : " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetPlayers([FromQuery] PaginationDto pagination)
        {
            if (pagination.Index < 0 || pagination.Count <= 0)
                return BadRequest(new { error = "Index ou count invalide. L'index doit être >= 0 et count > 0." });
            try
            {
                var players = _playerService.GetPlayers(pagination.Index, pagination.Count);
                if (players == null || !players.Any())
                    return NotFound(new { error = "Aucun joueur trouvé pour les paramètres donnés." });
                var playersDto = players.Select(p => PlayerMapper.ToDto(p)).ToList();
                var response = new ApiResponse<List<PlayerDto>>
                {
                    result = playersDto,
                    id = 2,
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
            catch (ArgumentException argEx)
            {
                return BadRequest(new { error = "Erreur de paramètre : " + argEx.Message });
            }
            catch (InvalidOperationException invOpEx)
            {
                return StatusCode(500, new { error = "Erreur opérationnelle : " + invOpEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erreur interne du serveur : " + ex.Message });
            }
        }
    }
}
