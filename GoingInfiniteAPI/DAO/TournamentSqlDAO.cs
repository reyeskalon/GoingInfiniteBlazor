using System.Data.SqlClient;
using GoingInfiniteAPI.Models;

namespace GoingInfiniteAPI.DAO
{
    public class TournamentSqlDAO : ITournamentDAO
    {
        private readonly string connectionString = Constants.Constants.DB_CONNECTION_STR;
        private readonly IMatchDAO matchDAO = new MatchSqlDAO();

        public DraftTournament NewTournament(DraftTournament tourney)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO draft_tournaments (lgs, tournament_date, player_id, set_code, deck_id) OUTPUT INSERTED.tournament_id VALUES(@lgs, @tournament_date, @player_id, @set_code, @deck_id)", conn);
                cmd.Parameters.AddWithValue("@lgs", tourney.LGS);
                cmd.Parameters.AddWithValue("@tournament_date", tourney.Date);
                cmd.Parameters.AddWithValue("@player_id", tourney.PlayerID);
                cmd.Parameters.AddWithValue("@set_code", tourney.SetCode);
                cmd.Parameters.AddWithValue("@deck_id", tourney.DeckID);

                tourney.ID = Convert.ToInt32(cmd.ExecuteScalar());
            }

            foreach (Match match in tourney.Matches)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO matches_in_tournament (match_id, tournament_id) VALUES (@match_id, @tournament_id)", conn);
                    cmd.Parameters.AddWithValue("@match_id", match.ID);
                    cmd.Parameters.AddWithValue("@tournament_id", tourney.ID);

                    cmd.ExecuteNonQuery();
                }
            }

            return tourney;
        }
        public DraftTournament GetTournament(int id)
        {
            DraftTournament tourney = new DraftTournament();
            
            List<Match> matches = new List<Match>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM draft_tournaments WHERE tournament_id = @tournament_id", conn);
                cmd.Parameters.AddWithValue("@tournament_id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    tourney = CreateTournamentFromReader(reader);
                }

                reader.Close();
            }

            List<int> matchIDs = GetMatchIDs(tourney);

            foreach (int matchID in matchIDs)
            {
                matches.Add(matchDAO.GetMatch(matchID));
            }

            tourney.Matches = matches;

            return tourney;
        }

        public List<DraftTournament> GetPlayersTournaments(string playerID)
        {
            List<DraftTournament> tournaments = new List<DraftTournament>();

            using (SqlConnection conn = new SqlConnection(Constants.Constants.DB_CONNECTION_STR))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM draft_tournaments WHERE player_id = @player_id", conn);
                cmd.Parameters.AddWithValue("@player_id", playerID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tournaments.Add(CreateTournamentFromReader(reader));
                }

                reader.Close();
            }

            foreach (DraftTournament tourney in tournaments)
            {
                List<int> matchIDs = GetMatchIDs(tourney);

                List<Match> matches = new List<Match>();
                foreach (int matchID in matchIDs)
                {
                    matches.Add(matchDAO.GetMatch(matchID));
                }

                tourney.Matches = matches;
            }

            return tournaments;
        }

        private DraftTournament CreateTournamentFromReader(SqlDataReader reader)
        {
            DraftTournament tourney = new DraftTournament()
            {
                ID = Convert.ToInt32(reader["tournament_id"]),
                LGS = Convert.ToString(reader["lgs"]),
                Date = Convert.ToDateTime(reader["tournament_date"]),
                PlayerID = Convert.ToString(reader["player_id"]),
                SetCode = Convert.ToString(reader["set_code"]),

            };
            if (reader["deck_id"] != null)
            {
                tourney.DeckID = Convert.ToInt32(reader["deck_id"]);
            }
            return tourney;
        }

        private List<int> GetMatchIDs(DraftTournament tourney)
        {
            List<int> matchIDs = new List<int>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM matches_in_tournament WHERE tournament_id = @tournament_id", conn);
                cmd.Parameters.AddWithValue("@tournament_id", tourney.ID);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    matchIDs.Add(Convert.ToInt32(reader["match_id"]));
                }
            }

            return matchIDs;
        }
    }
}
