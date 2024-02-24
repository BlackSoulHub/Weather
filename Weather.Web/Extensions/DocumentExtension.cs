using System.Globalization;
using NPOI.SS.UserModel;

namespace Weather.Web.Extensions;

public static class DocumentExtension
{
    public static string GetCellStringValue(this IRow row, int cell)
    {
        var c = row.GetCell(cell);
        if (c is null)
        {
            return "";
        }

        switch (c.CellType)
        {
            case CellType.Numeric:
                return c.NumericCellValue.ToString(CultureInfo.InvariantCulture);
            case CellType.String:
                return c.StringCellValue;
            case CellType.Blank:
                return "";
            default:
                throw new ArgumentOutOfRangeException(nameof(cell), $"Cell has type: {c.CellType}");
        }

        
    }
}