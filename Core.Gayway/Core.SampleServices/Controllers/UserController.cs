using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Core.SampleServices.Controllers
{
    [Route("[controller]/[action]")]
    public class UserController : BaseController
    {
        public UserController(ILogger<UserController> logger) : base(logger) { }

        [HttpGet]
        public IEnumerable<string> GetArgs()
        {
            string[] args = Program.StartArgs;
            foreach (var arg in args)
            {
                LogInfo(arg);
            }

            return args;
        }
    }
}
