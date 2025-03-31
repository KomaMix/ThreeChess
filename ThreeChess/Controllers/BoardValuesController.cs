using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ThreeChess.Models;
using ThreeChess.Services;

namespace ThreeChess.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BoardValuesController : ControllerBase
    {
        private BoardCreateService _boardCreateService;
        public BoardValuesController(BoardCreateService boardCreateService)
        {
            _boardCreateService = boardCreateService;
        }


        [HttpGet("cells")]
        public IActionResult Get()
        {
            var rightCells = _boardCreateService.CreateBoardCells();


            return Ok(rightCells);
        }
    }
}
