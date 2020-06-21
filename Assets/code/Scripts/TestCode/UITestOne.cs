using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Config.Data;
using Controllers;
using Game;
using StarPlatinum;
using UnityEngine;
using UIManager = UI.UIManager;

public class UITestOne : MonoBehaviour,IGameRuntimeData
{
	public ControllerManager ControllerManager => m_controllerManager;
	public ConfigProvider ConfigProvider => m_configProvider;
	public SoundService SoundService => SoundService.Instance;
	public ConfigDataProvider ConfigDataProvider => m_configDataProvider;

	public GameState State { get; private set; }

	// Use this for initialization
	void Start ()
	{
		State = GameState.Invalid;
		LoadConfig();
		m_gameRunTimeData = new GameRunTimeData(this);
		m_gameRunTimeData.State = State;
		m_uiManager.Initialize(m_gameRunTimeData);
		StartCoroutine(InitializeControllerByArgs());
	 	StartCoroutine(LoadGameStateAsync(m_startState));
	}

	private IEnumerator LoadGameStateAsync(GameState state)
	{
		State = state;
		m_uiManager.ActivatState(State);
		Debug.Log($"!!!!!!!!!!!!!!!!!!!!!!Current state:{m_gameRunTimeData.State}");
		yield break;
	}

	private IEnumerator InitializeControllerByArgs()
	{
		var res = m_controllerManager.InitializedControllers(m_gameRunTimeData);
		while (!res.IsCompleted)
		{
			yield return null;
		}

		if (res.Result == ControllerinitializedStep.Success)
		{
			Debug.Log("Controllers initialized!!!!!");
		}
		else
		{
			Debug.Log("Controllers initialize failure!!!!!");
			m_controllerManager.TerminateEverything("Some error occured on controllers", true);
		}
	}
	
	private void LoadConfig()
	{
		m_configProvider = new ConfigProvider();
		m_configDataProvider = new ConfigDataProvider();
		
		StoryConfig.PreloadInGameConfig();
		m_configDataProvider.InitialInfo();
	}
	// Update is called once per frame
	void Update () {
		m_uiManager.Tick();
		m_controllerManager.Tick();
	}

	private void OnDestroy()
	{
		m_uiManager.DeInitialize();
        if (m_configDataProvider != null)
        {
            m_configDataProvider.Dispose();
        }
        m_controllerManager.TerminateEverything("Game end");
	}
	[SerializeField] private GameState m_startState;
	[SerializeField] private UIManager m_uiManager;
	[SerializeField] private ControllerManager m_controllerManager;

	private GameRunTimeData m_gameRunTimeData;
	private ConfigProvider m_configProvider;
	private ConfigDataProvider m_configDataProvider;
}
