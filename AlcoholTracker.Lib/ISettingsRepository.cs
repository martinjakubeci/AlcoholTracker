namespace AlcoholTracker.Lib;

public interface ISettingsRepository
{
    DateTime Get(string name, DateTime defaultValue);
    bool Get(string name, bool defaultValue);
    int Get(string name, int defaultValue);
    decimal Get(string name, decimal defaultValue);

    void Set(string name, DateTime value);
    void Set(string name, bool value);
    void Set(string name, int value);
    void Set(string name, decimal value);
}

