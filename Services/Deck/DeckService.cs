using Microsoft.AspNetCore.Http.HttpResults;
using PokerAPI.enums;
using PokerAPI.Models;
using PokerAPI.Services;

namespace PokerAPI.Services.DeckServ{

    public class DeckService : IDeckService
    {
        private Deck? deck; 

        public DeckService(){      
            InitializeDeck();
        }

        /// <summary>
        /// Retrieves the complete deck of cards.
        /// </summary>
        /// <returns>The complete deck of cards.</returns>
        public async Task<Deck> GetCompleteDeck()
        {
            if(deck == null){
                InitializeDeck();
            }
            return deck ?? throw new InvalidOperationException("Deck has not been initialized");
        }

        /// <summary>
        /// Initializes the deck with all the cards in a standard order.
        /// </summary>
        public void InitializeDeck()
        {
            deck = new Deck();
            var rankAmount = Enum.GetValues(typeof(Ranks));
            var suitAmount = Enum.GetValues(typeof(Suits));
            int cardCounter = 0;
            foreach(Suits suit in suitAmount){
                foreach(Ranks rank in rankAmount){
                    deck.AddCardToDeck(new Card((int) suit, (int) rank, cardCounter));
                    cardCounter++;
                }
            }
        }

        /// <summary>
        /// Shuffles the deck to randomize the card order.
        /// </summary>
        public async Task ShuffleDeck()
        {
            deck?.ShuffleDeck();
        }

        public async Task<PokerHand> GetHand(int numberOfCards)
        {
            await ShuffleDeck();
            List<Card> handCards = deck?.GetCardsFromDeck(numberOfCards) ?? throw new InvalidOperationException("Deck is null");
            PokerHand hand = new PokerHand(handCards, 1);
            return hand;
        }

        /// <summary>
        /// Draws a poker hand with the specified number of cards from the deck.
        /// </summary>
        /// <param name="numberOfCards">The number of cards in the poker hand.</param>
        /// <returns>The drawn poker hand.</returns>
        public async Task<List<PokerHand>> GetHands(int numberOfCards, int numberOfHands)
        {
            List<PokerHand> hands = new List<PokerHand>();
            for(int i = 0; i < numberOfHands; i++){
                await ShuffleDeck();
                List<Card> handCards = deck?.GetCardsFromDeck(numberOfCards) ?? throw new InvalidOperationException("Deck is null");
                PokerHand hand = new PokerHand(handCards, i + 1);
                hands.Add(hand);
            }
            return hands;
        }
    }
}