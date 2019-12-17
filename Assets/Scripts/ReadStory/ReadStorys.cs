using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ReadStorys
{

    public ReadStorys()
    {
        LoadStory();
    }

    /// <summary>
    /// 加载故事
    /// </summary>
    private void LoadStory()
    {
        TextAsset story = Resources.Load<TextAsset>("Storys/k");
        if (story != null)
        {
            object json = JsonConvert.DeserializeObject(story.text);
            if(json != null)
            {
                JArray test = JArray.FromObject(json);
                if (test != null)
                {
                    int l = test.Count;
                    string type;
                    string data;
                    for (int i = 0; i < l; i++)
                    {
                        type = test[i]["typename"].ToString();
                        data = test[i].ToString();
                        switch (type)
                        {
                            case "word":
                                ReadWord(data);
                                break;
                            case "lebel":
                                ReadLabel(data);
                                break;
                            case "jump":
                                ReadJump(data);
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
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
    }

    /// <summary>
    /// 读取label
    /// </summary>
    /// <param name="json">数据</param>
    private void ReadLabel(string json)
    {
        StoryLabelData data = JsonConvert.DeserializeObject<StoryLabelData>(json);
        Debug.Log(data.label);
    }

    /// <summary>
    /// 读取jump
    /// </summary>
    /// <param name="json">数据</param>
    private void ReadJump(string json)
    {
        StoryJumpData data = JsonConvert.DeserializeObject<StoryJumpData>(json);
        Debug.Log(data.jump);
    }

    /// <summary>
    /// 基础数据
    /// </summary>
    [System.Serializable]
    class StoryBasicData
    {
        public string typename;
    }

    /// <summary>
    /// word类
    /// </summary>
    [System.Serializable]
    class StoryWordData : StoryBasicData
    {
        public string content;
        public string name;
    }

    /// <summary>
    /// label
    /// </summary>
    [System.Serializable]
    class StoryLabelData : StoryBasicData
    {
        public string label;
    }

    /// <summary>
    /// jump
    /// </summary>
    [System.Serializable]
    class StoryJumpData : StoryBasicData
    {
        public string jump;
    }
}
