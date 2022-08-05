using Microsoft.AspNetCore.Mvc;
using GoingInfiniteAPI.Models;
using GoingInfiniteAPI.DAO;

namespace GoingInfiniteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameDAO gameDAO = new GameSqlDAO();

        [HttpPost]
        public IActionResult NewGame(Game game)
        {
            try
            {
                game = gameDAO.NewGame(game);
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetGame(int id)
        {
            try
            {
                Game game = gameDAO.GetGame(id);
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
