using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Data;
using GamePlay.Global;
using UI.Panels.StaticBoard;
using System;
using UnityEngine.Assertions;

namespace StarPlatinum.StoryReader
{
	public class StoryReader
	{

		public StoryReader () { }
		public StoryReader (string path)
		{
			LoadStory (path);
		}

		public bool GetLoadResult ()
		{
			return m_loadResult;
		}

		public enum StoryJsonLayout
		{
			info,
			value
		};

		public enum InfoLayout
		{
			chapter,
			scene
		};

		public enum NodeType
		{
			none,
			label,
			word,
			jump,
			exhibit,
			end
		};

		//public List<StoryBasicData> GetSotry()
		//{
		//    return m_story;
		//}

		public string GetName ()
		{
			Assert.IsTrue (m_story [m_index].typename == NodeType.word.ToString ());

			StoryWordData data = m_story [m_index] as StoryWordData;
			return data.name;

		}

		public string GetContent ()
		{
			Assert.IsTrue (m_story [m_index].typename == NodeType.word.ToString ());

			StoryWordData data = m_story [m_index] as StoryWordData;
			return data.content;
		}

		public string GetExhibit ()
		{
			Assert.IsTrue (m_story [m_index].typename == NodeType.exhibit.ToString ());

			StoryExhibitData data = m_story [m_index] as StoryExhibitData;
			return data.exhibitName;
		}

		public List<Option> GetJump ()
		{
			Assert.IsTrue (m_story [m_index].typename == NodeType.jump.ToString ());

			//for (int i = 0; i < length; i++)
			//{
			List<Option> options = new List<Option> ();
			//}
			do {

				StoryJumpData data = m_story [m_index] as StoryJumpData;
				Option option = new Option (data.jump, data.content);
				options.Add (option);

				NextStory ();

			} while (m_story [m_index].typename == NodeType.jump.ToString ());
			return options;
		}


		public bool JumpToWordAfterLabel (string label)
		{
			int i = -1;
			foreach (StoryBasicData item in m_story) {
				i++;
				if (item.typename != NodeType.label.ToString ()) continue;

				StoryLabelData labelData = item as StoryLabelData;

				if (labelData.label != label) continue;

				if (i + 1 >= m_story.Count ()) {
					m_index = m_story.Count ();
					return false;
				}

				m_index = i + 1;
				return true;
				//ignore all label and jump
				//for (int j = i + 1; j < m_story.Count (); j++) {
				//	if (m_story [j].typename != NodeType.label.ToString ()) {
				//		m_index = j;
				//		return true;
				//	}
				//}
				//else, story is end


			}
			return false;
		}

		public int GetIndex ()
		{
			return m_index;
		}

		public NodeType GetNodeType ()
		{
			if (m_story [m_index].typename == NodeType.word.ToString ()) {
				return NodeType.word;
			} else if (m_story [m_index].typename == NodeType.jump.ToString ()) {
				return NodeType.jump;
			} else if (m_story [m_index].typename == NodeType.label.ToString ()) {
				return NodeType.label;
			} else if (m_story [m_index].typename == NodeType.exhibit.ToString ()) {
				return NodeType.exhibit;
			} else if (m_story [m_index].typename == NodeType.end.ToString ()) {
				return NodeType.end;
			}

			return NodeType.none;
		}

		public void LastStory ()
		{
			m_index--;
		}

		public void NextStory ()
		{
			m_index++;
		}

		public bool IsDone ()
		{
			return m_index >= m_story.Count ();
		}

		private void ResetIndex ()
		{
			m_index = 0;
		}

		public bool RequestLabel (string label)
		{
			foreach (StoryBasicData data in m_story) {
				if (data.typename == NodeType.label.ToString ()) {
					StoryLabelData labelData = data as StoryLabelData;
					if (labelData.label == label) {
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// 加载故事
		/// </summary>
		private bool LoadStory (string path)
		{
			m_loadResult = false;
			TextAsset story = Resources.Load<TextAsset> (path);
			if (story != null) {
				//DataSet json = JsonConvert.DeserializeObject<DataSet>(story.text);
				JObject json = JObject.Parse (story.text);
				//object json = JsonConvert.DeserializeObject(story.text);
				if (json != null) {
					JToken info = json.SelectToken (StoryJsonLayout.info.ToString ());
					if (info != null && info.Type != JTokenType.Null) {
						m_chapter = info [InfoLayout.chapter.ToString ()].ToString ();
						m_scene = info [InfoLayout.scene.ToString ()].ToString ();
					}
					JArray storyJson = JArray.Parse (json [StoryJsonLayout.value.ToString ()].ToString ());
					if (storyJson != null) {
						int count = storyJson.Count;
						NodeType type;
						string data;
						for (int i = 0; i < count; i++) {

							//type = storyJson[i]["typename"].ToString();
							type = (NodeType)System.Enum.Parse (typeof (NodeType), storyJson [i] ["typename"].ToString ());
							data = storyJson [i].ToString ();
							switch (type) {

							case NodeType.word:
								ReadWord (data);
								break;

							case NodeType.label:
								ReadLabel (data);
								break;

							case NodeType.jump:
								ReadJump (data);
								break;

							case NodeType.end:
								ReadEnd (data);
								break;

							case NodeType.exhibit:
								ReadExhibit (data);
								break;

							default:
								break;

							}
						}

						ResetIndex ();
						m_loadResult = true;
						return true;
					} else {
						Debug.Log ("Can`t open story content");
						return false;
					}
				} else {
					Debug.Log ("Can`t parse story scene and chapter");
					return false;
				}
			} else {
				Debug.Log ("Can`t open story file");
				return false;
			}
		}

		/// <summary>
		/// 读取word
		/// </summary>
		/// <param name="json">数据</param>
		private void ReadWord (string json)
		{
			StoryWordData data = JsonConvert.DeserializeObject<StoryWordData> (json);
			Debug.Log (data.content);
			Debug.Log (data.name);
			Debug.Log (data.typename);
			m_story.Add (data);
		}

		/// <summary>
		/// 读取label
		/// </summary>
		/// <param name="json">数据</param>
		private void ReadLabel (string json)
		{
			StoryLabelData data = JsonConvert.DeserializeObject<StoryLabelData> (json);
			Debug.Log (data.label);
			m_story.Add (data);

		}

		/// <summary>
		/// 读取jump
		/// </summary>
		/// <param name="json">数据</param>
		private void ReadJump (string json)
		{
			StoryJumpData data = JsonConvert.DeserializeObject<StoryJumpData> (json);
			Debug.Log (data.jump);
			m_story.Add (data);

		}

		private void ReadEnd (string json)
		{
			StoryEndData data = JsonConvert.DeserializeObject<StoryEndData> (json);
			m_story.Add (data);
		}

		private void ReadExhibit (string json)
		{
			StoryExhibitData data = JsonConvert.DeserializeObject<StoryExhibitData> (json);
			m_story.Add (data);
		}
		/// <summary>
		/// 基础数据
		/// </summary>
		private int m_index = 0;

		//DO NOT USE
		private string m_chapter;
		public string Chapter => m_chapter;

		//DO NOT USE
		private string m_scene;
		public string Scene => m_scene;

		private bool m_loadResult = false;

		private List<StoryBasicData> m_story = new List<StoryBasicData> ();
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
		public string content;
	}

	[System.Serializable]
	public class StoryEndData : StoryBasicData
	{
	}

	[System.Serializable]
	public class StoryExhibitData : StoryBasicData
	{
		public string exhibitName;
	}
}