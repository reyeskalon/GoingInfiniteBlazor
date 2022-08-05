using Microsoft.AspNetCore.Mvc;
using GoingInfiniteAPI.Models;
using GoingInfiniteAPI.DAO;

namespace GoingInfiniteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchDAO matchDAO = new MatchSqlDAO();
        [HttpPost]
        public IActionResult NewMatch(Match match)
        {
            try
            {
                match = matchDAO.NewMatch(match);
                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetMatch(int id)
        {
            try
            {
                Match match = matchDAO.GetMatch(id);
                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
