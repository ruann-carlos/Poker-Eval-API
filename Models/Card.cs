using PokerAPI.enums;

namespace PokerAPI.Models
{
    [Serializable]
    public class Card
    {
        public int Rank {get; private set;}
        public int Suit {get; private set;}
        public int CardIndex {get; private set;}
        public Dictionary<string, string> CardObject {get; private set;}
        public Card(int suit, int rank, int cardIndex){
            Suit = suit;
            Rank = rank;
            CardIndex = cardIndex;

            string suitName = Enum.GetName(typeof(Suits), suit);
            string rankName = Enum.GetName(typeof(Ranks), rank);
            CardObject = new Dictionary<string, string>(){
                {rankName, suitName}
            };
        }
    }
}