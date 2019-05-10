using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Skylight
{


	public class InputControl
	{
		/// <summary>
		/// Control type.
		/// 控制类型
		/// The types of control keys are separated according to their roles, 
		/// and are logically differentiated, such as attack, 
		/// defense, determination, cancellation, and so on.
		/// 控制键的类型根据作用的不同而分开，是逻辑上的区分，例如攻击、
		/// 防御、确定、取消等。
		/// </summary>
		public enum ControlType
		{
			HorizontalMove,
			VerticalMove,
			Jump,

			Action,
			Talk,

			Attack,
			UseProp,

			MenuPanel,

			Comfirm,
			Cancel,
			Pause,
		}



		ControlType [] m_controlType = {
			ControlType.HorizontalMove,
			ControlType.VerticalMove,
			ControlType.Jump,
			ControlType.Action,
			ControlType.Talk,
			ControlType.Attack,
			ControlType.UseProp,
			ControlType.MenuPanel,
			ControlType.Comfirm,
			ControlType.Cancel,
			ControlType.Pause,
		};

		public List<float> m_buttonValue = new List<float> ();

		//public Vector3 Move {
		//	get {

		//		Vector3 dir = Vector3.zero;
		//		//移动端的优先级更高
		//		HandheldVector3Input input = (HandheldVector3Input)m_controller [ControlType.Move].m_handheldInput;
		//		if (input.Value != Vector3.zero) {
		//			return input.Value;
		//		} else {

		//			return ((DesktopVector3Input)m_controller [ControlType.Move].m_desktopInput).Value;
		//		}

		//	}
		//}

		//public bool Jump {
		//	get {
		//		return ((HandheldButtonInput)m_controller [ControlType.Jump].m_handheldInput).Value ||
		//				((DesktopJumpInput)m_controller [ControlType.Jump].m_desktopInput).Value;
		//	}
		//}

		//public bool Action {
		//	get {
		//		return ((HandheldButtonInput)m_controller [ControlType.Action].m_handheldInput).Value ||
		//																					   ((DesktopActionInput)m_controller [ControlType.Action].m_desktopInput).Value;
		//	}
		//}

		//public bool Throw {
		//	get {
		//		return ((HandheldButtonInput)m_controller [ControlType.Throw].m_handheldInput).Value ||
		//																					  ((DesktopThrowInput)m_controller [ControlType.Throw].m_desktopInput).Value;
		//	}
		//}

		//public bool SoulControl {
		//	get {
		//		return ((HandheldButtonInput)m_controller [ControlType.SoulControll].m_handheldInput).Value ||
		//																							 ((DesktopSoulControlInput)m_controller [ControlType.SoulControll].m_desktopInput).Value;
		//	}
		//}

		//public bool SoulOut {
		//	get {
		//		return ((HandheldButtonInput)m_controller [ControlType.SoulOut].m_handheldInput).Value ||
		//																						((DesktopSoulOutInput)m_controller [ControlType.SoulOut].m_desktopInput).Value;
		//	}
		//}

		//public bool Puase {
		//	get {
		//		return ((HandheldButtonInput)m_controller [ControlType.Pause].m_handheldInput).Value ||
		//																					 ((DesktopPauseInput)m_controller [ControlType.Pause].m_desktopInput).Value;
		//	}
		//}

		public void Init ()
		{
			//int deviceCount = InputService.Instance ().m_openDeviceType.Length;
			int i = 0;
			foreach (ControlType type in m_controlType) {
				m_buttonValue.Add (0);
				//switch (type) {
				//case ControlType.Move:
				//	MoveContrller moveContrller = new MoveContrller ();
				//	moveContrller.Init ();
				//	m_controller.Add (ControlType.Move, moveContrller);
				//	//m_controller.Add(ControlType.Move, )
				//	break;

				//case ControlType.Jump:
				//	JumpController jumpController = new JumpController ();
				//	jumpController.Init ();
				//	m_controller.Add (ControlType.Jump, jumpController);
				//	break;

				//case ControlType.Action:
				//	ActionContrller actionContrller = new ActionContrller ();
				//	actionContrller.Init ();
				//	m_controller.Add (ControlType.Action, actionContrller);

				//	break;

				//case ControlType.Throw:
				//	ThrowController throwController = new ThrowController ();
				//	throwController.Init ();
				//	m_controller.Add (ControlType.Throw, throwController);

				//	break;

				//case ControlType.SoulControll:
				//	SoulControlContrller soulControlContrller = new SoulControlContrller ();
				//	soulControlContrller.Init ();
				//	m_controller.Add (ControlType.SoulControll, soulControlContrller);
				//	break;

				//case ControlType.SoulOut:
				//	SoulOutContrller soulOutContrller = new SoulOutContrller ();
				//	soulOutContrller.Init ();
				//	m_controller.Add (ControlType.SoulOut, soulOutContrller);
				//	break;


				//case ControlType.Pause:
				//PauseContrller pauseContrller = new PauseContrller ();
				//pauseContrller.Init ();
				//m_controller.Add (ControlType.Pause, pauseContrller);
				//break;


				//default:
				//	break;
				//}
			}
		}



		//public void Show ()
		//{
		//	Show (m_currentMode);
		//}
		//public void Show (Mode mode)
		//{
		//	switch (mode) {
		//	case Mode.Bear:
		//		m_currentMode = mode;
		//		Show (m_bearController);
		//		break;

		//	case Mode.Deer:
		//		m_currentMode = mode;
		//		Show (m_deerController);
		//		break;

		//	case Mode.Player:
		//		m_currentMode = mode;
		//		Show (m_playerController);
		//		break;


		//	}
		//}

		//public void Show (ControlType controlType)
		//{
		//	m_controller [controlType].Show ();
		//}
		//public void Show (ControlType [] types)
		//{
		//	CloseAll ();
		//	foreach (ControlType type in types) {
		//		m_controller [type].Show ();
		//	}
		//}
		//public void Close ()
		//{
		//	Close (m_currentMode);
		//}
		//public void ShowAll ()
		//{
		//	foreach (Controller controller in m_controller.Values) {
		//		controller.Show ();
		//	}
		//}


		//public void Close (Mode mode)
		//{
		//	switch (mode) {
		//	case Mode.Bear:
		//		m_currentMode = mode;
		//		Close (m_bearController);
		//		break;

		//	case Mode.Deer:
		//		m_currentMode = mode;
		//		Close (m_deerController);
		//		break;

		//	case Mode.Player:
		//		m_currentMode = mode;
		//		Close (m_playerController);
		//		break;
		//	}
		//}

		//public void Close (ControlType controlType)
		//{
		//	m_controller [controlType].Close ();
		//}

		//public void Close (ControlType [] types)
		//{
		//	//ShowAll ();
		//	foreach (ControlType type in types) {
		//		m_controller [type].Close ();

		//	}

		//}

		//public void CloseAll ()
		//{
		//	foreach (Controller controller in m_controller.Values) {
		//		controller.Close ();
		//	}
		//}



	}

	//public abstract class Controller
	//{
	//	public ControlType m_controlType;
	//	//protected int m_desktopInput;
	//	//protected int m_handheldInput;

	//	public BaseInput m_desktopInput;
	//	public BaseInput m_handheldInput;


	//	abstract public void Init ();

	//	abstract public void Show ();

	//	abstract public void Close ();
	//}
	/*
public class JumpController : Controller
{
	override public void Init ()
	{
		m_desktopInput = new DesktopJumpInput ();

		m_handheldInput = new HandheldButtonInput ();
	}

	public override void Show ()
	{
		VirtualButton virtualButton;
		UiJumpButton uiJumpButton = UIManager.Instance ().ShowPanel<UiJumpButton> ();
		virtualButton = uiJumpButton.GetComponentInChildren<VirtualButton> ();

		virtualButton.input = (HandheldButtonInput)m_handheldInput;
	}

	public override void Close ()
	{
		UIManager.Instance ().ClosePanel<UiJumpButton> ();
	}

}

public class ActionContrller : Controller
{
	public override void Init ()
	{
		m_desktopInput = new DesktopActionInput ();
		m_handheldInput = new HandheldButtonInput ();
	}

	public override void Show ()
	{
		VirtualButton virtualButton;
		UIActionButton uIActionButton = UIManager.Instance ().ShowPanel<UIActionButton> ();
		virtualButton = uIActionButton.GetComponentInChildren<VirtualButton> ();

		virtualButton.input = (HandheldButtonInput)m_handheldInput;
	}

	public override void Close ()
	{
		UIManager.Instance ().ClosePanel<UIActionButton> ();

	}
}

public class ThrowController : Controller
{
	public override void Init ()
	{
		m_desktopInput = new DesktopThrowInput ();
		m_handheldInput = new HandheldButtonInput ();
	}

	public override void Show ()
	{
		VirtualButton virtualButton;

		UIThrowButton uIThrowButton = UIManager.Instance ().ShowPanel<UIThrowButton> ();
		virtualButton = uIThrowButton.GetComponentInChildren<VirtualButton> ();

		virtualButton.input = (HandheldButtonInput)m_handheldInput;
	}

	public override void Close ()
	{
		UIManager.Instance ().ClosePanel<UIThrowButton> ();

	}
}


public class SoulControlContrller : Controller
{
	public override void Init ()
	{
		m_desktopInput = new DesktopSoulControlInput ();
		m_handheldInput = new HandheldButtonInput ();
	}

	public override void Show ()
	{
		VirtualButton virtualButton;

		UISoulControlButton uISoulControlButton = UIManager.Instance ().ShowPanel<UISoulControlButton> ();
		virtualButton = uISoulControlButton.GetComponentInChildren<VirtualButton> ();

		virtualButton.input = (HandheldButtonInput)m_handheldInput;
	}

	public override void Close ()
	{
		UIManager.Instance ().ClosePanel<UISoulControlButton> ();

	}
}


public class SoulOutContrller : Controller
{
	public override void Init ()
	{
		m_desktopInput = new DesktopSoulOutInput ();
		m_handheldInput = new HandheldButtonInput ();
	}

	public override void Show ()
	{

		VirtualButton virtualButton;

		UISoulOutButton uISoulOutButton = UIManager.Instance ().ShowPanel<UISoulOutButton> ();
		virtualButton = uISoulOutButton.GetComponentInChildren<VirtualButton> ();

		virtualButton.input = (HandheldButtonInput)m_handheldInput;
	}

	public override void Close ()
	{
		UIManager.Instance ().ClosePanel<UISoulOutButton> ();

	}
}


public class MoveContrller : Controller
{
	public override void Init ()
	{
		m_desktopInput = new DesktopVector3Input ();
		m_handheldInput = new HandheldVector3Input ();
	}

	public override void Show ()
	{
		UIVirtualJoystickPanel uiShootJoystickPanel = UIManager.Instance ().ShowPanel<UIVirtualJoystickPanel> (true);
		VirtualJoystick m_virtualJoystick = uiShootJoystickPanel.GetComponent<VirtualJoystick> ();

		m_virtualJoystick.m_vector3Input = (BaseVector3Input)m_handheldInput;

	}

	public override void Close ()
	{
		UIManager.Instance ().ClosePanel<UIVirtualJoystickPanel> ();

	}
}

public class PauseContrller : Controller
{
	public override void Init ()
	{
		m_desktopInput = new DesktopPauseInput ();
		m_handheldInput = new HandheldButtonInput ();
	}

	public override void Show ()
	{
		UIPauseButton uiShootJoystickPanel = UIManager.Instance ().ShowPanel<UIPauseButton> ();

		uiShootJoystickPanel.m_input.input = (HandheldButtonInput)m_handheldInput;

	}

	public override void Close ()
	{
		UIManager.Instance ().ClosePanel<UIPauseButton> ();

	}
}
*/
}
