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

public record HealthEntry(HealthEntryType Type, string Id, DateTime DateTime, decimal Value, string Description, DateTime? EndDateTime = null);
public enum MealSize { Small, Medium, Big }
public enum FeelingType { Great, Normal, Bad, Vomit }
public enum HealthEntryType { Session, AlcoholLevel, AlcoholConsumption }

