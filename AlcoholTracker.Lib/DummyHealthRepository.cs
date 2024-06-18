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

    public Task<HealthEntry[]> GetAll()
    {
        return Task.FromResult(healthEntries.OrderBy(e => e.DateTime).ToArray());
    }

    public void Initialize() { }

    public Task<bool> StoreDrink()
    {
        healthEntries.Add(new HealthEntry(HealthEntryType.AlcoholConsumption, Guid.NewGuid().ToString(), DateTime.Now, 1, "Drink"));

        return Task.FromResult(true);
    }

    public Task<bool> StoreSession(DateTime start, DateTime end)
    {
        return Task.FromResult(true);
    }
}

