using MapleApi.Extentions;
using MapleApi.Service;
using MapleLib;
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
        private readonly INodeService _nodeService;

        public MapleController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpGet]
        public MapleNode GetDefaultNode()
        {
            return _nodeService.BaseNode.ToNode();
        }
    }
}
