using NamedRangeTestApp.Exceptions;
using NamedRangeTestApp.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NamedRangeTestApp.Extensions
{
    internal static class ExcelNamedRangeExtensions
    {
        internal static IEnumerable<Cell> GetCells(this ExcelNamedRange cellRange)
        {
            var colNum = cellRange.Columns;
            var rowNum = cellRange.Rows;
            var startCol = cellRange.Start.Column;
            var startRow = cellRange.Start.Row;

            var result = new List<Cell>(colNum * rowNum);

            for (int i = startRow; i < (startRow + rowNum); i++)
            {
                for (int j = startCol; j < (startCol + colNum); j++)
                {
                    var cell = cellRange.Worksheet.Cells[i, j];

                    result.Add(new Cell
                    {
                        Address = cell.Address,
                        Value = cell.Value?.ToString() ?? string.Empty,
                    });
                }
            }

            return result;
        }

        internal static void Insert(this ExcelNamedRange cellRange, object[] values)
        {
            var startCol = cellRange.Start.Column;
            var startRow = cellRange.Start.Row;

            var currentCol = startCol;
            var currentRow = startRow;

            var notInsertedValues = new List<object>();

            foreach (var value in values)
            {
                try
                {
                    var (col, row) = cellRange.InsertValue(currentCol, currentRow, value);

                    currentCol = col;
                    currentRow = row;
                }
                catch (IndexOutOfRangeException)
                {
                    notInsertedValues.Add(value);
                }
            }

            cellRange.ClearUnusedValues(currentCol, currentRow);

            if (notInsertedValues.Any())
                throw new NamedRangeInsertException { Values = notInsertedValues.ToArray() };
        }

        private static (int nextCol, int nextRow) InsertValue(this ExcelNamedRange cellRange, int currentCol, int currentRow, object value)
        {
            var colNum = cellRange.Columns;
            var rowNum = cellRange.Rows;
            var startCol = cellRange.Start.Column;
            var startRow = cellRange.Start.Row;

            if (currentRow >= rowNum + startRow)
                throw new IndexOutOfRangeException();
            //return (currentCol, currentRow);

            var maxCol = startCol + colNum;

            var cell = cellRange.Worksheet.Cells[currentRow, currentCol];
            cell.Value = value;

            var nextCol = currentCol;
            var nextRow = currentRow;

            nextCol = currentCol < maxCol - 1
                ? nextCol + 1
                : startCol;

            nextRow = currentCol < maxCol - 1
                ? nextRow
                : nextRow + 1;

            return (nextCol, nextRow);
        }

        private static void ClearUnusedValues(this ExcelNamedRange cellRange, int currentCol, int currentRow)
        {
            var colNum = cellRange.Columns;
            var rowNum = cellRange.Rows;
            var startCol = cellRange.Start.Column;
            var startRow = cellRange.Start.Row;

            if (currentRow >= rowNum + startRow)
                return;

            for (int i = currentRow; i < (startRow + rowNum); i++)
            {
                for (int j = currentCol; j < (startCol + colNum); j++)
                {
                    var cell = cellRange.Worksheet.Cells[i, j];
                    cell.Clear();
                }

                currentCol = startCol;
            }
        }
    }
}
