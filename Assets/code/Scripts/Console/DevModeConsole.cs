using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Stage;
using StarPlatinum.Manager;
using UnityEngine;
using UnityEngine.UI;


namespace StarPlatinum.Development
{
	public class DevModeConsole : MonoBehaviour
	{
		public GameObject m_parentPanel;
		public InputField m_inputFiled;

		public Text m_log;

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
			bool isReadyLoadMission = false;
			text = text.ToLower ();
			string [] words = text.Split (' ');

			foreach (string word in words) {
				if (word.ToLower () == "loadscene") {
					isReadyChangeScene = true;
				} else if (isReadyChangeScene) {
					isReadyChangeScene = false;

					if (!LoadScene (word)) {
						PrintLog ("Load Scene Failed");
					}

				} else if (word.ToLower () == "loadmission") {
					isReadyLoadMission = true;
				} else if (isReadyLoadMission) {
					isReadyLoadMission = false;
					MissionEnum requestMission = MissionSceneManager.Instance.GetMissionEnumBy (word, false);
					if (requestMission != MissionEnum.None) {
                        if (MissionSceneManager.Instance.LoadMissionScene(requestMission)){
                            MissionSceneManager.Instance.SetCurrentMission(word);
                        }
                        else{
							PrintLog ("Mission Scene [" + word + "] Is Not Exist! Please check mission name again!");
							PrintAllValidMission ();
						}
					} else {
						PrintLog ("[" + word + "] cant find related mission enumeration! Please check mission name again!");
						PrintAllValidMission ();
					}

				}
			}
			if (isReadyChangeScene) {
				isReadyChangeScene = false;
				PrintAllValidSceneName ();
			}
			if (isReadyLoadMission) {
				isReadyLoadMission = false;
				PrintAllValidMission ();
			}
		}

		private bool LoadScene (string sceneName)
		{
			bool isSceneExist = SceneLookup.IsSceneExistNoMatchCase (sceneName);
			if (isSceneExist) {
                SceneLookupEnum scene = SceneLookup.GetEnum(sceneName, false);
                GameSceneManager.Instance.LoadScene(scene);
				//StarPlatinum.PrefabManager.Instance.LoadScene(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
				return true;
			} else {
				PrintLog ("Scene [" + sceneName + "] Is Not Exist! Please check scene name again!");

				PrintAllValidSceneName ();
				return false;
			}
		}

		private void PrintAllValidSceneName ()
		{
			PrintLog ("All Available Scene");
			string [] sceneList = SceneLookup.GetAllSceneString ();
			foreach (var sceneName in sceneList) {
				PrintLog (sceneName);
			}
		}

		private void PrintAllValidMission ()
		{
			PrintLog ("All Available Mission:");
			foreach (var sceneName in MissionSceneManager.Instance.GetAllMission ()) {
				PrintLog (sceneName.ToString ());
			}
		}

        private QueueArray<string> m_commandHistory;

        private string[] m_commandHistoryCache;
        private int m_commandHistoryIndex;

    }
}