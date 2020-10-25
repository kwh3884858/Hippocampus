using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapsController : MonoBehaviour
{

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
