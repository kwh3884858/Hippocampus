using Config;
using Controllers;
using Skylight;

namespace Game
{
    public interface IGameRuntimeData
    {
        ControllerManager ControllerManager { get; }
        ConfigProvider ConfigProvider { get; }
        SoundService SoundService { get; }
    }
}