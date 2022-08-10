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

        [CalcNamedRange.ArraysSum] = "arrays_sum",
        [CalcNamedRange.AggregationFunctions] = "aggregation_functions",
        [CalcNamedRange.AggregationFunctionsWithCondition] = "aggregation_functions_with_conditions",
        [CalcNamedRange.OtherArrayFunctions] = "other_array_functions",

        [CalcNamedRange.Strings] = "strings",
        [CalcNamedRange.Arrays] = "arrays",

        [CalcNamedRange.Others] = "others",
        [CalcNamedRange.Column] = "column",
        [CalcNamedRange.Row] = "row",
    };

    public static string GetRangeName(this CalcNamedRange range)
    {
        _rangeDictionary.TryGetValue(range, out var result);

        if (result == null)
            throw new NotImplementedException($"No such key ({range}) in named range dictionary");

        return result;
    }
}
