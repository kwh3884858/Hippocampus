
using UnityEngine;
using StarPlatinum;

namespace Config.GameRoot
{
    /// <summary>
    /// Only for editor
    /// </summary>
    [CreateAssetMenu(fileName = "RootConfig", menuName = "Config/SpawnRootConfig", order = 1)]
    public class RootConfig : ConfigSingleton<RootConfig>
    {
        [Header("Start Scene")]
        public SceneLookupEnum StartScene;

        [Header("Scene Config")]

        [SerializeField]
        private  string Chapter2ScenePier = "PierScene";

        public CameraService.SceneCameraType Chapter2ScenePierCameraType;
    }
}