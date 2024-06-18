using AlcoholTracker.Lib;
using Foundation;
using HealthKit;

namespace AlcoholTracker.Platforms.iOS;

public class IosHealthRepository : IHealthRepository
{
    private HKHealthStore healthKitStore = new HKHealthStore();
    private HKQuantityType BacType => HKQuantityType.Create(HKQuantityTypeIdentifier.BloodAlcoholContent)!;
    private HKQuantityType DrinksType => HKQuantityType.Create(HKQuantityTypeIdentifier.NumberOfAlcoholicBeverages)!;

    public void Initialize()
    {
        healthKitStore.RequestAuthorizationToShare(new NSSet(new[] { (HKObjectType)BacType, DrinksType, HKObjectType.WorkoutType }), new NSSet(new[] { (HKObjectType)BacType, DrinksType, HKObjectType.WorkoutType }), ReactToHealthCarePermissions);
    }

    public async Task<HealthEntry[]> GetAll()
    {
        var tcs = new TaskCompletionSource<HealthEntry[]>();
        var task = tcs.Task;

        var sort = new NSSortDescriptor(HKSample.SortIdentifierStartDate, false);
        var descriptor1 = new HKQueryDescriptor(BacType, null);
        var descriptor2 = new HKQueryDescriptor(DrinksType, null);
        var descriptor3 = new HKQueryDescriptor(HKObjectType.WorkoutType, null);

        //var query = new HKSampleQuery(BacType, null, 1, [descriptor], (_, results, error) =>
        var query = new HKSampleQuery([descriptor1, descriptor2, descriptor3], 100, [sort], (_, results, error) =>
        {
            var data = results?.Select(ToHealthEntry)?.Where(r => r != null).ToArray();
            tcs.SetResult(data ?? new HealthEntry[0]);
        });

        healthKitStore.ExecuteQuery(query);

        return await task;
    }

    private static HealthEntry? ToHealthEntry(HKSample record) => record switch
    {
        HKQuantitySample sample => new HealthEntry(HealthEntryType.AlcoholLevel, sample.Uuid.ToString(), (DateTime)sample.StartDate, (decimal)sample.Quantity.GetDoubleValue(HKUnit.Percent), $"Alcohol {sample.Quantity}"),
        HKWorkout workout when workout.WorkoutActivityType == HKWorkoutActivityType.SocialDance => new HealthEntry(HealthEntryType.Session, workout.Uuid.ToString(), (DateTime)workout.StartDate, 0, "Session", (DateTime)workout.EndDate),
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

    public async Task<bool> StoreDrink()
    {
        var tcs = new TaskCompletionSource<bool>();
        var task = tcs.Task;
        var now = DateTime.Now;
        var qty = HKQuantity.FromQuantity(HKUnit.Count, 1);
        var sample = HKQuantitySample.FromType(DrinksType, qty, (NSDate)now, (NSDate)now);

        healthKitStore.SaveObject(sample, (result, _) => tcs.SetResult(result));

        return await task;
    }

    public async Task<bool> StoreSession(DateTime start, DateTime end)
    {
        var tcs = new TaskCompletionSource<bool>();
        var task = tcs.Task;
        var workout = HKWorkout.Create(HKWorkoutActivityType.SocialDance, (NSDate)start, (NSDate)end);

        healthKitStore.SaveObject(workout, (result, _) => tcs.SetResult(result));

        return await task;
    }
}