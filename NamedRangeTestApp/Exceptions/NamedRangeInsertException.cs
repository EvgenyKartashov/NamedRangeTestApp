using System;

namespace NamedRangeTestApp.Exceptions
{
    public class NamedRangeInsertException : Exception
    {
        public string[] Values { get; init; }
        //public int Index { get; init; }
    }
}
