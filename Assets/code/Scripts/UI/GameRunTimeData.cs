using Config;
using Config.Data;
using Controllers;
using Game;
using LocalCache;
using StarPlatinum;


public class GameRunTimeData 
{
    public GameState State { get; set; }
    public ControllerManager ControllerManager => m_data.ControllerManager;
    public ConfigProvider ConfigProvider  => m_data.ConfigProvider;
    public SoundService SoundService => m_data.SoundService;

    public ConfigDataProvider ConfigDataProvider => m_data.ConfigDataProvider;

    public LocalCacheManager LocalCacheManager => m_data.LocalCacheManager;
    
    private IGameRuntimeData m_data;
    public GameRunTimeData(IGameRuntimeData data)
    {
        m_data = data;
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