namespace Weather.Web.Requests;

public class WatchRequest
{
    public int? Page { get; set; } = 1;
    public int? Year { get; init; }
    public int? Month { get; init; }
}