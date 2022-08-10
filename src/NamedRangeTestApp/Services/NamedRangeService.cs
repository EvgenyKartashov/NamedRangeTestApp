using Core;
using Core.Dictionaries;
using NamedRangeTestApp.DataAccess.Base;
using NamedRangeTestApp.Extensions;
using NamedRangeTestApp.Models;
using NamedRangeTestApp.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NamedRangeTestApp.Services;

public class NamedRangeService : INamedRangeService
{
    private readonly INamedRangeExcelService _namedRangeService;

    public NamedRangeService(INamedRangeExcelService namedRangeService)
    {
        _namedRangeService = namedRangeService;
    }

    public object GetRangeData(CalcNamedRange namedRange)
    {
        var namedRangeStr = namedRange.GetRangeName();
        var rangeValues = _namedRangeService.GetCellsByNamedRange(namedRangeStr);

        object result = namedRange switch
        {
            CalcNamedRange.SimpleOperations => GetSimpleOperationsValues(rangeValues),
            CalcNamedRange.LogicalOperations => GetLogicalOperationsValues(rangeValues),
            CalcNamedRange.MathOperations => GetMathOperationsValues(rangeValues),
            CalcNamedRange.OtherOperations => GetOtherOperationsValues(rangeValues),
            CalcNamedRange.ColumnCellsSum => GetColumnCellsSumValues(rangeValues),
            CalcNamedRange.RowCellsSum => GetRowCellsSumValues(rangeValues),

            CalcNamedRange.ArraysSum => GetArraySumValues(rangeValues),
            CalcNamedRange.AggregationFunctions => GetAggregationResult(rangeValues),
            CalcNamedRange.AggregationFunctionsWithCondition => GetAggregationWithConditionResult(rangeValues),
            CalcNamedRange.OtherArrayFunctions => GetOtherArrayFunctionResult(rangeValues),

            CalcNamedRange.Strings => GetStringsResult(rangeValues),

            CalcNamedRange.Others => GetVpr(rangeValues),
            CalcNamedRange.Column => GetColumns(rangeValues),
            CalcNamedRange.Row => GetRows(rangeValues),

            _ => throw new NotImplementedException(),
        };

        return result;
    }

    private static object GetSimpleOperationsValues(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C3" => "general_single_cell + in_single_cell",
                "C4" => "general_single_cell - in_single_cell",
                "C5" => "general_single_cell * (-3,14)",
                "C6" => "general_single_cell / 0,87",
                "C7" => "TRUNC (ОТБР)",
                "C8" => "ROUNDUP (ОКРУГЛВВЕРХ)",
                "C9" => "ROUNDDOWN (ОКРУГЛВНИЗ)",
                "C10" => "ROUND (ОКРУГЛ)",
                "C11" => "ABS",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetLogicalOperationsValues(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C12" => "general_single_cell > 0",
                "C13" => "general_single_cell < 0",
                "C14" => "general_single_cell = 0",
                "C15" => "NOT",
                "C16" => "OR",
                "C17" => "AND",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetMathOperationsValues(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C18" => "Степень",
                "C19" => "EXP",
                "C20" => "Логарифм",
                "C21" => "LN",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetOtherOperationsValues(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C22" => "RAND (СЛЧИС)",
                "C23" => "N (Ч)",
                "C24" => "NUMBERVALUE (ЧЗНАЧ) 1g000d + general_single_cell",
                "C25" => "ЯЧЕЙКА",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetColumnCellsSumValues(IEnumerable<Cell> cells)
    {
        var result = new
        {
            Info = "{general_column_cells+in_column_cells} с проверкой на пустую ячейку",
            Cells = cells
                .Select(cell => new { CellAddress = cell.Address, Value = cell.Value })
                .OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
                .ToArray(),
        };

        return result;
    }

    private static object GetRowCellsSumValues(IEnumerable<Cell> cells)
    {
        var result = new
        {
            Info = "{general_row_cells+in_row_cells} с проверкой на пустую ячейку",
            Cells = cells
                .Select(cell => new { CellAddress = cell.Address, Value = cell.Value })
                .OrderBy(i => i.CellAddress.GetFirstCharacter())
                .ToArray(),
        };

        return result;
    }

    private static object GetArraySumValues(IEnumerable<Cell> cells)
    {
        throw new NotImplementedException();
    }

    private static object GetAggregationResult(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C45" => "SUM (СУММ) general_row_cells",
                "C46" => "PRODUCT (ПРОИЗВЕД) general_row_cells",
                "C47" => "SUMPRODUCT (СУММПРОИЗВ) (in_row_cells, general_row_cells)",
                "C48" => "MAX (МАКС) general_row_cells",
                "C49" => "MIN (МИН) general_row_cells",
                "C50" => "AVERAGE (СРЗНАЧ) general_row_cells",
                "C51" => "SMALL (НАИМЕНЬШИЙ) (general_row_cells,2)",
                "C52" => "LARGE (НАИБОЛЬШИЙ) (general_row_cells,2)",
                "C53" => "COUNT (СЧЁТ) general_row_cells",
                "C54" => "COUNTA (СЧЁТЗ) general_row_cells",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetAggregationWithConditionResult(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C55" => "SUMIF (СУММЕСЛИ) (f1_p1,\"п1\",f1_v1)",
                "C56" => "SUMIFS (СУММЕСЛИМН) (f1_v1,f1_p1,\"п1\",f1_p2,\" > 1\")",
                "C57" => "СЧЁТЕСЛИ",
                "C58" => "СЧЁТЕСЛИМН",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetOtherArrayFunctionResult(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C59" => "ВПР",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetStringsResult(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C61" => "конкатенация",
                "C62" => "ПОДСТАВИТЬ",
                "C63" => "ПРАВСИМВ",
                "C64" => "ЛЕВСИМВ",
                "C65" => "НАЙТИ",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetVpr(IEnumerable<Cell> cells)
    {
        var result = cells.Select(cell =>
        {
            var info = cell.Address switch
            {
                "C74" => "ВПР",
                _ => throw new NotImplementedException(),
            };

            return new { Info = info, CellAddress = cell.Address, Value = cell.Value };
        }).OrderBy(q => int.Parse(q.CellAddress.RemoveFirstCharacter()))
          .ToArray();

        return result;
    }

    private static object GetColumns(IEnumerable<Cell> cells)
    {
        var result = new
        {
            Info = "Column",
            Cells = cells
                .Select(cell => new { CellAddress = cell.Address, Value = cell.Value })
                .OrderBy(i => i.CellAddress.GetFirstCharacter())
                .ToArray(),
        };

        return result;
    }

    private static object GetRows(IEnumerable<Cell> cells)
    {
        var result = new
        {
            Info = "Row",
            Cells = cells
                .Select(cell => new { CellAddress = cell.Address, Value = cell.Value })
                .OrderBy(i => int.Parse(i.CellAddress.RemoveFirstCharacter()))
                .ToArray(),
        };

        return result;
    }
}
