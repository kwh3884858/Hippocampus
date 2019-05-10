using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace Skylight
{
	public abstract class UIBox : UIPanel
	{
		// Use this for initialization
		public virtual void Start ()
		{
			AddClickEvent ("Mask", OnMaskClick);
			AddButtonClick ("Content/Close", UIClose);
			AddButtonClick ("Content/Cancel", UIClose);
		}

		public void OnMaskClick (BaseEventData eventData)
		{
			UIClose ();
		}

		public void UIClose ()
		{
			UIManager.Instance ().CloseBox ();
		}
	}
}
