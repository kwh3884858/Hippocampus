using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace StarPlatinum.StoryReader
{
    public class StoryReader
    {

        public int m_index = 0;

        private List<StoryBasicData> m_story = new List<StoryBasicData>();

        public StoryReader() { }
        public StoryReader(string path)
        {
            LoadStory(path);
        }

        public enum NodeType
        {
            none,
            label,
            word,
            jump
        };

        //public List<StoryBasicData> GetSotry()
        //{
        //    return m_story;
        //}

        public string GetName()
        {
            StoryWordData data = m_story[m_index] as StoryWordData;
            return data.name;
          
        }

        public string GetContent()
        {
            StoryWordData data = m_story[m_index] as StoryWordData;
            return data.content;
        }

        public string GetJump()
        {
            StoryJumpData data = m_story[m_index] as StoryJumpData;
            return data.jump;
        }

        public void JumpToLabel(string label)
        {
            int i = 0;
            foreach (StoryBasicData item in m_story)
            {
                i++;

                if (item.typename != NodeType.label.ToString()) continue;
                StoryLabelData labelData = item as StoryLabelData;
                if (labelData.label == label)
                {
                    m_index = ++i;
                }
            }
        }

        public int GetIndex()
        {
            return m_index;
        }

        public NodeType GetNodeType()
        {
            if (m_story[m_index].typename == NodeType.word.ToString())
            {
                return NodeType.word;
            }
            else if (m_story[m_index].typename == NodeType.jump.ToString())
            {
                return NodeType.jump;
            }
            else if (m_story[m_index].typename == NodeType.label.ToString())
            {
                return NodeType.label;
            }

            return NodeType.none;
        }

        public void Next()
        {
            m_index++;
        }

        public bool IsDone()
        {
            return m_index >= m_story.Count();
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
                            type = (NodeType)System.Enum.Parse(typeof(NodeType), storyJson[i]["typename"].ToString());
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

                        m_index = 0;
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