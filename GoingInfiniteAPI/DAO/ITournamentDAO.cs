using GoingInfiniteAPI.Models;

namespace GoingInfiniteAPI.DAO
{
    public interface ITournamentDAO
    {
        public DraftTournament NewTournament(DraftTournament tourney);
        public DraftTournament GetTournament(int id);
    }
}
