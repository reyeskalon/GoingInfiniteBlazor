using Microsoft.AspNetCore.Mvc;
using GoingInfiniteAPI.Models;
using GoingInfiniteAPI.DAO;

namespace GoingInfiniteAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentDAO tournamentDAO = new TournamentSqlDAO();
        [HttpPost]
        public IActionResult NewTournament(DraftTournament tourney)
        {
            try
            {
                tourney = tournamentDAO.NewTournament(tourney);
                return Ok(tourney);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTournament(int id)
        {
            try
            {
                DraftTournament tourney = tournamentDAO.GetTournament(id);
                return Ok(tourney);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
