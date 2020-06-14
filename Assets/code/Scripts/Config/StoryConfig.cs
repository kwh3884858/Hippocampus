using System.Collections.Generic;
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
        [Header("立绘位置")] 
        public List<float> PicturePosition;
        [Header("立绘移动曲线")] 
        public List<AnimationCurve> PictureMovingCurve;
        [Header("自动播放单句结束后停顿时间")] 
        public float AutoPlayWaitingTime = 0.5f;
        [Header("自动播放激活颜色")]
        public Color AutoPlayButtonActiveColor = Color.red;
        [Header("自动播放未激活颜色")]
        public Color AutoPlayButtonNormalColor = Color.white;
        [Header("打字机默认音效")] 
        public string TypewriterDefaultSound = "UI_Typewriter_Default";
    }
}