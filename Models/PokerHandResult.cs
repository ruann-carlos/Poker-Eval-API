namespace PokerAPI.Models{
    public class PokerHandResult
    {
        public PokerHand Hand;
        public int PlayerNumber { get; set; }
        public long RankValue { get; set; }
        public Dictionary<string, bool> Ranks { get; set; }
        public string RankDescription { get; set; }
    }
}
