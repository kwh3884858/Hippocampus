using UnityEngine;
using System.Collections;
using UI;
using UI.Panels.Providers.DataProviders.GameScene;

public class ReporterGUI : MonoBehaviour
{
	Reporter reporter;
	void Awake()
	{
		reporter = gameObject.GetComponent<Reporter>();
	}

	void OnGUI()
	{
		reporter.OnGUIDraw();
		
		//if (GUI.Button(new Rect(0, 150, 100, 50), "ShowLog"))
		//{
		//	reporter.show = !reporter.show; 
		//}
		if (GUI.Button(new Rect(0, 150, 100, 50), "争锋相对"))
		{
			UIManager.Instance().ShowPanel(UIPanelType.UIJudgmentControversyPanel,new ControversyDataProvider(){ID = "101"});
		}
	}
}
