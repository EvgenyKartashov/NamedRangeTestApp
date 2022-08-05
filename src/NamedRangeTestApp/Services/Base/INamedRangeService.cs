using Core;

namespace NamedRangeTestApp.Services.Base
{
    public interface INamedRangeService
    {
        object GetRangeData(CalcNamedRange namedRange);
    }
}
