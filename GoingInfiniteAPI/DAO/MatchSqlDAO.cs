using System.Data.SqlClient;
using GoingInfiniteAPI.Models;
using GoingInfiniteAPI.Constants;

namespace GoingInfiniteAPI.DAO
{
    public class MatchSqlDAO : IMatchDAO
    {
        private readonly string connectionString = Constants.Constants.DB_CONNECTION_STR;
        private readonly IGameDAO gameDAO = new GameSqlDAO();

        public Match NewMatch(Match match)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO matches (opponent, outcome) OUTPUT INSERTED.match_id VALUES(@opponent, @outcome)", conn);
                cmd.Parameters.AddWithValue("@opponent", match.OpponentName);
                cmd.Parameters.AddWithValue("@outcome", match.Outcome);

                match.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }

            foreach(Game game in match.Games)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO games_in_match (game_id, match_id) VALUES (@game_id, @match_id)", conn);
                    cmd.Parameters.AddWithValue("@game_id", game.ID);
                    cmd.Parameters.AddWithValue("@match_id", match.ID);

                    cmd.ExecuteNonQuery();
                }
            }

            return match;
        }

        public Match GetMatch(int id)
        {
            Match match = new Match();
            List<int> gameIDs = new List<int>();
            List<Game> games = new List<Game>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM matches WHERE match_id = @match_id", conn);
                cmd.Parameters.AddWithValue("@match_id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    match = CreateMatchFromReader(reader);
                }

                reader.Close();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM games_in_match WHERE match_id = @match_id", conn);
                cmd.Parameters.AddWithValue("@match_id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    gameIDs.Add(Convert.ToInt32(reader["game_id"]));
                }
            }

            foreach (int gameID in gameIDs)
            {
                games.Add(gameDAO.GetGame(gameID));
            }

            match.Games = games;

            return match;
        }

        private Match CreateMatchFromReader(SqlDataReader reader)
        {
            Match match = new Match()
            {
                ID = Convert.ToInt32(reader["match_id"]),
                OpponentName = Convert.ToString(reader["opponent"]),
                Outcome = Convert.ToString(reader["outcome"])
            };
            return match;
        }
    }
}
