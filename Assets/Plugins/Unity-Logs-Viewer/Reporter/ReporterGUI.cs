using UnityEngine;
using System.Collections;

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
		
		if (GUI.Button(new Rect(0, 150, 100, 50), "ShowLog"))
		{
			reporter.show = !reporter.show; 
		}
	}
}
