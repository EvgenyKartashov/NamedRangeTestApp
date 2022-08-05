namespace Core.Dictionaries;

public static class CalcNamedRangeDictionary
{
    private static Dictionary<CalcNamedRange, string> _rangeDictionary = new()
    {
        [CalcNamedRange.SimpleOperations] = "simple_operations",
        [CalcNamedRange.LogicalOperations] = "logical_operations",
        [CalcNamedRange.MathOperations] = "math_operations",
        [CalcNamedRange.OtherOperations] = "other",
        [CalcNamedRange.ColumnCellsSum] = "column_cells_sum",
        [CalcNamedRange.RowCellsSum] = "row_cells_sum",
    };

    public static string GetRangeName(this CalcNamedRange range)
    {
        _rangeDictionary.TryGetValue(range, out var result);

        if (result == null)
            throw new NotImplementedException($"No such key ({range}) in named range dictionary");

        return result;
    }
}
