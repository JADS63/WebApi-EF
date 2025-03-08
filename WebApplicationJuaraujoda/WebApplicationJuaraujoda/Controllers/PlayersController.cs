using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Entities;
using System.Threading.Tasks;

namespace WebApplicationJuaraujoda.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerService _playerService;
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(IPlayerService playerService, ILogger<PlayersController> logger)
        {
            _playerService = playerService;
            _logger = logger;
        }

        // GET: api/v1/Players?Index=0&Count=10&Sort=0
        [HttpGet]
        public async Task<IActionResult> GetPlayers([FromQuery] int Index = 0, [FromQuery] int Count = 10, [FromQuery] int Sort = 0)
        {
            _logger.LogInformation("GetPlayers appelé avec Index={Index}, Count={Count}, Sort={Sort}", Index, Count, Sort);
            var players = await _playerService.GetPlayersAsync(Index, Count, Sort);
            return Ok(new { result = players });
        }

        // GET: api/v1/Players/42
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            _logger.LogInformation("GetPlayerById appelé pour id={id}", id);
            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null)
            {
                _logger.LogWarning("Aucun joueur trouvé pour id={id}", id);
                return NotFound(new { result = (object)null });
            }
            return Ok(new { result = player });
        }

        // POST: api/v1/Players
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] Player player)
        {
            _logger.LogInformation("AddPlayer appelé");
            var addedPlayer = await _playerService.AddPlayerAsync(player);
            if (addedPlayer == null)
            {
                _logger.LogError("L'ajout du joueur a échoué.");
                return BadRequest(new { result = (object)null });
            }
            _logger.LogInformation("Joueur ajouté avec id={id}", addedPlayer.Id);
            return CreatedAtAction(nameof(GetPlayerById), new { id = addedPlayer.Id }, new { result = addedPlayer });
        }

        // PUT: api/v1/Players?id=50
        [HttpPut]
        public async Task<IActionResult> UpdatePlayer([FromQuery] int id, [FromBody] Player player)
        {
            _logger.LogInformation("UpdatePlayer appelé pour id={id}", id);
            var updatedPlayer = await _playerService.UpdatePlayerAsync(id, player);
            if (updatedPlayer == null)
            {
                _logger.LogError("La mise à jour du joueur id={id} a échoué.", id);
                return BadRequest(new { result = (object)null });
            }
            _logger.LogInformation("Joueur id={id} mis à jour.", id);
            return Ok(new { result = updatedPlayer });
        }

        // DELETE: api/v1/Players/50
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            _logger.LogInformation("DeletePlayer appelé pour id={id}", id);
            bool success = await _playerService.DeletePlayerAsync(id);
            if (!success)
            {
                _logger.LogError("La suppression du joueur id={id} a échoué.", id);
                return BadRequest(new { result = false });
            }
            _logger.LogInformation("Joueur id={id} supprimé.", id);
            return Ok(new { result = true });
        }

        // GET: api/v1/Players/TotalCount
        [HttpGet("TotalCount")]
        public async Task<IActionResult> GetTotalCount()
        {
            _logger.LogInformation("GetTotalCount appelé");
            int count = await _playerService.GetTotalCountAsync();
            return Ok(new { result = count });
        }

        // GET: api/v1/Players/TotalCountByName?name=John
        [HttpGet("TotalCountByName")]
        public async Task<IActionResult> GetTotalCountByName([FromQuery] string name)
        {
            _logger.LogInformation("GetTotalCountByName appelé pour name={name}", name);
            int count = await _playerService.GetTotalCountByNameAsync(name);
            return Ok(new { result = count });
        }

        // GET: api/v1/Players/TotalCountByNationality?nationality=USA
        [HttpGet("TotalCountByNationality")]
        public async Task<IActionResult> GetTotalCountByNationality([FromQuery] string nationality)
        {
            _logger.LogInformation("GetTotalCountByNationality appelé pour nationality={nationality}", nationality);
            int count = await _playerService.GetTotalCountByNationalityAsync(nationality);
            return Ok(new { result = count });
        }

        // GET: api/v1/Players/byName?name=John&Index=0&Count=10&Sort=0
        [HttpGet("byName")]
        public async Task<IActionResult> GetPlayersByName([FromQuery] string name, [FromQuery] int Index = 0, [FromQuery] int Count = 10, [FromQuery] int Sort = 0)
        {
            _logger.LogInformation("GetPlayersByName appelé pour name={name}, Index={Index}, Count={Count}, Sort={Sort}", name, Index, Count, Sort);
            var players = await _playerService.GetPlayersByNameAsync(name, Index, Count, Sort);
            return Ok(new { result = players });
        }

        // GET: api/v1/Players/byNationality?nationality=USA&Index=0&Count=10&Sort=0
        [HttpGet("byNationality")]
        public async Task<IActionResult> GetPlayersByNationality([FromQuery] string nationality, [FromQuery] int Index = 0, [FromQuery] int Count = 10, [FromQuery] int Sort = 0)
        {
            _logger.LogInformation("GetPlayersByNationality appelé pour nationality={nationality}, Index={Index}, Count={Count}, Sort={Sort}", nationality, Index, Count, Sort);
            var players = await _playerService.GetPlayersByNationalityAsync(nationality, Index, Count, Sort);
            return Ok(new { result = players });
        }
    }
}
