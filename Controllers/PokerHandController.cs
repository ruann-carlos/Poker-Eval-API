using Microsoft.AspNetCore.Mvc;
using PokerAPI.Services.Evaluation;
using PokerAPI.Models;
using PokerAPI.Services.DeckServ;

namespace PokerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokerHandController: ControllerBase
    {
        private const int STANDARD_HAND_COUNT = 5;
        private const int STANDARD_PLAYERS_COUNT = 4;
        private IDeckService deckService = new DeckService();
        private IEvaluationService evaluationService = new EvaluationService();

        public PokerHandController(IDeckService deckService, IEvaluationService evaluationService){
            this.deckService = deckService;
            this.evaluationService = evaluationService;
        }

        /// <summary>
        /// Generates a complete deck of cards.
        /// </summary>
        /// <returns>The generated deck of cards.</returns>
        [HttpGet("generatedeck")]
        public async Task<ActionResult<Deck>> GenerateDeck()
        {
            try{
                var deck = await deckService.GetCompleteDeck();
                if(deck != null){
                    return Ok(deck);
                }
            }catch(Exception err){
                return BadRequest("Error when creating the deck: " + err.Message);
            }
            return NotFound();
        }


        /// <summary>
        /// Gets a single poker hand with the specified number of cards.
        /// </summary>
        /// <param name="numberOfCards">The number of cards in the hand.</param>
        /// <returns>The drawn poker hand.</returns>
        [HttpGet("getsinglehand")]
        public async Task<ActionResult<PokerHand>> GetHand([FromQuery] int numberOfCards){
            try{
                var currentHand = await deckService.GetHand(numberOfCards);
                if(currentHand != null){
                    return Ok(currentHand);
                }
            }catch(Exception err){
                return BadRequest("Error getting hand: " + err.Message);
            }
            return NotFound();
        }


        /// <summary>
        /// Gets multiple poker hands with the specified number of cards each.
        /// </summary>
        /// <param name="numberOfCards">The number of cards in each hand.</param>
        /// <param name="numberOfHands">The number of poker hands to get.</param>
        /// <returns>The list of drawn poker hands.</returns>
        [HttpGet("gethands")]
        public async Task<ActionResult<List<PokerHand>>> GetHands([FromQuery] int numberOfCards, [FromQuery] int numberOfHands){
            try{
                var currentHand = await deckService.GetHands(numberOfCards, numberOfHands);
                if(currentHand != null){
                    return Ok(currentHand);
                }
            }catch(Exception err){
                return BadRequest("Error getting hands: " + err.Message);
            }
            return NotFound();
        }   

        /// <summary>
        /// Shuffles the deck of cards.
        /// </summary>
        /// <returns>A response indicating that the deck has been shuffled.</returns>
        [HttpGet("shuffledeck")]
        public async Task<ActionResult> ShuffleDeck()
        {
            try{
                await deckService.ShuffleDeck();
                return Ok("Deck Shuffled");
            }catch(Exception err){
                return BadRequest("Error shuffling the deck: " + err.Message);
            }
        }

        /// <summary>
        /// Evaluates the rank of a single poker hand.
        /// </summary>
        /// <param name="hand">The poker hand to evaluate.</param>
        /// <returns>The result of the hand evaluation.</returns>
        [HttpPost("evaluatesingle")]
        public async Task<ActionResult<PokerHandResult>> EvaluanteHand([FromBody] PokerHand? hand){
            try{
                PokerHandResult result;
                if(hand == null){
                    PokerHand newHand = await deckService.GetHand(5);
                    result = await evaluationService.EvaluateHand(newHand);
                }else{
                    result = await evaluationService.EvaluateHand(hand);
                }
                return Ok(result);
            }catch(Exception err){
                 return BadRequest("Error evaluating the hand rank: " + err);
            }
        }

        /// <summary>
        /// Evaluates the ranks of multiple poker hands.
        /// </summary>
        /// <param name="hands">The list of poker hands to evaluate.</param>
        /// <returns>The result of the hands evaluation.</returns>
        [HttpPost("evaluatehands")]
        public async Task<ActionResult<PokerTableResult>> EvaluateHands([FromBody] List<PokerHand>? hands){
            try{
                PokerTableResult evaluationResult;
                if(hands == null){
                    List<PokerHand> Hands = await deckService.GetHands(STANDARD_HAND_COUNT, STANDARD_PLAYERS_COUNT);
                    evaluationResult = await evaluationService.EvaluateHands(Hands);
                }else{
                    evaluationResult = await evaluationService.EvaluateHands(hands);
                }
                return Ok(evaluationResult);
            }catch(Exception err){
                return BadRequest("Error evaluating the hands: " + err);
            }
        }
    }
}
