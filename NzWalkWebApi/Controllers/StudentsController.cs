using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NzWalkWebApi.Controllers
{
    // https://localhost:portnum/api/atudent
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudent()
        {
            string[] student = new string[] { "Manohar", "Pawan", "Suman" };
            return Ok(student);
        }



    }
}
