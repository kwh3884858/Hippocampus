
using StarPlatinum;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using StarPlatinum.EventManager;
using System;
using Controllers.Subsystems;
using Controllers.Subsystems.Role;
using LocalCache;
using GamePlay.Stage;

namespace GamePlay.Global
{
	public class SingletonGlobalDataContainer : Singleton<SingletonGlobalDataContainer>
	{
		public void Initialize ()
		{
			EventManager.Instance.AddEventListener<PlayerPreSaveArchiveEvent> (OnPlayerPreSaveArchive);
			EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent> (OnPlayerLoadArchive);
		}

		public void Shutdown ()
		{
			EventManager.Instance.RemoveEventListener<PlayerPreSaveArchiveEvent> (OnPlayerPreSaveArchive);
			EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent> (OnPlayerLoadArchive);
		}

		public bool RegisterNewObject (string item)
		{
			int tmp;
			if (m_objectTriggeredCounter.TryGetValue (item, out tmp)) {
				return false;
			}
			m_objectTriggeredCounter.Add (item, 0);
			return true;
		}

		public bool GetObjectCounter (string gameObjectName, out int outValue)
		{
			return m_objectTriggeredCounter.TryGetValue (gameObjectName, out outValue);
		}

		public bool ModifyCounterValue (string gameObjectName, int modifiedValue)
		{
			int tmp;
			if (m_objectTriggeredCounter.TryGetValue (gameObjectName, out tmp)) {
				m_objectTriggeredCounter [gameObjectName] += modifiedValue;
				if (m_objectTriggeredCounter [gameObjectName] < 0) {
					m_objectTriggeredCounter [gameObjectName] = 0;
				}
				return true;
			} else {
				return false;
			}
		}

		//-----------------------------------------------
		public bool IsStoryTriggered (string storyId)
		{
			return m_isStoryTriggered.Contains (storyId);
		}

		public void AddtTriggeredStory (string storyId)
		{
			m_isStoryTriggered.Add (storyId);
		}

		//-----------------------------------------------
		private void OnPlayerPreSaveArchive (object sender, PlayerPreSaveArchiveEvent e)
		{
            MissionArchiveData data = new MissionArchiveData();

			Vector3 pos = CoreContainer.Instance.GetPlayerPosition();
			data.PlayerPositionX = pos.x;
			data.PlayerPositionY = pos.y;
			data.PlayerPositionZ = pos.y;

            data.CurrentGameScene = GamePlay.Stage.GameSceneManager.Instance.GetCurrentSceneEnum();
            data.CurrentMission = GamePlay.Stage.MissionSceneManager.Instance.GetCurrentMissionEnum();
			data.StoryTriggered = m_isStoryTriggered;
			data.ObjectTriggeredCounter = m_objectTriggeredCounter;

			GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.MissionArchieData = data;

		}

		private void OnPlayerLoadArchive (object sender, PlayerLoadArchiveEvent e)
        {
			MissionArchiveData data = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.MissionArchieData;

			SceneLookupEnum gameSceneEnum = data.CurrentGameScene;
            MissionEnum missionEnum = data.CurrentMission;
            if (gameSceneEnum != SceneLookupEnum.World_Invalid && gameSceneEnum != SceneLookupEnum.World_GameRoot && missionEnum != MissionEnum.None)
            {
				Vector3 PlayerPos = new Vector3(data.PlayerPositionX, data.PlayerPositionY, data.PlayerPositionZ);
				CoreContainer.Instance.SetPlayerPosition(PlayerPos);
				GameSceneManager.Instance.LoadScene(gameSceneEnum, "", () =>
				{
                    if (MissionSceneManager.Instance.IsMissionSceneExist(missionEnum))
                    {
                        MissionSceneManager.Instance.LoadMissionScene(missionEnum);
                    }
                    else
                    {
                        MissionSceneManager.Instance.LoadMissionScene(MissionEnum.None);
                    }
                });
            }
            else
            {
				Debug.LogError("Game Scene Enum: " + missionEnum + "nMission Scene Enum:" + gameSceneEnum + "\nIs not exist.");
			}

			m_isStoryTriggered = data.StoryTriggered;
			m_objectTriggeredCounter = data.ObjectTriggeredCounter;
			if (m_isStoryTriggered == null) {
				m_isStoryTriggered = new List<string> ();
			}
			if (m_objectTriggeredCounter == null) {
				m_objectTriggeredCounter = new Dictionary<string, int> ();
			}
		}

		//After switch scene, destory already triggered story.
		public List<string> m_isStoryTriggered = new List<string> ();

		//Interactable objects trigger counter to calculate the num of be triggered for showing the correct story.
		public Dictionary<string, int> m_objectTriggeredCounter = new Dictionary<string, int> ();

	}
}