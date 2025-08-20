using Microsoft.AspNetCore.Mvc;
using TaskWork.FUN;
using TaskWork.ModelApi;

namespace TaskWork
{


    [Route("api/[controller]")]
    [ApiController]
    public class GetHtmlController : Controller
    {
        [HttpGet("update")]
        public IActionResult Update(int id)
        {
            var model = new Job
            {
                Id = id,
                Name = "Zaktualizowana nazwa " + id
            };

            return PartialView("_JobPartial", model);
        }
    }
}
