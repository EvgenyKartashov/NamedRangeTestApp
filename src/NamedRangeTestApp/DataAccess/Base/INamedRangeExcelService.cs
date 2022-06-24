using NamedRangeTestApp.Models;
using System.Collections.Generic;

namespace NamedRangeTestApp.DataAccess.Base;

public interface INamedRangeExcelService
{
    IEnumerable<Cell> GetCellsByNamedRange(string namedRange);
    void InsertValuesToNamedRange(string namedRange, object[] values);
}
