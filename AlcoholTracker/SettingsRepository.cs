using AlcoholTracker.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcoholTracker
{
    internal class SettingsRepository : ISettingsRepository
    {
        public DateTime Get(string name, DateTime defaultValue) => Preferences.Get(name, defaultValue);
        public bool Get(string name, bool defaultValue) => Preferences.Get(name, defaultValue);
        public int Get(string name, int defaultValue) => Preferences.Get(name, defaultValue);
        public decimal Get(string name, decimal defaultValue) => (decimal)Preferences.Get(name, (double)defaultValue);

        public void Set(string name, DateTime value) => Preferences.Set(name, value);
        public void Set(string name, bool value) => Preferences.Set(name, value);
        public void Set(string name, int value) => Preferences.Set(name, value);
        public void Set(string name, decimal value) => Preferences.Set(name, (double)value);
    }
}
