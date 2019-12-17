using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class UITestOne : MonoBehaviour
{
	[SerializeField] private UIManager m_uiManager;
	// Use this for initialization
	void Start () {
		m_uiManager.Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		m_uiManager.Tick();
	}
}
