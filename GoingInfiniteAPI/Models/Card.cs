namespace GoingInfiniteAPI.Models
{
    public class Card
    {
        public string CardId { get; set; }

        public string Name { get; set; }

        public string Rarity { get; set; }

        public string ManaCost { get; set; }

        public double Cmc { get; set; }

        public string TypeLine { get; set; }

        public string OracleText { get; set; }

        public string[] Colors { get; set; }

        public string[] ColorIdentity { get; set; }

        public string[] Keywords { get; set; }

        public string Set { get; set; }

        public string SetName { get; set; }

        public string Image { get; set; }

        public string ArtCrop { get; set; }
    }
}