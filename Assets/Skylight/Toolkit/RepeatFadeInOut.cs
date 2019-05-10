using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepeatFadeInOut : MonoBehaviour {


    Image m_spriteRenderer;
    float time;

    public float m_loopTime = 1000;
    public float m_alphaHeap = 1;
    public float m_alphafrequency = 1;
	// Use this for initialization
	void Start () {
        m_spriteRenderer = GetComponent<Image>();
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time > m_loopTime)
        {
            time = 0;
        }

        m_spriteRenderer.color =new Color (
            m_spriteRenderer.color.r ,
            m_spriteRenderer.color.g,
            m_spriteRenderer.color.b,
            (Mathf.Sin(time * m_alphafrequency) * m_alphaHeap + 1 )/2)  ;
	}
}
