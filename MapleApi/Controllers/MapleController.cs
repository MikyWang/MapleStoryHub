using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapleController : ControllerBase
    {
        [HttpGet]
        public string GetDefault()
        {
            return "test";
        }
    }
}
