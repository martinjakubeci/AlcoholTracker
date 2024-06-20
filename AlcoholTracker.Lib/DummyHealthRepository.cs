namespace AlcoholTracker.Lib;

public class DummyHealthRepository : IHealthRepository
{
    List<HealthEntry> healthEntries = new[]
    {
        new HealthEntry(HealthEntryType.Session, "1", new DateTime(2024, 6, 17, 21, 0, 0), 1, "Session", new DateTime(2024, 6, 18, 2, 0, 0)),
        new HealthEntry(HealthEntryType.AlcoholLevel, "", new DateTime(2024, 6, 17, 21, 0, 0), 0, "Alcohol 0%"),
        new HealthEntry(HealthEntryType.AlcoholLevel, "", new DateTime(2024, 6, 17, 23, 0, 0), 0.2m, "Alcohol 0.2%"),
        new HealthEntry(HealthEntryType.AlcoholLevel, "", new DateTime(2024, 6, 18, 0, 30, 0), 0.4m, "Alcohol 0.4%"),
        new HealthEntry(HealthEntryType.AlcoholConsumption, "", new DateTime(2024, 6, 17, 21, 0, 0), 0.1m, "Alcohol"),
        new HealthEntry(HealthEntryType.AlcoholConsumption, "", new DateTime(2024, 6, 17, 23, 30, 0), 0.3m, "Alcohol"),
    }.ToList();

    public Task<HealthEntry?> Get(string id) => Task.FromResult(healthEntries.FirstOrDefault(e => e.Id == id));

    public Task<HealthEntry[]> GetAll() => Task.FromResult(healthEntries.OrderBy(e => e.DateTime).ToArray());

    public void Initialize() { }

    public Task<bool> StoreDrink(DateTime at)
    {
        healthEntries.Add(new HealthEntry(HealthEntryType.AlcoholConsumption, Guid.NewGuid().ToString(), at, 1, "Drink"));

        return Task.FromResult(true);
    }

    public Task<bool> StoreSession(DateTime start, DateTime end)
    {
        healthEntries.Add(new HealthEntry(HealthEntryType.Session, Guid.NewGuid().ToString(), start, 0, "Session", end));

        return Task.FromResult(true);
    }
}

