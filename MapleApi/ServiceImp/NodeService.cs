using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MapleApi.Service;
using MapleLib;
using Microsoft.Extensions.Logging;
using WzLib;

namespace MapleApi.ServiceImp
{
    public class NodeService : INodeService, IDisposable
    {
        private readonly ILogger<NodeService> logger;
        private readonly string FILEPATH = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"D:\文档\WzFile\Base.wz" : @"/Volumes/数据/MapleStory/Base.wz";
        private readonly Wz_Structure _Structure;

        public Wz_Node BaseNode { get; private set; }

        public NodeService(ILogger<NodeService> _logger)
        {
            logger = _logger;
            _Structure = new Wz_Structure();
            _Structure.Load(FILEPATH);
            BaseNode = _Structure.WzNode;

            logger.LogInformation($"加载{FILEPATH}成功,根节点为{BaseNode.Text}");

        }

        public MapleFileInfo GetFileInfo(MapleNode node)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MapleNode> GetNodeList(MapleNode parent, int start, int num)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, string> GetNodeProperties(MapleNode node)
        {
            throw new NotImplementedException();
        }

        public MaplePng GetPng(MapleNode node)
        {
            throw new NotImplementedException();
        }

        public MapleUol GetUol(MapleNode node)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _Structure.Clear();
        }
    }
}
