using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Data;
using ThreeChess.Enums;
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
        private BoardElementsService _boardCreateService;
        private MoveLogicalElementsService _moveElementsService;
        private AppDbContext _appContext;
        private static int _id = 0;

        public BoardValuesController(
            BoardElementsService boardCreateService, 
            MoveLogicalElementsService moveElementsService,
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
                return _boardCreateService.CreateBoardCellsForWhite();
            }

            if (_id % 3 == 1)
            {
                _id++;
                return _boardCreateService.CreateBoardCellsForBlack();
            }

            _id++;
            return _boardCreateService.CreateBoardCellsForRed();
            
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
