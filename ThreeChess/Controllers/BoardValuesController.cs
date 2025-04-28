using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Data;
using ThreeChess.Enums;
using ThreeChess.Interfaces;
using ThreeChess.Models;
using ThreeChess.Models.CellElements;
using ThreeChess.Services;

namespace ThreeChess.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class BoardValuesController : ControllerBase
    {
        private IBoardElementsService _boardCreateService;
        private IMoveLogicalElementsService _moveElementsService;
        private AppDbContext _appContext;
        private static int _id = 0;

        public BoardValuesController(
            IBoardElementsService boardCreateService, 
            IMoveLogicalElementsService moveElementsService,
            AppDbContext appContext)
        {
            _boardCreateService = boardCreateService;
            _moveElementsService = moveElementsService;
            _appContext = appContext;
        }

        [HttpGet("game-config")]
        public IActionResult GetGameConfig()
        {
            // diag = { "A1", "A2" }
            InitializingGameDto gameConfig = new InitializingGameDto
            {
                ControlledColor = GetFigureColor(),
                CurrentTurnColor = FigureColor.White,
                CellsLocation = GetBoardCells(),
                FiguresLocation = _boardCreateService.CreateFigures(),
                Diagonals = _moveElementsService.GetDiagonals(),
                MainLines = _moveElementsService.GetMainLines(),
                SecondaryLines = _moveElementsService.GetSecondaryLines()
            };

            return Ok(gameConfig);
        }

        private List<CellItem> GetBoardCells()
        {
            if (_id % 3 == 0)
            {
                _id++;
                return _boardCreateService.CreateBoardCellsForColor(FigureColor.White);
            }

            if (_id % 3 == 1)
            {
                _id++;
                return _boardCreateService.CreateBoardCellsForColor(FigureColor.Black);
            }

            _id++;
            return _boardCreateService.CreateBoardCellsForColor(FigureColor.Red);
            
        }

        private FigureColor GetFigureColor()
        {
            if (_id % 3 == 0)
            {
                return FigureColor.White;
            }

            if (_id % 3 == 1)
            {
                return FigureColor.Black;
            }

            return FigureColor.Red;
        }
    }
}
