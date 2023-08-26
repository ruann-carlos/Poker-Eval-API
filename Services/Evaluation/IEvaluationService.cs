using PokerAPI.Models;

namespace PokerAPI.Services.Evaluation{

    public interface IEvaluationService{

        public Task<PokerHandResult> EvaluateHand(PokerHand hand);
        public Task<PokerTableResult> EvaluateHands(List<PokerHand> hands);
        public long GetRankValue(List<int> cards);
        public Dictionary<string, bool> GetHandRankMatches(List<int> suits, List<int> cards);
    }
}