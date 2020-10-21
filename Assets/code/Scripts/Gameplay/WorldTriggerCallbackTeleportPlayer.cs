using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay.Stage;
using StarPlatinum.Utility;
using StarPlatinum;
using UI.Panels.Providers.DataProviders;
using GamePlay.Utility;

namespace GamePlay.EventTrigger
{
	public class WorldTriggerCallbackTeleportPlayer : WorldTriggerCallbackBase
	{
		public bool m_isCGScene = false;

		[ConditionalField ("m_isCGScene", true)]
		[SerializeField]
		private string m_teleportedGameScene = "";
		public string m_specificTeleportName = "";

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

		protected override void AfterStart() 
		{
#if UNITY_EDITOR
			//Debug 3D Text
			if (m_isCGScene)
            {

            }
            else {
				WorldDebug3DTextManager.Instance.AddTextToGameobject(m_teleportedGameScene, gameObject);
			}
#endif
		}
		protected override void Callback ()
		{
            UI.UIManager.Instance().ShowPanel(UIPanelType.UICommonGameplayTransitionPanel, new CommonGamePlayTransitionDataProvider { m_animationTranstionType = UI.Panels.CommonGamePlayTransitionPanel.AnimationType.FadeIn });

            if (m_isCGScene) {
				UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.UICommonCgscenePanel, new CGSceneDataProvider () { CGSceneID = m_cgSceneName });
				CoreContainer.Instance.SetPlayerPosition (transform.position + DirecitonMapping [m_spawnDirection] * m_lengthBetweenTriggerAndSpwanPoint + new Vector3 (0.0f, 0.5f, 0.0f));
			} else {
				if (m_teleportedGameScene == "") {
					Debug.LogError ("Teleport scene is not set. Use [Update Teleported Game Scene] to modify the value");
				}
				SceneLookupEnum scene = SceneLookup.GetEnum (m_teleportedGameScene);
				GameSceneManager.Instance.LoadScene(scene, m_specificTeleportName, delegate()
				{
					MissionSceneManager.Instance.LoadCurrentMissionScene(MissionSceneManager.LoadMissionBy.Teleport);
				});
			}
		}

		public void SetTeleportedScene(SceneLookupEnum scene)
		{
			m_teleportedGameScene = scene.ToString ();
		}

		public SceneLookupEnum GetTeleportScene ()
		{
            if (SceneLookup.IsSceneExist(m_teleportedGameScene, false))
            {
				return SceneLookup.GetEnum (m_teleportedGameScene);
            }
            else
            {
				return SceneLookupEnum.World_Invalid;
            }
		}

		public bool IsCGScene ()
		{
			return m_isCGScene;
		}
	}
}