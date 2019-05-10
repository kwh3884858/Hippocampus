using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Skylight;

//public class ShootJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
//{

//	public Image ImgBg;
//	public Image ImgJoystick;


//	public BaseVector3Input m_vector3Input;

//	private Vector3 _inputVector;
//	//public Vector3 InputVector
//	//{
//	//    get
//	//    {
//	//        return _inputVector;
//	//    }
//	//}


//	public void OnPointerDown (PointerEventData e)
//	{

//		//Vector2 vector2;
//		//RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), e.position, e.pressEventCamera, out vector2);
//		//Debug.Log(vector2);
//		//ImgBg.rectTransform.position = vector2;
//		OnDrag (e);
//	}

//	public void OnDrag (PointerEventData e)
//	{
//		Vector2 pos;
//		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (ImgBg.rectTransform, e.position, e.pressEventCamera, out pos)) {

//			pos.x = (pos.x / ImgBg.rectTransform.sizeDelta.x);
//			pos.y = (pos.y / ImgBg.rectTransform.sizeDelta.y);

//			_inputVector = new Vector3 (pos.x * 2 + 1, pos.y * 2 - 1, 0);
//			_inputVector = (_inputVector.magnitude > 1.0f) ? _inputVector.normalized : _inputVector;
//			//解耦，给inputservice发送消息，而不是直接通过移动组件直接调用
//			m_vector3Input.Value = _inputVector;

//			ImgJoystick.rectTransform.anchoredPosition = new Vector3 (_inputVector.x * (ImgBg.rectTransform.sizeDelta.x * .4f),
//																	 _inputVector.y * (ImgBg.rectTransform.sizeDelta.y * .4f));
//		}
//	}
//	public void OnPointerUp (PointerEventData e)
//	{
//		_inputVector = Vector3.zero;
//		m_vector3Input.Value = _inputVector;
//		ImgJoystick.rectTransform.anchoredPosition = Vector3.zero;
//	}


//	public float Horizontal ()
//	{
//		if (_inputVector.x != 0) {
//			return _inputVector.x;
//		}

//		return Input.GetAxis ("Horizontal");
//	}

//	public float Vertical ()
//	{
//		if (_inputVector.z != 0) {
//			return _inputVector.z;
//		}

//		return Input.GetAxis ("Vertical");
//	}

//}