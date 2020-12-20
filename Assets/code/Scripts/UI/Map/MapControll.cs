using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;    

//
public class MapControll : MonoBehaviour//获得地图后将MapCanUse设置为ture，改变场景时使用changeSenceFloorNum函数，将ChangeArrow，UseMap，ChangeMap函数放于update中,引用其中的方法与属性使用MapInstance
{
    // Start is called before the first frame update
    public static MapControll MapInstance;//地图控制全局变量
    public bool MapCanUse;//地图能否被使用
   
    public GameObject gameMap;//地图总组件

    public bool arrowSwitch;//是否改变主角的标识

    public GameObject arrowbox;
    public bool SceneChange;
    public Image arrow;//角色图标
    public Image map;
    public int FloorNum;//地图显示楼层数
    public int playerFloorNum;//玩家所在楼层数
    
    public int sceneNum;//由1开始依次排
    public float[] SceneLeft = new float[] { };
    public float[] SceneRight = new float[] { };
    public float playerPositon;
    public Transform player;
    public Vector3[] SceneStart = new Vector3[] { };//场景的最左边
    public Vector3[] SceneEnd = new Vector3[] { };//场景最右边
    public AudioSource MapAudio;
    public AudioClip MapOpen;
    public AudioClip MapClose;

    private bool MapActive;//地图是否显示
    private void Awake()
    {
        MapInstance = this;
    }
    void Start()
    {
        Init();
        ///测试用,测试完后删除
        //MapCanUse = true;
        arrowSwitch = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //UseMap(new Vector3(550,0,0));
        //ChangeArrow();
       // ArrowMove();
        //ChangeMap();
    }
   public  void Init()//初始化
    {
        gameMap.SetActive(false);
        MapCanUse = false;
        MapActive = false;
        arrowSwitch = false;
        SceneChange = true;
       // FloorNum = 1;//测试用
        //playerFloorNum = 2;//测试用
        ChangeArrow();
        MapAudio = gameObject.AddComponent<AudioSource>();
        //设置不一开始就播放音效
        MapAudio.playOnAwake = false;
        //加载音效文件，我把跳跃的音频文件命名为jump
        MapOpen = Resources.Load<AudioClip>("Audio/Map_Open");
        MapClose = Resources.Load<AudioClip>("Audio/Map_Close");
        playerPositon = player.localPosition.x;//为位置;

    }
    public void ChangeMap()
    {
        if (FloorNum == 1)
        {
            map.color = new Color(255, 255, 255, 0);
        }
        if (FloorNum == 2)
        {
            map.color = new Color(255, 255, 255, 255);
        }
    }
    public void ChangeArrow()//改变人物图标与朝向
    {
        if (FloorNum != playerFloorNum)
        {
            arrowbox.SetActive(false);
        }
        else
        {
            arrowbox.SetActive(true);
        }
        if (arrowSwitch == false)
        {
            if(transform.localScale.x<0)
            {
                if (SceneEnd[sceneNum - 1].x - SceneStart[sceneNum - 1].x > 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/right", typeof(Sprite)) as Sprite;
                }
                else if(SceneEnd[sceneNum - 1].x - SceneStart[sceneNum - 1].x < 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/left", typeof(Sprite)) as Sprite;
                }
                else if(SceneEnd[sceneNum - 1].y - SceneStart[sceneNum - 1].y> 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/up", typeof(Sprite)) as Sprite;
                }
                else if (SceneEnd[sceneNum - 1].y - SceneStart[sceneNum - 1].y < 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/down", typeof(Sprite)) as Sprite;
                }
            }
            else
            {
                if (SceneEnd[sceneNum - 1].x - SceneStart[sceneNum - 1].x < 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/right", typeof(Sprite)) as Sprite;
                }
                else if (SceneEnd[sceneNum - 1].x - SceneStart[sceneNum - 1].x > 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/left", typeof(Sprite)) as Sprite;
                }
                else if (SceneEnd[sceneNum - 1].y - SceneStart[sceneNum - 1].y < 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/up", typeof(Sprite)) as Sprite;
                }
                else if (SceneEnd[sceneNum - 1].y - SceneStart[sceneNum - 1].y > 0)
                {
                    arrow.overrideSprite = Resources.Load("arrows/down", typeof(Sprite)) as Sprite;
                }
            }
        }
        else
        {
            arrow.overrideSprite = Resources.Load("arrows/otherarrow", typeof(Sprite)) as Sprite;
        }
        
    }
    public void UseMap()//按M键使用地图
    {
        if (MapCanUse == true) {
        if (StarPlatinum.Services.InputService.Instance.Input.PlayerControls.Map.triggered)
        {
                MapActive = !MapActive;
                //arrowbox.transform.localPosition = startPoint;
                if (MapActive == false)
                {
                    MapAudio.clip = MapOpen;
                    MapAudio.Play();
                }
                else if(MapActive==true)
                {
                    MapAudio.clip = MapClose;
                    MapAudio.Play();
                }
            }
            playerPositon = player.localPosition.x;
            float a = (playerPositon - SceneLeft[sceneNum - 1]) / (SceneRight[sceneNum - 1] - SceneLeft[sceneNum - 1]);
            float x = (SceneEnd[sceneNum - 1].x - SceneStart[sceneNum - 1].x)*a+SceneStart[sceneNum].x;
            float y = (SceneEnd[sceneNum - 1].y - SceneStart[sceneNum - 1].y) * a + SceneStart[sceneNum].y;

            arrowbox.transform.localPosition = new Vector3(x, y, 0);

            gameMap.SetActive(MapActive);
            ChangeMap();
      }
    }
    //public void ArrowMove()
    //{

    //        float x = Input.GetAxisRaw("Horizontal");
    //        Vector3 pos = arrowbox.transform.position;
    //    arrowbox.transform.position = new Vector3(Mathf.Clamp(arrowbox.transform.position.x, Mathf.Min(SceneEnd[sceneNum - 1].x, SceneStart[sceneNum - 1].x)+960, Mathf.Max(SceneEnd[sceneNum - 1].x, SceneStart[sceneNum - 1].x)+960), Mathf.Clamp(arrowbox.transform.position.y, Mathf.Min(SceneEnd[sceneNum - 1].y, SceneStart[sceneNum - 1].y)+540, Mathf.Max(SceneEnd[sceneNum - 1].y, SceneStart[sceneNum - 1].y)+540), 0);
    //    Vector3 move = (SceneEnd[sceneNum - 1] - SceneStart[sceneNum - 1]) ;

    //        if (arrowSwitch == false)
    //        { 
    //            arrowbox.transform.Translate(new Vector3(x * move.x * Time.deltaTime, x * move.y * Time.deltaTime, 0.0f));       
            
    //        }
        
    //}
    //public void ResetPositon()
    //{
    //    if (SceneChange == true)
    //    {
    //        arrowbox.transform.localPosition = SceneStart[sceneNum - 1];
    //        SceneChange = false;
    //    }
    //}

    public void changeSenceFloorNum(int Scenenum,int floorNum,bool arrowswitch)//改变场景时要使用，输出场景编号与所在楼层号
    {
        sceneNum = Scenenum;
        //SceneChange = true;
        playerFloorNum = floorNum;
        FloorNum = floorNum;
        arrowSwitch = arrowswitch;
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
