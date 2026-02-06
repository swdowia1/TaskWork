using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskWork.FUN;

using TaskWork.Models;
using TaskWork.Serwices;

namespace TaskWork
{
    [Route("api/[controller]")]
    //[Route("api/ang")]
    [ApiController]
    public class bazaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TagPredictionService _tagPredictionService;

        public bazaController(AppDbContext context, TagPredictionService tagPredictionService)
        {
            _context = context;
            _tagPredictionService = tagPredictionService;
        }



        //// Czwarta metoda GET zwracająca listę Job
        //[HttpPost("addtask")]
        //public async Task<ActionResult<int>> addtask([FromBody] AddTask dane)
        //{



        //    return new JsonResult(1);
        //}
        //deleteTasktime
        [HttpPost("deleteTasktime")]
        public async Task<ActionResult<int>> deleteTasktime([FromBody] int val)
        {
            
            return new JsonResult(1);
        }
        // Czwarta metoda GET zwracająca listę Job
        [HttpPost("deleteTask")]
        public async Task<ActionResult<int>> deleteTask([FromBody] int val)
        {
            
            return new JsonResult(1);
        }

        [HttpPost("predict")]
        public async Task<ActionResult<List<string>>> predict([FromBody] string content)
        {
            var predictedTags = _tagPredictionService.PredictTags(content ?? "");
            return new JsonResult(predictedTags);
        }

    }


}
