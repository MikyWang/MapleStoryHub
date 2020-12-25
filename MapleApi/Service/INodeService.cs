using System;
using System.Collections.Generic;
using MapleLib;
using WzLib;

namespace MapleApi.Service
{
    public interface INodeService
    {
        public Wz_Node BaseNode { get; }

        public IDictionary<string, string> GetNodeProperties(MapleNode node);
        public IEnumerable<MapleNode> GetNodeList(MapleNode parent, int start, int num);
        public MapleFileInfo GetFileInfo(MapleNode node);
        public MaplePng GetPng(MapleNode node);
        public MapleUol GetUol(MapleNode node);
    }
}
