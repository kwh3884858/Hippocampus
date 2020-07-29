using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay.Stage;
using StarPlatinum.Utility;
using StarPlatinum;
using UI.Panels.Providers.DataProviders;

namespace GamePlay.EventTrigger
{
	public class WorldTriggerCallbackTeleportPlayer : WorldTriggerCallbackBase
	{
		public bool m_isCGScene = false;

		[ConditionalField ("m_isCGScene", true)]
		[SerializeField]
		private string m_teleportGameScene = "";

		[ConditionalField ("m_isCGScene")]
		public string m_cgSceneName = "";

		[Header ("When the player teleports from another relative scene, he should spawn in this direction")]
		public Direction m_spawnDirection = Direction.Right;
		public float m_lengthBetweenTriggerAndSpwanPoint = 4.0f;

		public enum Direction
		{
			Up, Down, Left, Right
		}
		public static Dictionary<WorldTriggerCallbackTeleportPlayer.Direction, Vector3> DirecitonMapping =
			new Dictionary<WorldTriggerCallbackTeleportPlayer.Direction, Vector3>{
			{ WorldTriggerCallbackTeleportPlayer.Direction.Up, Vector3.forward },
			{ WorldTriggerCallbackTeleportPlayer.Direction.Down, Vector3.back },
			{ WorldTriggerCallbackTeleportPlayer.Direction.Left, Vector3.left },
			{ WorldTriggerCallbackTeleportPlayer.Direction.Right, Vector3.right }
		};
		protected override void Callback ()
		{
			if (m_isCGScene) {
				UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.UICommonCgscenePanel, new CGSceneDataProvider () { CGSceneID = m_cgSceneName });
				CoreContainer.Instance.SetPlayerPosition (transform.position + DirecitonMapping [m_spawnDirection] * m_lengthBetweenTriggerAndSpwanPoint + new Vector3 (0.0f, 0.5f, 0.0f));
			} else {
				if (m_teleportGameScene == "") {
					Debug.LogError ("Teleport scene is not set. Use [Update Teleported Game Scene] to modify the value");
				}
				SceneLookupEnum scene = SceneLookup.GetEnum (m_teleportGameScene);
				if (MissionSceneManager.Instance.IsGameSceneExistCurrentMission (scene)) {
					GameSceneManager.Instance.LoadScene (scene);
					MissionSceneManager.Instance.LoadCurrentMissionScene ();
				} else {
					Debug.LogError (m_teleportGameScene + " is not exist mission " + MissionSceneManager.Instance.GetCurrentMission ().ToString ());
				}
			}
		}

		public void SetTeleportedScene(SceneLookupEnum scene)
		{
			m_teleportGameScene = scene.ToString ();
		}

		public SceneLookupEnum GetTeleportScene ()
		{
			return SceneLookup.GetEnum (m_teleportGameScene);
		}
	}
}