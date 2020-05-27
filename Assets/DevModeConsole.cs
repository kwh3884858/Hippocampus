using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class DevModeConsole : MonoBehaviour
{
	public GameObject m_parentPanel;
	public InputField m_inputFiled;

	public Text m_log;

	private QueueArray<string> m_commandHistory;

	private string [] m_commandHistoryCache;
	private int m_commandHistoryIndex;

	class QueueArray<T>
	{
		T [] m_content;
		uint m_capacity;

		uint m_endPos;
		uint m_count;
		public QueueArray (uint capacity)
		{
			m_content = new T [capacity];
			m_capacity = capacity;
			m_endPos = 0;
			m_count = 0;
		}

		public void Push (T item)
		{
			m_content [m_endPos++] = item;
			m_endPos = m_endPos % m_capacity;

			m_count++;
			if (m_count > m_capacity) {
				m_count = m_capacity;
			};
		}

		public T [] GetAll ()
		{
			T [] result = new T [m_count];
			int index = (int)m_endPos;

			for (int i = 0; i < m_count; i++) {
				index = (--index < 0) ? index + (int)m_capacity : index;
				result [i] = m_content [index];
			}
			return result;
		}
	}

	void Start ()
	{
		//Default close console
		m_parentPanel.SetActive (false);
		m_commandHistory = new QueueArray<string> (30);
		m_commandHistoryCache = null;
		m_commandHistoryIndex = 0;
	}


	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.BackQuote)) {
			bool isActive = m_parentPanel.activeSelf;
			m_parentPanel.SetActive (!isActive);
		}

		if (m_inputFiled.text != "" &&
				(
				Input.GetKeyDown (KeyCode.Return) ||
				Input.GetKeyDown (KeyCode.KeypadEnter)
				)
			) {
			string command = m_inputFiled.text;
			PrintLog (command);
			m_commandHistory.Push (command);
			m_commandHistoryCache = m_commandHistory.GetAll ();
			m_commandHistoryIndex = 0;
			ParseCommand (command);

			m_inputFiled.text = "";
		}

		if (m_commandHistoryCache != null &&
			Input.GetKeyDown (KeyCode.UpArrow)) {
			m_inputFiled.text = GetCommandHistroy (true);
		}

		if (m_commandHistoryCache != null &&
			Input.GetKeyDown (KeyCode.DownArrow)) {
			m_inputFiled.text = GetCommandHistroy (false);
		}

	}

	private string GetCommandHistroy (bool isPlus)
	{
		if (isPlus) {
			m_commandHistoryIndex++;
		} else {
			m_commandHistoryIndex--;
		}
		if (m_commandHistoryIndex < 0) {
			m_commandHistoryIndex = 0;
		}
		if (m_commandHistoryIndex >= m_commandHistoryCache.Length) {
			m_commandHistoryIndex = m_commandHistoryCache.Length - 1;
		}
		return m_commandHistoryCache [m_commandHistoryIndex];
	}

	private void PrintLog (string log)
	{
		m_log.text += log + "\n";
	}

	private void ParseCommand (string text)
	{
		bool isReadyChangeScene = false;
		text = text.ToLower ();
		string [] words = text.Split (' ');

		foreach (string word in words) {
			if (word == "loadscene") {
				isReadyChangeScene = true;
				continue;
			}
			if (isReadyChangeScene) {
				isReadyChangeScene = false;

				bool isSceneExist = SceneLookup.IsSceneExist (word, false);
				if (isSceneExist) {
					StarPlatinum.PrefabManager.Instance.LoadScene (SceneLookup.GetEnum (word, false), UnityEngine.SceneManagement.LoadSceneMode.Additive);
				} else {
					PrintLog ("Scene [" + word + "] Is Not Exist! Please check scene name again!");
					PrintLog ("All Available Scene");
					PrintAllValidSceneName ();
				}
				continue;
			}
		}
		if (isReadyChangeScene) {
			isReadyChangeScene = false;
			PrintLog ("All Available Scene");
			PrintAllValidSceneName ();
		}
	}

	private void PrintAllValidSceneName ()
	{
		string [] sceneList = SceneLookup.GetAllSceneString ();
		foreach (var sceneName in sceneList) {
			PrintLog (sceneName);
		}
	}
}
