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
        public BoardValuesController(BoardCreateService boardCreateService)
        {
            _boardCreateService = boardCreateService;
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
    }
}
