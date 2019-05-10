using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewOffsetHorizontal : MonoBehaviour {

   // public float tileSizeZ;
    //public float scrollSpeed = 0.5F;

    //private Vector2 savedOffset;
    private Vector3 startPosition;
    private Renderer render;
    Camera m_camera;
    void Start()
    {
        m_camera = Camera.main;
        //startPosition = transform.position;
        startPosition = m_camera.transform.position;
        //float width = GetComponent<SpriteRenderer>().bounds.size.x;
        //savedOffset = GetComponent<Renderer>().sharedMaterial.mainTextureOffset;
        render = GetComponent<Renderer>();
    }

    void Update()
    {
        //m_camera.transform.position;
        render.material.SetFloat("_Distance",(m_camera.transform.position.x- startPosition.x)/70);
        transform.position = new Vector3(transform.position.x, m_camera.transform.position.y-4f, 0);
        //float x = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ * 4);
        //x = x / tileSizeZ;
        //x = Mathf.Floor(x);
        //x = x / 4;
        //Vector2 offset = new Vector2(x, savedOffset.y);
        //render.sharedMaterial.SetTextureOffset("_MainTex", offset);
        //float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        //transform.position = startPosition + Vector3.back * newPosition;
    }

    //void OnDisable()
    //{
    //    render.sharedMaterial.mainTextureOffset = savedOffset;
    //}
}
