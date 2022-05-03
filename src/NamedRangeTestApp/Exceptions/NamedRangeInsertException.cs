using System;

namespace NamedRangeTestApp.Exceptions
{
    public class NamedRangeInsertException : Exception
    {
        public object[] Values { get; init; }
        //public int Index { get; init; }
    }
}
