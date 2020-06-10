using System.Collections;
using System.Collections.Generic;
using StarPlatinum.Base;
using UnityEngine;

public enum MissionEnum
{
	None,
	DockByPier,
}

class MissionManager : Singleton<MissionManager>
{
	private MissionEnum [] ALL_MISSION = {
		MissionEnum.None,
		MissionEnum.DockByPier
	};

	public MissionEnum [] GetAllMission ()
	{
		return ALL_MISSION;
	}
	private MissionEnum m_currentMission = MissionEnum.None;
	public MissionEnum GetCurrentMission ()
	{
		return m_currentMission;
	}
	public bool SetCurrentMission (string mission)
	{
		MissionEnum result = GetMissionEnumBy (mission);
		if (result == MissionEnum.None) {
			return false;
		} else {
			return true;
		}
	}

	public MissionEnum GetMissionEnumBy (string mission, bool isMatchCase = true)
	{
		foreach (MissionEnum item in ALL_MISSION) {
			if (isMatchCase) {
				if (item.ToString () == mission) {
					return item;
				}
			} else {
				if (item.ToString ().ToLower () == mission.ToLower ()) {
					return item;
				}
			}

		}

		return MissionEnum.None;
	}

	public bool IsMissionExist (MissionEnum mission)
	{
		foreach (var item in ALL_MISSION) {
			if (item == mission) {
				return true;
			}
		}

		return false;
	}
}