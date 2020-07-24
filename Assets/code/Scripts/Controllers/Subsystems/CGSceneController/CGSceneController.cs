using System.Collections.Generic;
using LocalCache;

namespace Controllers.Subsystems
{
    public class CGScenePointInfo
    {
        public int ID;
        public int touchNum;
    }
    public class CGSceneController: ControllerBase
    {
        public override void Initialize(IControllerProvider args)
        {
            base.Initialize(args);
        }

        public override void Tick()
        {
            base.Tick();
        }

        public override void Terminate()
        {
            base.Terminate();
        }

        public CGScenePointInfo GetScenePointInfo(int pointID)
        {
            return m_data.PointInfos[pointID];
        }

        private CGSceneArchiveData m_data;

    }
}