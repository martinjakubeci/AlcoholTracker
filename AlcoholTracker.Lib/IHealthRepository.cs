using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholTracker.Lib;

public interface IHealthRepository
{
    void Initialize();
    Task<HealthEntry[]> GetAll();
    Task<bool> StoreDrink();

    Task<bool> StoreSession(DateTime start, DateTime end);
}

public class DummyHealthRepository : IHealthRepository
{
    List<HealthEntry> healthEntries = new[]
    {
        new HealthEntry(HealthEntryType.Session, "1", new DateTime(2024, 6, 17, 21, 0, 0), 1, "Session", new DateTime(2024, 6, 18, 2, 0, 0)),
        new HealthEntry(HealthEntryType.AlcoholLevel, "", new DateTime(2024, 6, 17, 21, 0, 0), 0, "Alcohol 0%"),
        new HealthEntry(HealthEntryType.AlcoholLevel, "", new DateTime(2024, 6, 17, 23, 0, 0), 0.2m, "Alcohol 0.2%"),
        new HealthEntry(HealthEntryType.AlcoholLevel, "", new DateTime(2024, 6, 18, 0, 30, 0), 0.4m, "Alcohol 0.4%"),
        new HealthEntry(HealthEntryType.AlcoholConsumption, "", new DateTime(2024, 6, 17, 21, 15, 0), 1m, "Alcohol"),
        new HealthEntry(HealthEntryType.AlcoholConsumption, "", new DateTime(2024, 6, 17, 23, 30, 0), 1m, "Alcohol"),
    }.ToList();

    public Task<HealthEntry[]> GetAll()
    {
        return Task.FromResult(healthEntries.ToArray());
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

public record HealthEntry(HealthEntryType Type, string Id, DateTime DateTime, decimal Value, string Description, DateTime? EndDateTime = null);
public enum MealSize { Small, Medium, Big }
public enum FeelingType { Great, Normal, Bad, Vomit }
public enum HealthEntryType { Session, AlcoholLevel, AlcoholConsumption }

