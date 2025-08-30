using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (!string.IsNullOrEmpty(dane.ilosc))
            {
                int ilosc = 0;
                string pom=dane.ilosc.ToLower().Trim();
                if (pom.Contains("h"))

                {
                    if (pom.EndsWith("h"))
                        pom += "0";
                    string[] kol = pom.Split("h");
                    if (kol.Length == 2)
                    {
                     
                        ilosc = int.Parse(kol[0].Trim()) * 60 + int.Parse(kol[1].Trim());
                       

                    }
                    else
                    {
                        ilosc = int.Parse(kol[0].Trim());
                    }
                }
                else
                {
                    ilosc = int.Parse(pom);
                }
                    t.TimeEntries = new List<TimeEntry>() { new TimeEntry() { Minutes = ilosc, CreatedAt = classFun.CurrentTimeUTC() } };

            }
           
            _context.Tasks.Add(t);
            _context.SaveChanges();

            return new JsonResult(1);
        }
        //deleteTasktime
        [HttpPost("deleteTasktime")]
        public async Task<ActionResult<int>> deleteTasktime([FromBody] int val)
        {
            int k = val;
            var task = await _context.TimeEntries.FirstOrDefaultAsync(t => t.Id == val);

            if (task != null)
            {
               
                _context.TimeEntries.Remove(task);

                await _context.SaveChangesAsync();
            }
            return new JsonResult(1);
        }
        // Czwarta metoda GET zwracająca listę Job
        [HttpPost("deleteTask")]
        public async Task<ActionResult<int>> deleteTask([FromBody] int val)
        {
            int k = val;
            var task = await _context.Tasks
    .Include(t => t.TimeEntries)
    .FirstOrDefaultAsync(t => t.Id == val);

            if (task != null)
            {
                _context.TimeEntries.RemoveRange(task.TimeEntries);
                _context.Tasks.Remove(task);

                await _context.SaveChangesAsync();
            }
            return new JsonResult(1);
        }
        //addtime
        // Czwarta metoda GET zwracająca listę Job
        [HttpPost("addtime")]
        public async Task<ActionResult<int>> addtime([FromBody] AddTaskTime dane)
        {


         
            TimeEntry te = new TimeEntry();

            int ilosc = 0;
            string pom = dane.ilosc.ToLower().Trim();
            
            if (pom.Contains("h"))

            {
                if(pom.EndsWith("h"))
                    pom+= "0";
                string[] kol = pom.Split("h");
                if (kol.Length == 2)
                {
                    ilosc = int.Parse(kol[0].Trim()) * 60 + int.Parse(kol[1].Trim());

                }
                else
                {
                    ilosc = int.Parse(kol[0].Trim());
                }
            }
            else
                ilosc = int.Parse(pom);
            te.Minutes = ilosc;
            te.TaskItemId = dane.taskid;
            _context.TimeEntries.Add(te);
            _context.SaveChanges();

            return new JsonResult(1);
        }
    }


}
