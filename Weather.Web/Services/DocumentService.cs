using System.Globalization;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Weather.Web.Entity;
using Weather.Web.Extensions;

namespace Weather.Web.Services;

public class DocumentService(ILogger<DocumentService> logger)
{
    public List<WeatherRecord> ParseDocument(Stream file)
    {
        using var workBook = new XSSFWorkbook(file);

        var data = new List<WeatherRecord>();
        
        for (int i = 0; i < workBook.NumberOfSheets; i++)
        {
            var sheet = workBook.GetSheetAt(i);
            if (!ValidateSheet(sheet))
            {
                logger.LogError("Sheet {sheet} has not valid format", sheet.SheetName);
                continue;
            }
            
            var rows = sheet.PhysicalNumberOfRows;
            
            for (int j = 5; j < rows; j++)
            {
                var row = sheet.GetRow(j);
                try
                {
                    var record = ParseRow(row);
                    data.Add(record);
                }
                catch (FormatException exception)
                {
                    logger.LogError("Error parsing row: {error}. Skip this row", exception.Message);
                }
            }
        }
        
        return data;
    }

    private WeatherRecord ParseRow(IRow row)
    {
        var (date, time) = (row.GetCellStringValue(0), row.GetCellStringValue(1));
        if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(time))
        {
            throw new FormatException("Date or time is empty");
        }

        var dateTime = new DateTime(DateOnly.Parse(date, new CultureInfo("ru")),
            TimeOnly.Parse(time, new CultureInfo("ru")));

        var windSpeed = row.GetCellStringValue(7);
        var cloudiness = row.GetCellStringValue(8);
        var lowCloudiness = row.GetCellStringValue(9);
        var vv = row.GetCellStringValue(10);

        return new WeatherRecord
        {
            DateTime = dateTime,
            Temperature = double.Parse(row.GetCellStringValue(2)),
            Humidity = double.Parse(row.GetCellStringValue(3)),
            Td = double.Parse(row.GetCellStringValue(4)),
            AirPressure = double.Parse(row.GetCellStringValue(5)),
            WindDirection = row.GetCellStringValue(6),
            WindSpeed = string.IsNullOrWhiteSpace(windSpeed) ? null : int.Parse(windSpeed),
            Cloudiness = string.IsNullOrWhiteSpace(cloudiness) ? null : byte.Parse(cloudiness),
            LowCloudiness = string.IsNullOrWhiteSpace(lowCloudiness) ? null : int.Parse(lowCloudiness),
            VV = string.IsNullOrWhiteSpace(vv) ? null : double.Parse(vv),
            WeatherEvent = row.GetCellStringValue(11)
        };
    }

    private bool ValidateSheet(ISheet sheet)
    {
        var firstDataRow = sheet.GetRow(5);
        if (firstDataRow is null)
        {
            return false;
        }

        var firstCell = firstDataRow.GetCell(0);
        var secondCell = firstDataRow.GetCell(1);
        if (firstCell is null || secondCell is null)
        {
            return false;
        }

        DateOnly.Parse(firstCell.StringCellValue);
        TimeOnly.Parse(secondCell.StringCellValue);
        return true;
    }
}