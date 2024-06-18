namespace AlcoholTracker.Lib;

public interface ISettingsRepository
{
    DateTime Get(string name, DateTime defaultValue);
    bool Get(string name, bool defaultValue);

    void Set(string name, DateTime value);
    void Set(string name, bool value);
}

