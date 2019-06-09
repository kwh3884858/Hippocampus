using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
using UnityEngine.UI;

public class UIEquipedWeapon : UIPanel
{

	CharacterController m_hero = null;
	Text m_weaponName = null;
	public override void PanelInit ()
	{
		base.PanelInit ();

		m_hero = GameObject.Find ("Hero").GetComponent<CharacterController> ();
		if (!m_hero) {
			Debug.Log ("Cant find hero in the scene");
		}

		PollerService.Instance ().RegisterPoller (0, HandleTimerCallback);
		PollerService.Instance ().OpenPollerList (0);

		m_weaponName = transform.Find ("Weapon").GetComponent<Text> ();

	}

	public override void PanelOpen ()
	{
		base.PanelOpen ();

	}

	public override void PanelClose ()
	{
		base.PanelClose ();

	}


	bool HandleTimerCallback ()
	{
		CharacterController.WeaponState state = m_hero.GetWeaponState ();
		switch (state) {

		case CharacterController.WeaponState.SetDoor:
			m_weaponName.text = "传送门";
			break;

		case CharacterController.WeaponState.Knife:
			m_weaponName.text = "飞刀";

			break;

		case CharacterController.WeaponState.None:
			m_weaponName.text = "没装备武器，你个白痴";

			break;
		default:
			break;
		}
		return true;
	}

}
