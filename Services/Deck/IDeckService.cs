using System.Collections.Generic;
using PokerAPI.Models;

namespace PokerAPI.Services.DeckServ
{
    public interface IDeckService
    {
        public void InitializeDeck();
        public Task<Deck> GetCompleteDeck();
        public Task ShuffleDeck();
        public  Task<PokerHand> GetHand(int numberOfCards);
        public Task<List<PokerHand>> GetHands(int numberOfCards, int numberOfHands);
    }

}