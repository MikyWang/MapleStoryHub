using System;
namespace MapleLib
{
    public class SingleRequest<T> where T : new()
    {
        T Parameter { get; set; }
    }

    public class SingleResponese<T> where T : new()
    {
        T Result { get; set; }
    }
}
