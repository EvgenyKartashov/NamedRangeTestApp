using NamedRangeTestApp.Models;
using System.Collections.Generic;

namespace NamedRangeTestApp.DataAccess.Base
{
    public interface ITestExcelService
    {
        IEnumerable<Cell> AddValuesToScenarioAndCalc(string namedRange, object[] values);
    }
}
