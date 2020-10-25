using System.Collections.Generic;
using UI.UIComponent;
using UnityEngine;

namespace Config.Data
{
    public class CommonConfig : BaseConfig
    {
        public static CommonConfig Data;
        public int ID;
        public int DefaultSoundVolume;
        public int ControversyStageOnePoint;
        public int ControversyStageTwoPoint1;
        public float ControversyBarrageMoveSpeed;
        public float ControversyNormalAttackInterval;
        public float ControversyHeavyAttackInterval;
        public float ControversyHeavyAttackRecover;
        
        public static CommonConfig GetConfigByKey(int key)
        {
            CommonConfig ret;
            if(!_configCache.TryGetValue(key, out ret))
            {
                Debug.LogError($"ConfigData:{typeof(CommonConfig).Name}找不到Key:{key}");
            }
            return ret;
        }
        
        private static Dictionary<int, CommonConfig> _configCache;

        public static void Init(Dictionary<int,CommonConfig> configCache)
        {
            _configCache = configCache;
            Data = configCache[0];
        }

        public static void Dispose()
        {
            _configCache.Clear();
        }
    }
}