using Core;
using Core.Dictionaries;
using NamedRangeTestApp.DataAccess.Base;
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
        });

        return result.OrderBy(i => i.CellAddress).ToArray();
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
        });

        return result.OrderBy(i => i.CellAddress).ToArray();
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
        });

        return result.OrderBy(i => i.CellAddress).ToArray();
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
        });

        return result.OrderBy(i => i.CellAddress).ToArray();
    }

    private static object GetColumnCellsSumValues(IEnumerable<Cell> cells)
    {
        var result = new
        {
            Info = "{general_column_cells+in_column_cells} с проверкой на пустую ячейку",
            Cells = cells
                .Select(cell => new { CellAddress = cell.Address, Value = cell.Value })
                .OrderBy(i => i.CellAddress),
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
                .OrderBy(i => i.CellAddress),
        };

        return result;
    }
}
