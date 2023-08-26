using PokerAPI.enums;

namespace PokerAPI.Models
{
    [Serializable]
    public class PokerHand
    {
        public int HandNumber {get; private set;}
        public List<Card>? Cards { get; private set; }

        public PokerHand(List<Card> cards, int handNumber){
            HandNumber = handNumber;
            Cards = cards;
        }
    }
}