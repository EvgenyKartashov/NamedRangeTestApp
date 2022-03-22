using NamedRangeTestApp.Models;
using System.Collections.Generic;

namespace NamedRangeTestApp.DataAccess.Base
{
    public interface IExcelService
    {
        IEnumerable<Cell> GetCellsByNamedRange(string namedRange);
    }
}
