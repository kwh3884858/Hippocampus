using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Skylight
{
	public abstract class UIOverlay : UIPanel
	{

        public abstract void Callback();

        public abstract void ShowMsg(string msg);
        //Image m_image;
        //Text m_text;


        //List<Component> m_components;
        //public override void PanelOpen()
        //{
        //    base.PanelOpen();
        //    m_text = transform.Find("Background/MsgText").GetComponent<Text>();
        //    m_image = transform.Find("Background").GetComponent<Image>();
        //}

        //public void ShowMsg(string msg,float fadeTime, Callback callback = null,int offsetX = 0, int offsetY = 0)
        //{
        //    m_text.text = msg;
        //    m_text .CrossFadeAlpha(0, fadeTime, true);
        //    m_image.CrossFadeAlpha(0, fadeTime, true);

        //    FadeImage(fadeTime, DealCallback);
        //}

        //private void DealCallback()
        //{
        //    string[] strings = {"I", "am", "HERO"};
        //    foreach(string i in strings)
        //    {
        //        Debug.Log(i);
        //    }
        //}

        //IEnumerator FadeImage(float delayTime,Callback callback)
        //{
        //    yield return new WaitForSeconds(delayTime);
        //}
        //IEnumerator FadeImage(bool fadeAway)
        //{
        //    // fade from opaque to transparent
        //    if (fadeAway)
        //    {
        //        // loop over 1 second backwards
        //        for (float i = 1; i >= 0; i -= Time.deltaTime / 2)
        //        {
        //            // set color with i as alpha
        //            background.color = new Color(0, 0, 0, i);
        //            yield return null;
        //        }
        //    }
        //    // fade from transparent to opaque
        //    else
        //    {
        //        // loop over 1 second
        //        for (float i = 0; i <= 1; i += Time.deltaTime / 2)
        //        {
        //            // set color with i as alpha
        //            background.color = new Color(0, 0, 0, i);
        //            yield return null;
        //        }
        //    }
        //    Skylight.UIManager.Instance().ClosePanel<UIBlackCurtain>();
        //}
    }

}
