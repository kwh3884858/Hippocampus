using UnityEngine;

namespace StarPlatinum.Services.EffectService
{    
    public enum EnumEffectLayerType
    {
        Scene,
        UI,
    }
    
    public class EffectConfig
    {
        public int TargetID;
        public string EffectName;
        public Transform Parent;
        public Vector3 Pos;
        public Vector3 EulerAngle;
        public Vector3 Scale;
        public bool IsFollow = true;
        public bool IsLoop;
        public float Duration;
        public EnumEffectLayerType LayerType;
        
        public EffectConfig()
        {
            
        }

        public EffectConfig(int targetID, string effectName, Transform parent, Vector3 pos, Vector3 eulerAngle, Vector3 scale, bool isFollow, bool isLoop, float duration,EnumEffectLayerType layerType)
        {
            TargetID = targetID;
            EffectName = effectName;
            Parent = parent;
            Pos = pos;
            EulerAngle = eulerAngle;
            Scale = scale;
            IsFollow = isFollow;
            IsLoop = isLoop;
            Duration = duration;
            LayerType = layerType;
        }
    }
}