using System.Data.SqlClient;
using GoingInfiniteAPI.Models;

namespace GoingInfiniteAPI.DAO
{
    public class GameSqlDAO : IGameDAO
    {
        private readonly string connectionString = "Server=DESKTOP-JUOLNMK;Database=going_infinite_blazor;Trusted_Connection=True;";

        public Game NewGame(Game game)
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO games (outcome) OUTPUT INSERTED.game_id VALUES(@outcome)", conn);
                cmd.Parameters.AddWithValue("@outcome", game.Outcome);

                game.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return game;
        }

        public Game GetGame(int id)
        {
            Game game = new Game();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM games WHERE game_id = @game_id", conn);
                cmd.Parameters.AddWithValue("@game_id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    game = CreateGameFromReader(reader);
                }
            }
            return game;
        }

        private Game CreateGameFromReader(SqlDataReader reader)
        {
            Game game = new Game()
            {
                ID = Convert.ToInt32(reader["game_id"]),
                Outcome = Convert.ToString(reader["outcome"])
            };
            return game;
        }
    }
}
