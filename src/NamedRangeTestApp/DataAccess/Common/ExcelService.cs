using OfficeOpenXml;
using System;
using System.IO;

namespace NamedRangeTestApp.DataAccess.Common
{
    public abstract class ExcelService
    {
        protected ExcelPackage InitPackage(string subdir, string fileName)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var file = new FileInfo($"{currentDir}/{subdir}/{fileName}");

            if (!file.Exists)
                throw new Exception("File does not exist");

            return new ExcelPackage(file);
        }
    }
}
