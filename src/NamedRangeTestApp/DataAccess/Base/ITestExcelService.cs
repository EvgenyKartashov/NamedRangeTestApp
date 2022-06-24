using NamedRangeTestApp.Models;
using System.Collections.Generic;

namespace NamedRangeTestApp.DataAccess.Base;

public interface ITestExcelService
{
    void AddValuesToScenario(IEnumerable<NamedRangeData> data);
    Cell[] RecalculateModel();
}
