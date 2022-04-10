using NamedRangeTestApp.Models;
using System.Collections.Generic;

namespace NamedRangeTestApp.DataAccess.Base
{
    public interface ITestExcelService
    {
        public IEnumerable<Cell> CheckNamedRangeReferences(string namedRange, string[] values);
    }
}
