
using UnityEngine;

using StarPlatinum.Services;
using StarPlatinum.Base;
using System.Collections.Generic;
using GamePlay.Stage;

namespace Config.GameRoot
{
	/// <summary>
	/// Only for editor
	/// </summary>
	[CreateAssetMenu (fileName = "ConfigRoot", menuName = "Config/SpawnConfigRoot", order = 1)]
	public class ConfigRoot : ConfigSingleton<ConfigRoot>
	{
		[Header ("Start Scene")]
		public SceneLookupEnum StartScene;

		[Header ("Start Mission")]
		public MissionEnum StartMission;
	}
}