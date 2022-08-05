namespace GoingInfiniteBlazor.Models
{
    public class DraftTournament
    {
        public int ID { get; set; }
        public string LGS { get; set; }
        public DateTime Date { get; set; }
        public string PlayerID { get; set; }
        public string SetCode { get; set; }
        public int? DeckID { get; set; }
        public List<Match> Matches  { get; set; }
    }
}
