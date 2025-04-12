using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Data;
using ThreeChess.Enums;
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
        private AppDbContext _appContext;
        private static int _id = 0;

        public BoardValuesController(
            BoardCreateService boardCreateService, 
            MoveLogicalElementsService moveElementsService,
            AppDbContext appContext)
        {
            _boardCreateService = boardCreateService;
            _moveElementsService = moveElementsService;
            _appContext = appContext;
        }


        //[HttpGet("cells-location")]
        //public IActionResult GetCellsLocation()
        //{
        //    var cells = _boardCreateService.CreateBoardCells();


        //    return Ok(cells);
        //}

        //[HttpGet("figures-location")]
        //public IActionResult GetCells()
        //{
        //    var figures = _boardCreateService.CreateFigures();

        //    //figures = new Dictionary<string, FigureInfo>();

        //    return Ok(figures);
        //}

        //[HttpGet("diagonals")]
        //public IActionResult GetDiagonals()
        //{
        //    // diag = { "A1", "A2" }
        //    List<List<string>> diagonals = _moveElementsService.GetDiagonals();

        //    return Ok(diagonals);
        //}

        //[HttpGet("main-lines")]
        //public IActionResult GetMainLines()
        //{
        //    // diag = { "A1", "A2" }
        //    List<List<string>> lines = _moveElementsService.GetMainLines();

        //    return Ok(lines);
        //}

        //[HttpGet("secondary-lines")]
        //public IActionResult GetSecondaryLines()
        //{
        //    // diag = { "A1", "A2" }
        //    List<List<string>> lines = _moveElementsService.GetSecondaryLines();

        //    return Ok(lines);
        //}

        [HttpGet("game-config")]
        public IActionResult GetGameConfig()
        {
            // diag = { "A1", "A2" }
            GameConfig gameConfig = new GameConfig
            {
                Color = GetFigureColor(),
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
