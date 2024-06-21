using AlcoholTracker.Lib;
using Foundation;
using HealthKit;

namespace AlcoholTracker.Platforms.iOS;

public class IosHealthRepository : IHealthRepository
{
    private readonly HKHealthStore healthKitStore = new();
    private static HKQuantityType BacType => HKQuantityType.Create(HKQuantityTypeIdentifier.BloodAlcoholContent)!;
    private static HKQuantityType DrinksType => HKQuantityType.Create(HKQuantityTypeIdentifier.NumberOfAlcoholicBeverages)!;

    public void Initialize()
    {
        healthKitStore.RequestAuthorizationToShare(new NSSet(new[] { (HKObjectType)BacType, DrinksType, HKObjectType.WorkoutType }), new NSSet(new[] { (HKObjectType)BacType, DrinksType, HKObjectType.WorkoutType }), ReactToHealthCarePermissions);
    }

    public async Task<HealthEntry[]> GetAll()
    {
        var tcs = new TaskCompletionSource<HealthEntry[]>();
        var task = tcs.Task;

        var sort = new NSSortDescriptor(HKSample.SortIdentifierStartDate, true);
        var descriptor1 = new HKQueryDescriptor(BacType, null);
        var descriptor2 = new HKQueryDescriptor(DrinksType, null);
        var descriptor3 = new HKQueryDescriptor(HKObjectType.WorkoutType, null);

        //var query = new HKSampleQuery(BacType, null, 1, [descriptor], (_, results, error) =>
        var query = new HKSampleQuery([descriptor1, descriptor2, descriptor3], 100, [sort], (_, results, error) =>
        {
            var data = results?.Select(ToHealthEntry).OfType<HealthEntry>().ToArray();
            tcs.SetResult(data ?? []);
        });

        healthKitStore.ExecuteQuery(query);

        return await task;
    }

    private static HealthEntry? ToHealthEntry(HKSample record) => record switch
    {
        HKQuantitySample sample when sample.QuantityType.Description == BacType.Description => new HealthEntry(HealthEntryType.AlcoholLevel, sample.Uuid.ToString(), ((DateTime)sample.StartDate).ToLocalTime(), (decimal)sample.Quantity.GetDoubleValue(HKUnit.Percent) * 100, $"Alcohol {sample.Quantity}"),
        HKQuantitySample sample when sample.QuantityType.Description == DrinksType.Description => new HealthEntry(HealthEntryType.AlcoholConsumption, sample.Uuid.ToString(), ((DateTime)sample.StartDate).ToLocalTime(), (decimal)sample.Quantity.GetDoubleValue(HKUnit.Count), $"Alcohol"),
        HKWorkout workout when workout.WorkoutActivityType == HKWorkoutActivityType.SocialDance => new HealthEntry(HealthEntryType.Session, workout.Uuid.ToString(), ((DateTime)workout.StartDate).ToLocalTime(), 0, "Session", ((DateTime)workout.EndDate).ToLocalTime()),
        _ => null
    };

    void ReactToHealthCarePermissions(bool success, NSError error)
    {
        var access = healthKitStore.GetAuthorizationStatus(BacType);

        if (access.HasFlag(HKAuthorizationStatus.SharingAuthorized))
        {
            //HeartRateModel.Instance.Enabled = true;
        }
        else
        {
            //HeartRateModel.Instance.Enabled = false;
        }
    }

    public Task<bool> StoreDrink(DateTime at) => StoreQuantitySample(DrinksType, 1m, at);

    public Task<bool> StoreAlcoholLevel(decimal level, DateTime at) => StoreQuantitySample(BacType, level, at);

    public async Task<bool> StoreSession(DateTime start, DateTime end)
    {
        var tcs = new TaskCompletionSource<bool>();
        var task = tcs.Task;
        var workout = HKWorkout.Create(HKWorkoutActivityType.SocialDance, (NSDate)start.ToUniversalTime(), (NSDate)end.ToUniversalTime());

        healthKitStore.SaveObject(workout, (result, _) => tcs.SetResult(result));

        return await task;
    }

    public async Task<HealthEntry?> Get(string id)
    {
        var all = await GetAll();

        return all.FirstOrDefault(x => x.Id == id);
    }

    private async Task<bool> StoreQuantitySample(HKQuantityType quantityType, decimal value, DateTime at)
    {
        at = at.ToUniversalTime();
        var tcs = new TaskCompletionSource<bool>();
        var task = tcs.Task;
        var qty = HKQuantity.FromQuantity(HKUnit.Count, (double)value);
        var sample = HKQuantitySample.FromType(quantityType, qty, (NSDate)at, (NSDate)at);

        healthKitStore.SaveObject(sample, (result, _) => tcs.SetResult(result));

        return await task;
    }
}