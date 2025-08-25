using Microsoft.AspNetCore.Mvc;
using TaskWork.FUN;
using TaskWork.ModelApi;
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

        // Czwarta metoda GET zwracająca listę Job
        [HttpPost("update")]
        public async Task<ActionResult<int>> update([FromBody] DaneUpdate dane)
        {

            classFun.opuznienie(3);
         


            return new JsonResult(1);
        }

        // Czwarta metoda GET zwracająca listę Job
        [HttpPost("addtask")]
        public async Task<ActionResult<int>> addtask([FromBody] AddTask dane)
        {


            TaskItem t = new TaskItem();
            t.CompanyId = dane.companyid;
            t.Title = dane.name;
            if (dane.ilosc > 0)
            {
                t.TimeEntries = new List<TimeEntry>() { new TimeEntry() { Minutes = dane.ilosc, CreatedAt=classFun.CurrentTimeUTC() } };

            }
           
            _context.Tasks.Add(t);
            _context.SaveChanges();

            return new JsonResult(1);
        }
    }


}
