using System;
using System.Collections.Generic;

namespace MapleLib
{
    public class ListRequest<T>
    {
        public T Parameter { get; set; }
        public int Start { get; set; }
        public int Num { get; set; }
    }

    public class ListResponse<T>
    {
        public IList<T> Results { get; set; }
        public bool HasNext { get; set; }
    }
}
