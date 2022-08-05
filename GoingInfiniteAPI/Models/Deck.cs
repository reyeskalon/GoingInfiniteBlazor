namespace GoingInfiniteAPI.Models
{
    public class Deck
    {
        public int ID { get; set; }
        public Dictionary<Card, int> Cards { get; set; }
    }
}
