using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnloadDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
      if(  GUI.Button(new Rect(0,0,100,100),"Unload Scene"))
        {
            Skylight.SceneManager.Instance().UnloadScene(SceneLookupEnum.SceneKnifeAndTransformDoor.ToString());

        }
    }
}
