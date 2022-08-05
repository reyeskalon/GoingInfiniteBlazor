namespace GoingInfiniteAPI.Models
{
    public class Match
    {
        public int ID { get; set; }
        public string OpponentName { get; set; }
        public string Outcome { get; set; }
        public List<Game> Games { get; set; }
    }
}
