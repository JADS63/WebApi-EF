using Microsoft.AspNetCore.Mvc;
using Services;
using Stub;
using Dto;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase, IPlayerService
    {
        private List<PlayerDto> _players = Stub.StubTennis.GetPlayers();

        public IEnumerable<IActionResult> GetAll()
        {
            try
            {
                var result = _players;
                if (result == null)
                {
                    return (IEnumerable<IActionResult>)NotFound();
                }
                else
                {
                     return (IEnumerable<IActionResult>)Ok(result);
                }
            }catch (Exception ex)
            {
                return (IEnumerable<IActionResult>)StatusCode(500);
            }

        }

        public IEnumerable<IActionResult> Getplayers(int index, int count)
        {
            try
            {
                var result = _players.FirstOrDefault();
                if (result == null)
                {
                    return (IEnumerable<IActionResult>)NotFound();
                }
                else
                {
                    return (IEnumerable<IActionResult>)Ok(result);
                }
            }
            catch (Exception ex)
            {
                return (IEnumerable<IActionResult>)StatusCode(500);
            }
        }
    }
}
