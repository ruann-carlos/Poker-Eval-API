using System.Data;
using PokerAPI.Models;

namespace PokerAPI.Services.Evaluation{

    public class EvaluationService : IEvaluationService
    {
        private const int NUM_SUITS_IN_DECK = 4;
        private const int NUM_VALUES_IN_DECK = 13;
        private const int NUM_CARDS_IN_HAND = 5;
        private const int TEN_CARD_POSITION = 8;
        private long RANK_BASE_VALUE = (long) Math.Pow(10, 9);
        private long rankValue = 0;
        private int firstCardIndex = 0;

        public EvaluationService(){}

        /// <summary>
        /// Evaluates a poker hand and determines its rank based on the hand's cards.
        /// </summary>
        /// <param name="hand">The poker hand to evaluate.</param>
        /// <returns>The result of the hand evaluation as a PokerHandResult.</returns>
        public async Task<PokerHandResult> EvaluateHand(PokerHand hand){
            if(hand.Cards == null){
                throw new InvalidOperationException("Invalid cards on Hand");
            }
            List<int> suits = new List<int>(Enumerable.Repeat(0, NUM_SUITS_IN_DECK));
            List<int> cards = new List<int>(Enumerable.Repeat(0, NUM_VALUES_IN_DECK));
            
            CountSuitsAndCards(hand, suits, cards);

            rankValue = GetRankValue(cards);
            firstCardIndex = cards.FindIndex(index => index == 0);
            Dictionary<string, bool> handRanks = GetHandRankMatches(suits, cards);
            int rankIndex = 0;
            string rankDescription = "";
            foreach (var kvp in handRanks.Reverse())
            {
                if (kvp.Value == true)
                {
                    rankIndex = 10 - Array.IndexOf(handRanks.Keys.ToArray(), kvp.Key);
                    rankDescription = kvp.Key;
                }
            }
            rankValue += rankIndex * RANK_BASE_VALUE;
            rankDescription = FormatRankDescription(rankDescription);

            return new PokerHandResult
            {
                PlayerNumber = hand.HandNumber,
                RankValue = rankValue,
                Ranks = handRanks,
                RankDescription = rankDescription,
            };
  
        }

        /// <summary>
        /// Calculates the rank value of a set of cards based on their occurrences.
        /// </summary>
        /// <param name="cards">A list of integers representing card occurrences.</param>
        /// <returns>The calculated rank value for the given card occurrences.</returns>
        public long GetRankValue(List<int> cards){
            long rank = cards.Select((val, index) =>{
                if(val == 1){
                    return (int)Math.Pow(2, index + 1);
                }else if(val > 1){
                    return (int)Math.Pow(2, index + 1) * val;
                }
                return 0;
            }).Sum();

            return rank;
        }

        /// <summary>
        /// Determines the possible hand ranks for a set of suits and cards.
        /// </summary>
        /// <param name="suits">A list of integers representing suit occurrences.</param>
        /// <param name="cards">A list of integers representing card occurrences.</param>
        /// <returns>A dictionary containing the possible hand ranks and their boolean values.</returns>
        public Dictionary<string, bool> GetHandRankMatches(List<int> suits, List<int> cards){
            Dictionary<string, bool> ranks = new Dictionary<string, bool>
            {
                { "royal_flush", false },
                { "straight_flush", false },
                { "quads", cards.Any(count => count == 4) },
                { "full_house", cards.Where(count => count != 0).Count() == 2},
                { "flush", suits.Any(count => count == NUM_CARDS_IN_HAND) },
                { "straight", cards.Skip(firstCardIndex).Take(NUM_CARDS_IN_HAND).Count(count => count == 1) == NUM_CARDS_IN_HAND},
                { "trips", cards.Any(count => count == 3) },
                { "two_pairs", cards.Count(count => count == 2) == 2 },
                { "pair", cards.Count(count => count == 2) == 1 },
                { "high_card", true }
            };

            ranks["straight_flush"] = ranks["flush"] && ranks["straight"];
            ranks["royal_flush"] = ranks["straight_flush"] && firstCardIndex == TEN_CARD_POSITION;
            return ranks;
        }


        /// <summary>
        /// Evaluates a list of poker hands and determines the winner based on their ranks.
        /// </summary>
        /// <param name="hands">A list of poker hands to evaluate.</param>
        /// <returns>The result of the poker table evaluation as a PokerTableResult.</returns>
        public async Task<PokerTableResult> EvaluateHands(List<PokerHand> hands)
        {
            List<PokerHandResult> results = new List<PokerHandResult>();
            foreach(PokerHand hand in hands){
                results.Add(await EvaluateHand(hand));
            }
            int winner = results.OrderByDescending(hand => hand.RankValue).FirstOrDefault().PlayerNumber;
            PokerTableResult OverallResult = new PokerTableResult(results, winner);
            return OverallResult;
        }

        /// <summary>
        /// Counts the occurrences of suits and cards in a poker hand and updates the corresponding lists.
        /// </summary>
        /// <param name="hand">The poker hand to process.</param>
        /// <param name="suits">A list representing the count of suits.</param>
        /// <param name="cards">A list representing the count of cards.</param>
        /// <exception cref="InvalidOperationException">Thrown when the hand's cards are null.</exception>
        private void CountSuitsAndCards(PokerHand hand, List<int> suits, List<int> cards){
            if(hand.Cards == null){
                throw new InvalidOperationException("Invalid cards on Hand");
            }
            foreach(var card in hand.Cards){
                suits[card.Suit] ++;
                cards[card.CardIndex % NUM_VALUES_IN_DECK] ++;
            }
        }

        /// <summary>
        /// Formats a rank description string by capitalizing the first letter of each word.
        /// </summary>
        /// <param name="rankDescription">The rank description to format.</param>
        /// <returns>The formatted rank description.</returns>
        private string FormatRankDescription(string rankDescription)
        {
            return string.Join(" ", rankDescription.Split("_")
                .Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        }
    }
}