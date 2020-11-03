using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITachieTest : MonoBehaviour
{
    public Slider m_verticalSlider;
    public Slider m_horizonSlider;
    public RectTransform m_pic;

    // Start is called before the first frame update
    void Start()
    {
        m_verticalSlider = GameObject.Find("VerticalSlider").GetComponent<Slider>();
        m_horizonSlider = GameObject.Find("HorizonSlider").GetComponent<Slider>();
        m_pic = GameObject.Find("Pic").transform.rectTransform();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        GUIStyle thumb = new GUIStyle();
        thumb.normal.background = Texture2D.whiteTexture;    //设置背景填充
        GUI.skin.horizontalSlider = new GUIStyle(thumb);
        m_verticalSlider.value = GUI.HorizontalSlider(new Rect(560, 200, 300, 10), m_verticalSlider.value, 0, 100);
        m_horizonSlider.value = GUI.HorizontalSlider(new Rect(560, 140, 300, 10), m_horizonSlider.value, 0.0f, 100.0f);

        GUIStyle fontStyle = new GUIStyle();
        fontStyle.normal.background = null;    //设置背景填充
        fontStyle.normal.textColor = Color.white;   //设置字体颜色
        fontStyle.fontSize = 40;       //字体大小

        float picX = 19.2f * m_horizonSlider.value - 960;
        float picY = 12f * (m_verticalSlider.value-5) - 540;
        m_pic.anchoredPosition = new Vector2(picX,picY);
        GUI.Label(new Rect(960, 200, Screen.width, 100), "纵向坐标值：" + m_verticalSlider.value,fontStyle);
        GUI.Label(new Rect(960, 140, Screen.width, 100), "横向坐标值：" + m_horizonSlider.value, fontStyle);
    }
}
