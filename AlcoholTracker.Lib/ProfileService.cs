using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholTracker.Lib
{
    public class ProfileService
    {
        private readonly ISettingsRepository settingsRepository;

        public ProfileService(ISettingsRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public decimal SobrietyPace
        {
            get => settingsRepository.Get(nameof(SobrietyPace), 0.001m);
            set => settingsRepository.Set(nameof(SobrietyPace), value);
        }

        public decimal DrinkEffect
        {
            get => settingsRepository.Get(nameof(DrinkEffect), 0.002m);
            set => settingsRepository.Set(nameof(DrinkEffect), value);
        }
    }
}
