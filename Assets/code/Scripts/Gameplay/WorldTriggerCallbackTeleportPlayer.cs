using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay.Stage;

namespace GamePlay.EventTrigger
{
	public class WorldTriggerCallbackTeleportPlayer : WorldTriggerCallbackBase
	{
		public SceneLookupEnum m_teleportGameScene;
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
			if (MissionSceneManager.Instance.IsGameSceneExistCurrentMission (m_teleportGameScene)) {
				GameSceneManager.Instance.LoadScene (m_teleportGameScene);
				MissionSceneManager.Instance.LoadCurrentMissionScene ();
			} else {
				Debug.LogError (m_teleportGameScene.ToString () + " is not exist mission " + MissionSceneManager.Instance.GetCurrentMission ().ToString ());
			}
		}

	}
}