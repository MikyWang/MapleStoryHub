using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using MapleLib;
using WzLib;

namespace MapleApi.Extentions
{
    public static class MapleExtentions
    {
        public static NodeType GetNodeType(this Wz_Node wz_Node)
        {
            var value = wz_Node.Value;
            if (value == null) return NodeType.Wz_Null;
            if (value is Wz_File) return NodeType.Wz_File;
            if (value is Wz_Image) return NodeType.Wz_Image;
            if (value is Wz_Png) return NodeType.Wz_Png;
            if (value is Wz_Sound) return NodeType.Wz_Sound;
            if (value is Wz_Uol) return NodeType.Wz_Uol;
            if (value is Wz_Vector) return NodeType.Wz_Vector;
            return NodeType.Wz_Normal;
        }

        public static MapleNode ToNode(this Wz_Node wz_Node)
        {

            return new MapleNode
            {
                Text = wz_Node.Text,
                Value = wz_Node.Value?.ToString(),
                FullPathToFile = wz_Node.FullPathToFile,
                Type = wz_Node.GetNodeType(),
                Parent = wz_Node.ParentNode?.ToNode()
            };
        }

        public static Wz_Node GetImageNode(this Wz_Node wz_Node)
        {
            var image = wz_Node.GetValue<Wz_Image>();
            if (image != null && image.TryExtract())
            {
                return image.Node;
            }
            return null;
        }

        public static MapleFileInfo GetFileInfo(this Wz_Node wz_Node)
        {
            var file = wz_Node.GetValue<Wz_File>();

            if (file == null) return null;

            return new MapleFileInfo
            {
                Signature = file.Header.Signature,
                Copyright = file.Header.Copyright,
                FileName = file.Header.FileName,
                HeaderSize = file.Header.HeaderSize,
                DataSize = file.Header.DataSize,
                FileSize = file.Header.FileSize,
                EncryptedVersion = file.Header.EncryptedVersion,
                VersionChecked = file.Header.VersionChecked,
                TextEncoding = file.TextEncoding.ToString(),
                ImageCount = file.ImageCount
            };
        }

        public static Wz_Node SearchNode(this Wz_Node wz_Node, string fullPathToFile)
        {
            if (fullPathToFile == null || wz_Node.FullPathToFile == fullPathToFile) return wz_Node;
            var pathes = fullPathToFile.Split('\\').ToList();
            if (pathes[0] != wz_Node.FullPathToFile) pathes.Insert(0, wz_Node.FullPathToFile);
            return SearchNode(wz_Node, pathes);

        }

        private static Wz_Node SearchNode(this Wz_Node wz_Node, List<string> pathes)
        {
            Wz_Node node;
            pathes.RemoveAt(0);
            if (pathes.Count == 0) return wz_Node;
            node = wz_Node.FindNodeByPath(pathes[0]);
            if (node == null)
            {
                var value = wz_Node.GetValue<Wz_Image>();
                if (value != null && value.TryExtract())
                {
                    wz_Node = value.Node;
                    node = wz_Node.FindNodeByPath(pathes[0]);
                }
            }
            else
            {
                var value = node.GetValue<Wz_Image>();
                if (value != null && value.TryExtract())
                {
                    node = value.Node;
                }
            }
            return SearchNode(node, pathes);
        }

        public static Wz_Node ToWzNode(this MapleNode node, Wz_Node headNode)
        {
            var wz_Node = headNode.SearchNode(node.FullPathToFile);
            if (wz_Node == null)
            {
                if (headNode.FullPathToFile == node.FullPathToFile)
                {
                    wz_Node = headNode;
                }
            }
            return wz_Node;
        }

        public static string GetValueString(this Wz_Node node)
        {
            var type = node.GetNodeType();
            string value;
            switch (type)
            {
                case NodeType.Wz_Uol:
                    var target = node.GetValue<Wz_Uol>().HandleUol(node);
                    value = $"link:{target.FullPathToFile}";
                    break;
                case NodeType.Wz_Vector:
                    var point = node.GetValue<Wz_Vector>();
                    value = $"({point.X},{point.Y})";
                    break;
                case NodeType.Wz_Normal:
                    value = node.Value.ToString();
                    break;
                case NodeType.Wz_Null:
                case NodeType.Wz_File:
                case NodeType.Wz_Image:
                case NodeType.Wz_Sound:
                case NodeType.Wz_Png:
                default:
                    value = $"link:{node.FullPathToFile}";
                    break;
            }
            return value;
        }

        public static MaplePng ToMaplePng(this Wz_Png wz_Png)
        {
            if (wz_Png == null) return null;

            string base64;
            using (var bmp = wz_Png.ExtractPng())
            {
                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] data = ms.ToArray();
                    base64 = Convert.ToBase64String(data);
                }
            }
            return new MaplePng
            {
                Width = wz_Png.Width,
                Height = wz_Png.Height,
                Base64Data = base64,
                DataLength = wz_Png.DataLength,
                Form = wz_Png.Form
            };
        }

        public static MaplePng GetMaplePng(this Wz_Node wz_Node, Wz_Node baseNode)
        {
            var nodes = wz_Node.Nodes;
            var inLinkNode = nodes["_inlink"];
            var outLinkNode = nodes["_outlink"];
            MaplePng pngInfo;
            if (inLinkNode != null)
            {
                var link = inLinkNode.Value.ToString().Replace('/', '\\');
                var node = wz_Node.GetNodeWzImage().Node.SearchNode(link);
                pngInfo = node.GetValue<Wz_Png>()?.ToMaplePng();
            }
            else if (outLinkNode != null)
            {
                var link = outLinkNode.Value.ToString().Replace('/', '\\');
                var node = baseNode.SearchNode(link);
                pngInfo = node.GetValue<Wz_Png>()?.ToMaplePng();
            }
            else
            {
                pngInfo = wz_Node.GetValue<Wz_Png>()?.ToMaplePng();
            }
            return pngInfo;
        }
        public static IDictionary<string, Point> GetMap(this Wz_Node wz_Node)
        {
            var map = new Dictionary<string, Point>();
            var mapNode = wz_Node.Nodes["map"];
            if (mapNode == null) return null;
            foreach (var node in mapNode.Nodes)
            {
                map.Add(node.Text, node.GetValue<Wz_Vector>());
            }
            return map;
        }
    }
}
