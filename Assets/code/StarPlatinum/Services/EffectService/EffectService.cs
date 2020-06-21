using System.Collections.Generic;
using StarPlatinum.Base;
using UnityEngine;

namespace StarPlatinum.Services.EffectService
{
    public class EffectStateInfo
    {
        public int GameObjectHash;
        public int Count;
    }
    public class EffectService: Singleton<EffectService>
    {
        private Dictionary<string, Dictionary<string,EffectStateInfo>> m_loops;
        #region UI Effect
        public void PlayUIEffectOnce(GameObject target, string newEffectName , Vector3 pos)
        {
            var config = GetEffectConfig(target.GetHashCode(),newEffectName, null,pos);
            CreateEffectObject(config);
        }
        public void PlayUIEffectOnce(GameObject target, string newEffectName,Transform parent, Vector3 posOffset = default(Vector3), bool isFollow = true)
        {
            var config = GetEffectConfig(target.GetHashCode(),newEffectName, parent,posOffset,default(Vector3),default(Vector3),isFollow);
            CreateEffectObject(config);
        }
        
        public void PlayUIEffectLoop(GameObject target, string newEffectName, Vector3 posOffset = default(Vector3), bool isFollow = true, Transform parent = null)
        {
            
        }
        
        public void StopUIEffectLoop(GameObject target, string effectName)
        {
            RemoveSceneEffectLoop(target, effectName);
        }

        #endregion

        public void ReleaseEffectAsset(GameObject effect)
        {
            
        }
        
        private void RemoveSceneEffectLoop(GameObject target,string effectName)
        {
            
        }
        
        private EffectConfig GetEffectConfig(int targetID, string newEffectName, Transform parent, Vector3 pos = default(Vector3), Vector3 angle = default(Vector3), Vector3 scale = default(Vector3), bool isFollow = true, bool isLoop = false, EnumEffectLayerType layerType = EnumEffectLayerType.Scene, float duration = 0)
        {
            var config = new EffectConfig(targetID,newEffectName, parent, pos, angle,scale, isFollow, isLoop,duration , layerType);
            return config;
        }

        private void CreateEffectObject(EffectConfig config)
        {
            PrefabManager.Instance.InstantiateComponentAsync<EffectMono>(config.EffectName, (result) =>
            {
                if (result.status == RequestStatus.FAIL)
                {
                    Debug.LogError($"特效加载失败,请检查！Key:{config.EffectName}");
                    return;
                }

                var effect = result.result as EffectMono;
                effect.SetInfo(config);
            });
        }
    }
}