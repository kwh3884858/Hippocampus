using Config;
using Config.Data;
using Controllers;
using Game;

using StarPlatinum;


public class GameRunTimeData 
{
    public GameState State { get; set; }
    public ControllerManager ControllerManager => m_data.ControllerManager;
    public ConfigProvider ConfigProvider  => m_data.ConfigProvider;
    public SoundService SoundService => m_data.SoundService;

    public ConfigDataProvider ConfigDataProvider => m_data.ConfigDataProvider;
    
    private IGameRuntimeData m_data;
    public GameRunTimeData(IGameRuntimeData data)
    {
        m_data = data;
    }
}

public enum GameState
{
    Loading,
    MainManu,
    Battle,
    Invalid,
}