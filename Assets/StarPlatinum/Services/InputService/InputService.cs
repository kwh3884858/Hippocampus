using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StarPlatinum
{
	public enum InputType
	{
		UnKnown,
		Axis,
		Button,

	}

    /// <summary>
    /// Define all behavior
    /// </summary>
	public enum KeyMap
	{
		Confirm,
		Cancel,
		Horizontal,         //Axis
		Vertical            //Axis

	}

	/// <summary>
	/// Environment type for scene.
	/// 每个场景都场景定一个操作环境
	/// 例如：战斗场景和对话场景，使用的就是一套不同的操作环境。
	/// UI也被归入到一个场景中，场景加载时，根据数据导入需要的UI模块。
	/// </summary>
	public enum Environment
	{
		UI = 0,             //UI场景
		Story,              //故事场景
		Battle,             //战斗场景

	}

	public enum Behavior
	{
		Horizontal = 0,
		Vertical,

	}


	/// <summary>
	/// 输入服务
	/// InputService
	/// 输入服务通过暴露简单的接口隐藏不同设备之间的输入差异
	/// Input services hide input differences between different devices by exposing simple interfaces
	/// 
	/// 在不同的场景中切换的时候，会遇到多个运行的场景调用同一个键位的情况。
	/// 例如在即时战斗中，确定键是作为攻击键的，
	/// 但是在剧情场景中，确定键作为对话键，
	/// 为了能够切换这些场景，框架提供一个*输入环境的栈*
	/// 每次新的环境压入时，都会只相应这个环境的输入请。
	/// 例如，当在游戏中开启菜单时，确定键只会被菜单获取到。
	/// 尽管游戏场景也在运行，但是不会获取到对应的操作。
	/// When switching between different environment, 
	/// you will encounter multiple scenes that call the same key. 
	/// For example, in an battle, 
	/// the key is determined as an attack key, 
	/// but in the scenario environment, the key is determined as a dialog key, 
	/// and in order to be able to switch between these scenes, 
	/// the framework provides a stack of *input environments*. 
	/// Every time a new environment is pushed in, 
	/// it will only be input for this environment. 
	/// For example, when the menu is opened in the game, 
	/// the comfirm button will only be retrieved by the menu. 
	/// Although the game scene is also running, 
	/// it will not get the corresponding operation.
	/// 
	/// </summary>
	/// 
	public class InputService : Singleton<InputService>
	{

        public string[] m_PCkeyCode =
{
        "Confirm",
        "Cancel",
        "Horizontal",
        "Vertical"
    };

        private InputControl m_inputControl;

		/// <summary>
		/// The is environment stack.
		/// 这是环境栈，所有的场景，UI在定义时候，都会有一个对应的环境栈
		/// 注册之前，需要往其中压入对应的栈
		/// </summary>
		static private Stack<Environment> m_envStack = new Stack<Environment> ();

		static private Dictionary<KeyMap, BaseInput> m_inputs = new Dictionary<KeyMap, BaseInput> ();

		//InputService ()
		//{


		//}


		public InputService()
		{


			//SwitchDeviceInput (DeviceType.Desktop, true);
			//让移动模块来决定自己需要的是哪一种输出
			m_inputControl = new InputControl();
			//初始化的时候会把电脑端的操作开启，并且没有提供特别的对应桌面的输入关闭
			m_inputControl.Init();

			//m_inputs.Add (KeyMap.A, new ButtonInput (KeyCode.A.ToString ()));
			//m_inputs.Add (KeyMap.D, new ButtonInput (KeyCode.D.ToString ()));

            // For PC
			m_inputs.Add(KeyMap.Horizontal, new AxisInput(m_PCkeyCode[(int)KeyMap.Horizontal]));
			m_inputs.Add(KeyMap.Vertical, new AxisInput(m_PCkeyCode[(int)KeyMap.Vertical]));

			//EventManager.Instance().AddEventListener<ButtonDownEvent>(HandleButtonDownEvent);
			//EventManager.Instance().AddEventListener<ButtonUpEvent>(HandleButtonUpEvent);


		}


		//void HandleButtonUpEvent (object sender, ButtonUpEvent e)
		//{
		//	switch (e.m_buttonName) {
		//	case "A":
		//	case "a":
		//	case "d":
		//	case "D":
		//		SetAxis (KeyMap.Horizontal, 0f);
		//		break;

		//	case "W":
		//	case "w":
		//	case "S":
		//	case "s":
		//		SetAxis (KeyMap.Vertical, 0f);
		//		break;

		//	}
		//}

		//void HandleButtonDownEvent (object sender, ButtonDownEvent e)
		//{
		//	//KeyMap a = KeyMap.A.name();

		//	switch (e.m_buttonName) {
		//	case "A":
		//	case "a":
		//		AddAxis (KeyMap.Horizontal, -1f);

		//		break;


		//	case "d":
		//	case "D":
		//		AddAxis (KeyMap.Horizontal, 1f);

		//		break;

		//	case "W":
		//	case "w":
		//		AddAxis (KeyMap.Vertical, 1f);

		//		break;


		//	case "S":
		//	case "s":
		//		AddAxis (KeyMap.Vertical, -1f);

		//		break;

		//	case "J":
		//	case "j":
		//		SetInput (KeyMap.Knife, true);
		//		break;

		//	case "K":
		//	case "k":
		//		SetInput (KeyMap.Door, true);
		//		break;


		//	case "N":
		//	case "n":
		//		SetInput (KeyMap.Flash, true);
		//		break;
		//	}
		//}


		public InputControl Input {
			get {
				return m_inputControl;
			}
		}

		public void SetInput (KeyMap map, bool isPress)
		{
			(m_inputs [map] as ButtonInput).Value = isPress;
		}
		public bool GetInput (KeyMap map)
		{
			return (m_inputs [map] as ButtonInput).Value;
		}

		//public void AddAxis (KeyMap map, float value)
		//{
		//	(m_inputs [map] as AxisInput).Value += value;
		//	Mathf.Clamp ((m_inputs [map] as AxisInput).Value, -1.0f, 1.0f);
		//}
		public void SetAxis (KeyMap map, float value)
		{
			(m_inputs [map] as AxisInput).Value = value;
		}
		public float GetAxis (KeyMap map)
		{
			return (m_inputs [map] as AxisInput).Value;

		}

		/// <summary>
		/// Gets the input.
		/// 获取一个输入
		/// </summary>
		/// <returns>The input.</returns>
		/// <param name="environment">Environment.环境，应该由框架来处理并传入</param>
		/// <param name="inputType">Input type.开发者需要指定一个想获取的输入类型</param>
		//public float GetInput (Environment environment, InputType inputType)
		//{

		//	if (m_envStack.Peek () != environment) {

		//		return m_inputControl.m_buttonValue [(int)inputType];
		//	} else {
		//		Debug.Log ("Type error!");
		//		return 0;

		//	}
		//}

		/// <summary>
		/// Sets the input.
		/// 设置一个输入
		/// </summary>
		/// <param name="environment">Environment.环境，应该由框架来处理并传入</param>
		/// <param name="inputType">Input type.开发者需要指定一个想获取的输入类型</param>
		/// <param name="value">Value.需要传入该类型的数值</param>
		//public void SetInput (Environment environment, InputType inputType, float value)
		//{
		//	try {
		//		if (m_envStack.Peek () != environment) {
		//			throw new System.Exception ("Environment type error!");
		//		}
		//		m_inputControl.m_buttonValue [(int)inputType] = value;
		//	} catch (System.Exception e) {
		//		Debug.Log (e.Message);
		//	}

		//}
		/// <summary>
		/// Pushing a the environment into stack，
		/// 推入一个环境变量到栈顶。
		/// Attention, this interface is used by SceneManager
		/// you need to judge whether the top of stack is already the same type
		/// If it is the same type, it is not pushed onto the stack
		/// 请注意，这个接口由框架中的场景管理器调用
		/// 推入一个环境类型，需要判断最上层是否已经是相同的类型。
		/// 如果是相同类型，则不推入栈中，提示错误
		/// </summary>
		/// <param name="environment">Environment.环境类型</param>
		private bool PushEnvironment (Environment environment)
		{
			if (m_envStack.Peek () == environment) {
				Debug.Log ("Error, top of stack already have a same type");
				return false;
			} else {
				m_envStack.Push (environment);
				return true;
			}
		}

		/// <summary>
		/// Poping a environment type out stack.
		/// 从栈顶弹出一个环境变量。
		/// Attention, this interface is used by SceneManager
		/// 请注意，这个接口由框架中的场景管理器调用
		/// 推出一个环境类型，需要判断在栈顶是否存在相同的类型。
		/// 如果不存在，则提示错误
		/// </summary>
		/// <param name="environment">Environment.环境类型</param>
		private bool PopEnvironment (Environment environment)
		{
			if (m_envStack.Peek () == environment) {
				m_envStack.Pop ();
				return true;
			} else {
				Debug.Log ("Error, top of stack doesn`t have " + environment +
				" type.");
				return true;
			}
		}
	

	}

}
