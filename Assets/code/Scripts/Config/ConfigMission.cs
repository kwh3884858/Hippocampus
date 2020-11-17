using System.Collections;
using System.Collections.Generic;
using StarPlatinum.Base;
using UnityEngine;

namespace Config.GameRoot
{
    [CreateAssetMenu(fileName = "ConfigMission", menuName = "Config/SpawnConfigMission", order = 2)]
    public class ConfigMission : ConfigSingleton<ConfigMission>
    {
        [Header("Name Prefix")]

        public string Text_Interactable_Object_Group = "Interactables";
        public string Text_Event_Trigger_Group = "Triggers";
        public string Text_Mission_Group = "Missions";
        public string Text_Teleport_Group = "Teleports";
        public string Text_Spawn_Point_Name = "Spawn_Point";

        public string Path_To_InteractableObject = $"Assets/data/graphics/Interaction/Interaction_Interactable_Object.prefab";
        public string Path_To_WorldTrigger = $"Assets/data/graphics/Interaction/Interaction_World_Trigger.prefab";
        public string Path_To_SpawnPoint = $"Assets/data/graphics/Interaction/Interaction_Spawn_Point.prefab";
        public string Path_To_Teleport = $"Assets/data/graphics/Interaction/Teleport_Point.prefab";
        public string Path_To_AudioTrigger = $"Assets/data/graphics/Interaction/AudioTrigger.prefab";
        public string Path_To_Mission = $"Assets/data/graphics/Interaction/Interaction_Missions.prefab";

        public string Prefix_Mission_Folder = "Mission_";
        public string Prefix_Scene = "World_";
        public string Prefix_Mission_Scene = "World_Mission_";
        public string Path_To_Folder_World = "Assets/data/graphics/World";

    }
}