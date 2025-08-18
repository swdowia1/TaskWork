using Microsoft.AspNetCore.Mvc;
using TaskWork.FUN;
using TaskWork.ModelApi;

namespace TaskWork
{
    [Route("api/[controller]")]
    //[Route("api/ang")]
    [ApiController]
    public class bazaController : ControllerBase
    {

        // Czwarta metoda GET zwracająca listę Job
        [HttpPost("update")]
        public async Task<ActionResult<int>> update([FromBody] DaneUpdate dane)
        {

            classFun.opuznienie(3);
         


            return new JsonResult(1);
        }
    }
}
