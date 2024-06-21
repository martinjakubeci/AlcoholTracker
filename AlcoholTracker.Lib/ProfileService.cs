using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholTracker.Lib
{
    public class ProfileService(ISettingsRepository settingsRepository)
    {
        private readonly ISettingsRepository settingsRepository = settingsRepository;

        public decimal SobrietyPace
        {
            get => settingsRepository.Get(nameof(SobrietyPace), 0.01m);
            set => settingsRepository.Set(nameof(SobrietyPace), value);
        }

        public decimal DrinkEffect
        {
            get => settingsRepository.Get(nameof(DrinkEffect), 0.02m);
            set => settingsRepository.Set(nameof(DrinkEffect), value);
        }

        public decimal OptimalLevel
        {
            get => settingsRepository.Get(nameof(OptimalLevel), 0.1m);
            set => settingsRepository.Set(nameof(OptimalLevel), value);
        }
    }
}
