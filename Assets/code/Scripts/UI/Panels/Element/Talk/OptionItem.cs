using System;
using Config.Data;
using StarPlatinum;
using TMPro;
using UI.Panels.Element;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.StaticBoard.Element
{
	public class OptionItem : UIElementBase
	{
		public Action<string> OnClickItem;

		public void Init(string id,string content ,int uiType)
		{
			content =  content.Replace("…", "...");
			m_content.text = content;
			m_id = id;
			
			RefreshUIType(uiType);
		}


		private void RefreshUIType(int uiType)
		{
			if (m_type == uiType)
			{
				return;
			}
			m_type = uiType;

			var config = TalkPanelConfig.GetConfigByKey(uiType);
			if (config == null)
			{
				return;
			}
			PrefabManager.Instance.SetImage(m_bg,config.optionUnSelectedBG);
			PrefabManager.Instance.LoadAssetAsync<Sprite>(config.optionSelectedBG, (result) =>
			{
				if (result.status != RequestStatus.FAIL)
				{
					var sprite = result.result as Sprite;
					SpriteState spriteState = m_btn.spriteState;
					spriteState.pressedSprite = sprite;
					m_btn.spriteState = spriteState;

				}
			});
			PrefabManager.Instance.LoadAssetAsync<Sprite>(config.optionHoverBG, (result) =>
			{
				if (result.status != RequestStatus.FAIL)
				{
					var sprite = result.result as Sprite;
					SpriteState spriteState = m_btn.spriteState;
					spriteState.selectedSprite = sprite;
					spriteState.highlightedSprite = sprite;
					m_btn.spriteState = spriteState;
				}
			});
			Color newColor;
			if (ColorUtility.TryParseHtmlString(config.optionTextColor, out newColor))
			{
				m_content.color = newColor;
			}
		}
		
		public void OnClick()
		{
			OnClickItem?.Invoke(m_id);
		}

		[SerializeField]
		private TMP_Text m_content;
		[SerializeField] 
		private Image m_bg;

		[SerializeField] private Button m_btn;

		private string m_id;
		private int m_type = -1;
	}
}