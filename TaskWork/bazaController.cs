using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskWork.FUN;

using TaskWork.Models;

namespace TaskWork
{
    [Route("api/[controller]")]
    //[Route("api/ang")]
    [ApiController]
    public class bazaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public bazaController(AppDbContext context)
        {
            _context = context;
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
      
    }


}
