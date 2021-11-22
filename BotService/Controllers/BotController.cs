using BotService.Domain;
using chat_application.Infra.CrossCutting.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : Controller
    {
        IMessageBroker broker;

        public BotController(IMessageBroker messageBroker)
        {
            broker = messageBroker;
        }
        [HttpPost]
        public IActionResult Index(string message)
        {
            if (!ModelState.IsValid) return BadRequest();

            broker.Insert(message);

            return Accepted();
        }
    }
}
