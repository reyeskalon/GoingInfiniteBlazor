using GoingInfiniteAPI.Models;

namespace GoingInfiniteAPI.DAO
{
    public interface IMatchDAO
    {
        public Match NewMatch(Match match);
        public Match GetMatch(int id);
    }
}