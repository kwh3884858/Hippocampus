using code.Scripts.Provider;
using Config;
using Config.Data;
using Controllers;
using Game;
using LocalCache;
using StarPlatinum;


public class GameRunTimeData 
{
    public GameState State { get; set; }
    public ControllerManager ControllerManager => Data.ControllerManager;
    public ConfigProvider ConfigProvider  => Data.ConfigProvider;
    public SoundService SoundService => Data.SoundService;

    public ConfigDataProvider ConfigDataProvider => Data.ConfigDataProvider;

    public LocalCacheManager LocalCacheManager => Data.LocalCacheManager;

    public ColorProvider ColorProvider => Data.ColorProvider;
    
    public IGameRuntimeData Data;
    public GameRunTimeData(IGameRuntimeData data)
    {
        Data = data;
    }

    public static GameRunTimeData Instance;
}

public enum GameState
{
    Loading,
    MainManu,
    Battle,
    Invalid,
}