namespace PokerAPI.Models{

    [Serializable]
    public class Deck{

        const int DECK_STARTER_POSITION = 0;
        public List<Card> DeckList {get; private set;}
        private Random random = new Random();

        public Deck(){
            DeckList = new List<Card>();
        }

        /// <summary>
        /// Adds a card to the deck.
        /// </summary>
        /// <param name="card">The card to be added to the deck.</param>
        /// <exception cref="InvalidOperationException">Thrown when the card is already in the deck.</exception>
        public void AddCardToDeck(Card card){
            if(DeckList.Contains(card)){
                throw new InvalidOperationException("Card already added to deck list");
            }
            DeckList.Add(card);
        }

        /// <summary>
        /// Shuffles the cards in the deck to randomize their order.
        /// </summary>
        public void ShuffleDeck(){
            int count = DeckList.Count;
            while (count > 0)
            {
                count--;
                int randomIndex = random.Next(count + 1);
                var temp = DeckList[randomIndex];
                DeckList[randomIndex] = DeckList[count];
                DeckList[count] = temp;
            }
        }

        /// <summary>
        /// Gets a specified number of cards from the top of the deck.
        /// </summary>
        /// <param name="numberOfCards">The number of cards to retrieve from the deck.</param>
        /// <returns>A list of cards drawn from the deck.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the number of cards is invalid.</exception>
        public List<Card> GetCardsFromDeck(int numberOfCards){
            if(numberOfCards <= 0 || numberOfCards > DeckList.Count){
                throw new ArgumentOutOfRangeException(nameof(numberOfCards), "Invalid number of cards");
            }
            return DeckList.GetRange(DECK_STARTER_POSITION, numberOfCards);
        }
    }
}