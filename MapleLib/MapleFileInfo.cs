using System;
namespace MapleLib
{
    public class MapleFileInfo
    {
        //Header
        public string Signature { get; set; }
        public string Copyright { get; set; }
        public string FileName { get; set; }

        public int HeaderSize { get; set; }
        public long DataSize { get; set; }
        public long FileSize { get; set; }
        public int EncryptedVersion { get; set; }

        public bool VersionChecked { get; set; }

        //info
        public string TextEncoding { get; set; }
        public int ImageCount { get; set; }

    }
}
