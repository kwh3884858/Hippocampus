using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StarPlatinum.StoryReader
{
    public class StoryReader
    {
        private List<StoryBasicData> m_story = new List<StoryBasicData>();
        public StoryReader() { }
        public StoryReader(string path)
        {
            LoadStory(path);
        }

        public enum NodeType
        {
            none ,
            label,
            word,
            jump
        };

         public List<StoryBasicData> GetSotry()
        {
            return m_story;
        }

        /// <summary>
        /// 加载故事
        /// </summary>
        private void LoadStory(string path)
        {
            TextAsset story = Resources.Load<TextAsset>(path);
            if (story != null)
            {
                object json = JsonConvert.DeserializeObject(story.text);
                if (json != null)
                {
                    JArray storyJson = JArray.FromObject(json);
                    if (storyJson != null)
                    {
                        int count = storyJson.Count;
                        NodeType type;
                        string data;
                        for (int i = 0; i < count; i++)
                        {

                            //type = storyJson[i]["typename"].ToString();
                            type =(NodeType)System.Enum.Parse(typeof(NodeType), storyJson[i]["typename"].ToString());
                            data = storyJson[i].ToString();
                            switch (type)
                            {
                                
                                case NodeType.word:
                                    ReadWord(data);
                                    break;

                                case NodeType.label:
                                    ReadLabel(data);
                                    break;

                                case NodeType.jump:
                                    ReadJump(data);
                                    break;

                                default:
                                    break;

                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("Can`t open story file");
            }
        }

        /// <summary>
        /// 读取word
        /// </summary>
        /// <param name="json">数据</param>
        private void ReadWord(string json)
        {
            StoryWordData data = JsonConvert.DeserializeObject<StoryWordData>(json);
            Debug.Log(data.content);
            Debug.Log(data.name);
            Debug.Log(data.typename);
            m_story.Add(data);
        }

        /// <summary>
        /// 读取label
        /// </summary>
        /// <param name="json">数据</param>
        private void ReadLabel(string json)
        {
            StoryLabelData data = JsonConvert.DeserializeObject<StoryLabelData>(json);
            Debug.Log(data.label);
            m_story.Add(data);

        }

        /// <summary>
        /// 读取jump
        /// </summary>
        /// <param name="json">数据</param>
        private void ReadJump(string json)
        {
            StoryJumpData data = JsonConvert.DeserializeObject<StoryJumpData>(json);
            Debug.Log(data.jump);
            m_story.Add(data);

        }

        /// <summary>
        /// 基础数据
        /// </summary>
   
    }

    [System.Serializable]
    public class StoryBasicData
    {
        public string typename;
    }

    /// <summary>
    /// word类
    /// </summary>
    [System.Serializable]
    public class StoryWordData : StoryBasicData
    {
        public string content;
        public string name;
    }

    /// <summary>
    /// label
    /// </summary>
    [System.Serializable]
    public class StoryLabelData : StoryBasicData
    {
        public string label;
    }

    /// <summary>
    /// jump
    /// </summary>
    [System.Serializable]
    public class StoryJumpData : StoryBasicData
    {
        public string jump;
    }
}