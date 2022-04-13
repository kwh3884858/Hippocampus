using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCullout: MonoBehaviour
{
    [SerializeField]
    private Transform m_target;

    [SerializeField]
    private LayerMask wallmask;

    private Camera mainCamera;

    public void Refresh()
    {

        if (m_target != null)
        {
            return;
        }

        GameObject player = GameObject.Find("Hero");
        if (player == null)
        {
            m_target = null;
            return;

        }

        m_target = player.transform;
    }

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
        Vector2 culloutPos = mainCamera.WorldToViewportPoint(m_target.position);
        culloutPos.y /= Screen.width / Screen.height;

        Vector2 offset = m_target.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallmask);

        for (int i=0;i<hitObjects.Length;++i)
        {
            Vector2 disIndex = m_target.position - hitObjects[i].transform.position;
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;
            for(int j =0;j<materials.Length;++j)
            {
                materials[j].SetVector("_culloutPosition", culloutPos);
                materials[j].SetFloat("_culloutSize", disIndex.magnitude * 0.1f);
                Debug.Log(materials[j].GetFloat("_culloutSize") +"cullout size"+ disIndex.magnitude * 0.1f);
                materials[j].SetFloat("_falloffSize",0.05f);
            }

        }
    }
}
