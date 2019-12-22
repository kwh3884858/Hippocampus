using Controllers.Subsystems.Story;

namespace Controllers.Subsystems
{
    public interface IControllerProvider
    {
        GameRunTimeData Data { get; }
        
        StoryController StoryController { get; }
    }
}