using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreeChess.Data;
using ThreeChess.DTOs;
using ThreeChess.Enums;
using ThreeChess.Interfaces;
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


    }
}
