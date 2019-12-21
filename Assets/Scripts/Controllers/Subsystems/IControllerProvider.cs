using Controllers.Subsystems.Stroy;

namespace Controllers.Subsystems
{
    public interface IControllerProvider
    {
        GameRunTimeData Data { get; }
        
        StoryController StoryController { get; }
    }
}