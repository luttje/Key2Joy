namespace Key2Joy.Config;

public interface IConfigManager
{
    bool IsInitialized { get; }

    void Save();

    void LoadOrCreate();

    ConfigState GetConfigState();

    bool IsPluginEnabled(string pluginAssemblyPath);

    string GetExpectedPluginChecksum(string pluginAssemblyPath);

    void SetPluginEnabled(string pluginAssemblyPath, string loadedChecksum);
}
