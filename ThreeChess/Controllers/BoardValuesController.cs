using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class BoardValuesController : ControllerBase
    {
        private BoardCreateService _boardCreateService;
        private MoveLogicalElementsService _moveElementsService;
        public BoardValuesController(BoardCreateService boardCreateService, MoveLogicalElementsService moveElementsService)
        {
            _boardCreateService = boardCreateService;
            _moveElementsService = moveElementsService;
        }


        [HttpGet("cells-location")]
        public IActionResult GetCellsLocation()
        {
            var cells = _boardCreateService.CreateBoardCells();


            return Ok(cells);
        }

        [HttpGet("figures-location")]
        public IActionResult GetCells()
        {
            var figures = _boardCreateService.CreateFigures();

            return Ok(figures);
        }

        [HttpGet("diagonals")]
        public IActionResult GetDiagonals()
        {
            // diag = { "A1", "A2" }
            List<List<string>> diagonals = _moveElementsService.GetDiagonals();

            return Ok(diagonals);
        }
    }
}
