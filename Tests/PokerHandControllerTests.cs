using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using PokerAPI.Controllers;
using PokerAPI.Models;
using PokerAPI.Services.DeckServ;
using PokerAPI.Services.Evaluation;
using PokerAPI.enums;

namespace PokerAPI.Tests{

    public class PokerHandControllerTests
    {
        private Mock<IDeckService> mockDeckService;
        private Mock<IEvaluationService> mockEvaluationService;
        private PokerHandController controller;

        public PokerHandControllerTests()
        {
            mockDeckService = new Mock<IDeckService>();
            mockEvaluationService = new Mock<IEvaluationService>();
            controller = new PokerHandController(mockDeckService.Object, mockEvaluationService.Object);
            controller.ControllerContext = new ControllerContext();
        }

        [Fact]
        public async Task GenerateDeck_ReturnsOk_WhenDeckIsGenerated()
        {
            mockDeckService.Setup(service => service.GetCompleteDeck()).ReturnsAsync(new Deck());
            var controller = new PokerHandController(mockDeckService.Object, mockEvaluationService.Object);
            var result = await controller.GenerateDeck();

            Assert.IsType<ActionResult<Deck>>(result);
        }

        [Fact]
        public async Task EvaluanteHand_WithValidHand_ReturnsOkResult()
        {
            var mockHand = new PokerHand(new List<Card>(), 5);
            mockDeckService.Setup(service => service.GetHand(It.IsAny<int>())).ReturnsAsync(mockHand);
            mockEvaluationService.Setup(service => service.EvaluateHand(mockHand)).ReturnsAsync(new PokerHandResult());
            var result = await controller.EvaluanteHand(mockHand);

            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}

