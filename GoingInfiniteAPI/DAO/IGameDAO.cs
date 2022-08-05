using GoingInfiniteAPI.Models;

namespace GoingInfiniteAPI.DAO
{
    public interface IGameDAO
    {
        public Game GetGame(int id);
        public Game NewGame(Game game);
    }
}
