using Microsoft.AspNetCore.Mvc;
using Services;
using Dto;
using System;
using System.Linq;

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
                {
                    return NotFound(new { error = "Aucun joueur trouvé." });
                }
                return Ok(players);
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
        public IActionResult GetPlayers([FromQuery] int index, [FromQuery] int count)
        {
            if (index < 0 || count <= 0)
            {
                return BadRequest(new { error = "Index ou count invalide. L'index doit être >= 0 et count > 0." });
            }

            try
            {
                var players = _playerService.GetPlayers(index, count);
                if (players == null || !players.Any())
                {
                    return NotFound(new { error = "Aucun joueur trouvé pour les paramètres donnés." });
                }
                return Ok(players);
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
