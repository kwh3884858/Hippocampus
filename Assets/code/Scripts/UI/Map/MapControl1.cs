using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControl1 : MonoBehaviour
{
    // Start is called before the first frame update
    public Image map2l;
    public int FloorNum;//地图显示楼层数
    public GameObject gameMap;//地图总组件
    public static MapControl1 MapInstance;//地图控制全局变量
    bool MapActive;//控制地图是否打开
    void Start()
    {
        Init();
    }
    public void Init()//初始化
    {
        gameMap.SetActive(false);
        MapActive = false;
        FloorNum = 1;

    }
    public void SetMapActive()
    {
        MapActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        gameMap.SetActive(MapActive);
        ChangeMap();
    }
    public void ChangeMap()
    {
        if (FloorNum == 1)
        {
            map2l.color = new Color(255, 255, 255, 0);
        }
        if (FloorNum == 2)
        {
            map2l.color = new Color(255, 255, 255, 255);
        }
    }
    public void OnBtnClose()
    {
        MapActive = false;
    }
    public void OnBtnFloor1()
    {
        FloorNum = 1;
    }
    public void OnBtnFloor2()
    {
        FloorNum = 2;
    }
}
