using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using app.Models;

namespace app.Controllers
{

    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;

        public SessionController(ILogger<SessionController> logger)
        {
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task Create([FromBody] Session session)
        {
            Console.WriteLine(session.Title);
        }
    }

}
