using UnityEngine;

namespace Config
{
    [CreateAssetMenu]
    public class StoryConfig: BaseConfig<StoryConfig>
    {
        [Header("文本数据存储路径")] 
        public string StoryPath;
    }
}