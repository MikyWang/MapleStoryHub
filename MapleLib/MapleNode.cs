using System;
using System.ComponentModel;

namespace MapleLib
{
    public class MapleNode
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public NodeType Type { get; set; }
        public MapleNode Parent { get; set; }
        public string FullPathToFile { get; set; }
    }

    public enum NodeType
    {
        [Description("文件")]
        Wz_File,
        [Description("映像")]
        Wz_Image,
        [Description("音乐")]
        Wz_Sound,
        [Description("图片")]
        Wz_Png,
        [Description("链接")]
        Wz_Uol,
        [Description("坐标")]
        Wz_Vector,
        [Description("字符串")]
        Wz_Normal,
        [Description("默认")]
        Wz_Null
    }
}
