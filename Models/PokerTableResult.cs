namespace PokerAPI.Models{
    public class PokerTableResult
    {
      public List<PokerHandResult> HandResults {get; private set;}

      public string Winner {get; set;}

      public PokerTableResult(List<PokerHandResult> results, int winner){
        HandResults =  results;
        Winner = $"The winner is the player: {winner}";
      }
    }
}
