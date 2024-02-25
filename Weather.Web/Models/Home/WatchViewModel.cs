using Weather.Web.Entity;
using Weather.Web.Requests;

namespace Weather.Web.Models.Home;

public class WatchViewModel
{
    public required WatchRequest Filter { get; init; }
    public required List<WeatherRecord> Items { get; init; }
    public required List<int> AvailableYears { get; init; }
    public required int PageCount { get; init; }

    public bool HasPrevPage => Filter.Page > 1;
    public bool HasNextPage => Filter.Page < PageCount;
}