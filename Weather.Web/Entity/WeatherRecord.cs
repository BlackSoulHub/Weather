namespace Weather.Web.Entity;

public class WeatherRecord
{
    /// <summary>
    /// Id.
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Дата и время записи. Время - московское.
    /// </summary>
    public required DateTime DateTime { get; init; }
    /// <summary>
    /// Температура. В градусах цельсия.
    /// </summary>
    public required double Temperature { get; init; }
    /// <summary>
    /// Влажность. В процентах.
    /// </summary>
    public required double Humidity { get; init; }
    /// <summary>
    /// Точка росы
    /// </summary>
    public required double Td { get; init; }
    /// <summary>
    /// Атмосферное давление. В мм. ртутного столба.
    /// </summary>
    public required double AirPressure { get; init; }
    /// <summary>
    /// Направление ветра
    /// </summary>
    public required string? WindDirection { get; init; }
    /// <summary>
    /// Скорость ветра. В метрах в секунду
    /// </summary>
    public required int? WindSpeed { get; init; }
    /// <summary>
    /// Облачность. В процентах
    /// </summary>
    public required byte? Cloudiness { get; init; }
    /// <summary>
    /// Нижняя граница облачности. В метрах.
    /// </summary>
    public required int? LowCloudiness { get; init; }
    /// <summary>
    /// Горизонтальная видимость. В киллометрах.
    /// </summary>
    public required double? VV { get; init; }
    /// <summary>
    /// Погодные явления
    /// </summary>
    public required string? WeatherEvent { get; init; }
}