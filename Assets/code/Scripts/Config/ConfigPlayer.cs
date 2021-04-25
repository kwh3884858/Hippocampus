using StarPlatinum.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config.GameRoot
{
    [CreateAssetMenu(fileName = "ConfigPlayer", menuName = "Config/SpawnConfigPlayer", order = 2)]
    public class ConfigPlayer : ConfigSingleton<ConfigPlayer>
    {
        [Header("Search Config")]
        public Transform raycastOrigin;
        public float interactableRadius;
        public LayerMask interactableLayer;
        public int maxColliders = 10;
    }
}