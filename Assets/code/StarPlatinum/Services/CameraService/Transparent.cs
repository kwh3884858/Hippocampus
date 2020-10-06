using System.Collections.Generic;
using UnityEngine;

namespace Assets.code.StarPlatinum.Services.CameraService
{
    public class Transparent : MonoBehaviour
    {

        //得到主人公
        public GameObject hero_renderer;

        public float transparent_distance = 12;
        //上次碰撞到的物体
        public List<GameObject> lastColliderObject = new List<GameObject>();
        //需要复原的物体
        public List<GameObject> recoverColliderObject = new List<GameObject>();

        //本次碰撞到的物体
        public List<GameObject> colliderObject = new List<GameObject>();

        void Start()
        {
            hero_renderer = GameObject.FindGameObjectWithTag("Player");

        }
        void Update()
        {
            //为了调式时看的清楚画的线

            Vector3 dir = (transform.position - hero_renderer.transform.position).normalized;
            RaycastHit[] hits = Physics.RaycastAll(hero_renderer.transform.position, dir, transparent_distance);
            Debug.DrawLine(hero_renderer.transform.position, hero_renderer.transform.position+dir* transparent_distance, Color.red);

            //将这次 colliderObject 中所有的值添加进 lastColliderObject
            foreach (var t in colliderObject)
                lastColliderObject.Add(t);

            colliderObject.Clear();//清空本次碰撞到的所有物体

            foreach (var t in hits)
            {
                if (t.collider.tag != "MainCamera")//cull out camera
                {
                    //Debug.Log(t.collider.gameObject.name);
                    colliderObject.Add(t.collider.gameObject);
                    SetMaterialsColor(t.collider.gameObject, 0.5f);//置当前物体材质透明度
                }
            }

            //上次与本次对比，本次还存在的物体则移除 lastColliderObject
            foreach (var o in lastColliderObject)
            {
                foreach (var o1 in colliderObject)
                {
                    if (o1!=null)
                    {
                        if (o==o1)
                        {
                            recoverColliderObject.Add(o);
                            break;
                        }
                    }
                }
            }

            foreach (var o in recoverColliderObject)
            {
                lastColliderObject.Remove(o);
                lastColliderObject.Remove(null);
            }
            recoverColliderObject.Clear();

            // return to normal alpha

            foreach (var o in lastColliderObject)
            {
                SetMaterialsColor(o, 1);
            }

        }



        private void SetMaterialsColor(GameObject objRenderer, float alpha)
        {
            if (objRenderer!=null)
            {
                Renderer obj_renderer = objRenderer.GetComponent<Renderer>();
                if (obj_renderer != null)
                {
                    int materialsNumber = obj_renderer.sharedMaterials.Length;

                    for (int i = 0; i < materialsNumber; i++)
                    {
                        Color obj_color = obj_renderer.materials[i].GetColor("_BaseColor");
                        obj_color.a = alpha;
                        obj_renderer.material.SetColor("_BaseColor", obj_color);
                    }
                }
            }
        }
    }
}