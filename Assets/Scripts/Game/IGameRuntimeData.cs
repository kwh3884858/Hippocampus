using Config;
using Controllers;
using StarPlatinum;

namespace Game
{
    public interface IGameRuntimeData
    {
        ControllerManager ControllerManager { get; }
        ConfigProvider ConfigProvider { get; }
        SoundService SoundService { get; }
    }
}