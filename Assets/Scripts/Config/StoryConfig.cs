using UnityEngine;
using UnityEngine.Serialization;

namespace Config
{
    [CreateAssetMenu]
    public class StoryConfig: BaseConfig<StoryConfig>
    {
        [Header("文本数据存储路径")] 
        public string StoryPath;
        [Header("打字机显示中文速度 单位-秒")]
        public float ChineseContentSpeed;
        [Header("打字机显示英文速度 单位-秒")]
        public float EnglishContentSpeed;
    }
}