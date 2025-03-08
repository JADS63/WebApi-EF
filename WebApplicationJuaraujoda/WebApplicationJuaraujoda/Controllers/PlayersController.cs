using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Entities;
using System.Threading.Tasks;
using System.Linq; // Important pour .Select()
using Dto;
using WtaApi.Mappers;

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
        /// Récupère une liste paginée de joueurs.
        /// </summary>
        /// <param name="index">L'index de la page (commence à 0).</param>
        /// <param name="count">Le nombre de joueurs par page.</param>
        /// <param name="sort">Le critère de tri (voir l'enum SortCriteria).</param>
        /// <returns>Une réponse paginée contenant la liste des joueurs (DTOs).</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlayers([FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] Entities.SortCriteria sort = Entities.SortCriteria.ByNameThenFirstName)
        {
            _logger.LogInformation("GetPlayers appelé avec Index={Index}, Count={Count}, Sort={Sort}", index, count, sort);
            var players = await _playerService.GetPlayersAsync(index, count, sort);

            var playerDtos = players.Select(p => PlayerMapper.ToDto(p)).ToList();

            var paginatedResponse = new PaginatedResponseDto<PlayerDto>
            {  
                Items = playerDtos,
                TotalCount = await _playerService.GetTotalCountAsync(), 
                PageIndex = index,
                CountPerPage = count
            };
            return Ok(paginatedResponse); 
        }


        /// <summary>
        /// Récupère un joueur par son ID.
        /// </summary>
        /// <param name="id">L'ID du joueur.</param>
        /// <returns>Le joueur (DTO) correspondant à l'ID, ou 404 si non trouvé.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPlayerById(int id)
        {
            _logger.LogInformation("GetPlayerById appelé pour id={id}", id);
            var player = await _playerService.GetPlayerByIdAsync(id);
            if (player == null)
            {
                _logger.LogWarning("Aucun joueur trouvé pour id={id}", id);
                return NotFound(); 
            }
            return Ok(PlayerMapper.ToDto(player)); 
        }

        /// <summary>
        /// Ajoute un nouveau joueur.
        /// </summary>
        /// <param name="playerDto">Le DTO du joueur à ajouter.</param>
        /// <returns>Le joueur créé (DTO), avec son ID.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPlayer([FromBody] PlayerDto playerDto)
        {
            _logger.LogInformation("AddPlayer appelé");
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState); 
            }

            var player = PlayerMapper.ToEntity(playerDto); 
            var addedPlayer = await _playerService.AddPlayerAsync(player);

            if (addedPlayer == null)
            {
                _logger.LogError("L'ajout du joueur a échoué.");
                return StatusCode(500); 
            }

            _logger.LogInformation("Joueur ajouté avec id={id}", addedPlayer.Id);
            return CreatedAtAction(nameof(GetPlayerById), new { id = addedPlayer.Id }, PlayerMapper.ToDto(addedPlayer));
        }

        /// <summary>
        /// Met à jour un joueur existant.
        /// </summary>
        /// <param name="id">L'ID du joueur à mettre à jour.</param>
        /// <param name="playerDto">Le DTO du joueur avec les données mises à jour.</param>
        /// <returns>Le joueur mis à jour (DTO), ou 404 si non trouvé, ou 400 si l'ID dans l'URL ne correspond pas à l'ID dans le corps.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerDto playerDto)
        {
            _logger.LogInformation("UpdatePlayer appelé pour id={id}", id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerDto.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            var player = PlayerMapper.ToEntity(playerDto); 
            var updatedPlayer = await _playerService.UpdatePlayerAsync(id, player);

            if (updatedPlayer == null)
            {
                _logger.LogError("La mise à jour du joueur id={id} a échoué.", id);
                return NotFound();
            }

            _logger.LogInformation("Joueur id={id} mis à jour.", id);
            return Ok(PlayerMapper.ToDto(updatedPlayer)); 
        }

        /// <summary>
        /// Supprime un joueur.
        /// </summary>
        /// <param name="id">L'ID du joueur à supprimer.</param>
        /// <returns>204 No Content si la suppression réussit, 404 si le joueur n'est pas trouvé.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            _logger.LogInformation("DeletePlayer appelé pour id={id}", id);
            bool success = await _playerService.DeletePlayerAsync(id);
            if (!success)
            {
                _logger.LogError("La suppression du joueur id={id} a échoué.", id);
                return NotFound(); 
            }
            _logger.LogInformation("Joueur id={id} supprimé.", id);
            return NoContent(); 
        }

        /// <summary>
        /// Récupère le nombre total de joueurs.
        /// </summary>
        /// <returns>Le nombre total de joueurs.</returns>
        [HttpGet("TotalCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalCount()
        {
            _logger.LogInformation("GetTotalCount appelé");
            int count = await _playerService.GetTotalCountAsync();
            return Ok(count);
        }

        /// <summary>
        /// Récupère le nombre total de joueurs dont le nom contient une chaîne de caractères donnée.
        /// </summary>
        /// <param name="name">La chaîne de caractères à rechercher.</param>
        /// <returns>Le nombre total de joueurs correspondant.</returns>
        [HttpGet("TotalCountByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalCountByName([FromQuery] string name)
        {
            _logger.LogInformation("GetTotalCountByName appelé pour name={name}", name);
            int count = await _playerService.GetTotalCountByNameAsync(name);
            return Ok(count);
        }

        /// <summary>
        /// Récupère le nombre total de joueurs d'une nationalité donnée.
        /// </summary>
        /// <param name="nationality">La nationalité à rechercher.</param>
        /// <returns>Le nombre total de joueurs correspondant.</returns>
        [HttpGet("TotalCountByNationality")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTotalCountByNationality([FromQuery] string nationality)
        {
            _logger.LogInformation("GetTotalCountByNationality appelé pour nationality={nationality}", nationality);
            int count = await _playerService.GetTotalCountByNationalityAsync(nationality);
            return Ok(count);
        }

        /// <summary>
        /// Récupère une liste paginée de joueurs dont le nom contient une chaîne de caractères donnée.
        /// </summary>
        /// <param name="name">La chaîne de caractères à rechercher.</param>
        /// <param name="index">L'index de la page.</param>
        /// <param name="count">Le nombre de joueurs par page.</param>
        /// <param name="sort">Le critère de tri.</param>
        /// <returns>Une réponse paginée contenant la liste des joueurs.</returns>
        [HttpGet("byName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlayersByName([FromQuery] string name, [FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] Entities.SortCriteria sort = Entities.SortCriteria.ByNameThenFirstName)
        {
            _logger.LogInformation("GetPlayersByName appelé pour name={name}, Index={index}, Count={count}, Sort={sort}", name, index, count, sort);
            var players = await _playerService.GetPlayersByNameAsync(name, index, count, sort);
            var playerDtos = players.Select(p => PlayerMapper.ToDto(p)).ToList();
            var paginatedResponse = new PaginatedResponseDto<PlayerDto> { Items = playerDtos, TotalCount = await _playerService.GetTotalCountByNameAsync(name), PageIndex = index, CountPerPage = count };
            return Ok(paginatedResponse);
        }

        /// <summary>
        /// Récupère une liste paginée de joueurs d'une nationalité donnée.
        /// </summary>
        /// <param name="nationality">La nationalité à rechercher.</param>
        /// <param name="index">L'index de la page.</param>
        /// <param name="count">Le nombre de joueurs par page.</param>
        /// <param name="sort">Le critère de tri.</param>
        /// <returns>Une réponse paginée contenant la liste des joueurs.</returns>
        [HttpGet("byNationality")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPlayersByNationality([FromQuery] string nationality, [FromQuery] int index = 0, [FromQuery] int count = 10, [FromQuery] Entities.SortCriteria sort = Entities.SortCriteria.ByNameThenFirstName)
        {
            _logger.LogInformation("GetPlayersByNationality appelé pour nationality={nationality}, Index={index}, Count={count}, Sort={sort}", nationality, index, count, sort);
            var players = await _playerService.GetPlayersByNationalityAsync(nationality, index, count, sort);
            var playerDtos = players.Select(p => PlayerMapper.ToDto(p)).ToList();
            var paginatedResponse = new PaginatedResponseDto<PlayerDto> { Items = playerDtos, TotalCount = await _playerService.GetTotalCountByNationalityAsync(nationality), PageIndex = index, CountPerPage = count };
            return Ok(paginatedResponse);
        }
    }
}