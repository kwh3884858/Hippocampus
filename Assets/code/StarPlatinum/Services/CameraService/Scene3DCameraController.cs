using UnityEngine;

namespace Assets.code.StarPlatinum.Services.CameraService
{
    public class Scene3DCameraController : MonoBehaviour
    {
        [Header("basic setting")]
        public float m_xMargin = 1f;
        public float m_yMargin = 1f;
        public float m_zMargin = 1f;
        public float m_xSmooth = 3f;
        public float m_ySmooth = 3f;
        public float m_zSmooth = 3f;
        public Vector2 m_maxXAndY = new Vector2(1000, 1000);
        public Vector2 m_minXAndY = new Vector2(-1000, -1000);
        public float m_yOffset = 0;
        public float m_zOffset = 0;

        [SerializeField]
        private Transform m_target;

        [Header("side adjust setting")]
        public float m_sideAdjustDistance = 5f;
        public float m_sideAdjustAngle = 10f;
        public float m_rotateSmooth = 3f;


        public void Refresh()
        {
            //m_xMargin = xMargin;
            //m_yMargin = yMargin;

            //m_xSmooth = xSmooth;
            //m_ySmooth = ySmooth;

            //m_maxXAndY = new Vector2 (maxX, maxY);
            //m_minXAndY = new Vector2 (minX, minY);

            //m_yOffset = yOffset;

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


        void Start()
        {
            Refresh();


        }

        //public  void SetMainCamera(GameObject cameraGameobject)
        //  {
        //      transform.SetParent(cameraGameobject.transform);
        //  }

        public void ChangeTarget(Transform nextTarget)
        {
            m_target = nextTarget;
        }

        public void BackToPlayer()
        {
            m_target = GameObject.Find("Hero").transform;
        }

        bool CheckXMargin()
        {
            return Mathf.Abs(transform.position.x - m_target.position.x) > m_xMargin;
            //return true;
        }

        bool CheckYMargin()
        {
            return Mathf.Abs(transform.position.y - m_target.position.y) > m_yMargin;

        }

        private bool CheckZMargin()
        {
            return Mathf.Abs(transform.position.z - m_target.position.z) > m_zMargin;
        }

        void FixedUpdate()
        {
            if (m_target == null) return;
            //Debug.Log (player.transform.position);
            TrackPlayer();

        }

        int SideDoorAdjust()
        {
            Ray leftRay = new Ray(m_target.position+Vector3.up, Vector3.left);
            Ray rightRay = new Ray(m_target.position+ Vector3.up, Vector3.right);
            bool bLeftHit = Physics.Raycast(leftRay, out RaycastHit hitInfo1, m_sideAdjustDistance);
            bool bRightHit = Physics.Raycast(rightRay, out RaycastHit hitInfo2, m_sideAdjustDistance);
            Debug.DrawLine(m_target.position + Vector3.up, m_target.position + Vector3.right * m_sideAdjustDistance, Color.red);
            Debug.DrawLine(m_target.position + Vector3.up, m_target.position + Vector3.left * m_sideAdjustDistance, Color.red);
            if (bLeftHit)
            {
                //Debug.Log("left  " + hitInfo1.collider.name);
                return hitInfo1.collider.tag == "door" ? -1 : 0;
            }
            if (bRightHit)
            {
                //Debug.Log(" right  " + hitInfo2.collider.gameObject.name);
                return hitInfo2.collider.tag == "door" ? 1 : 0;
            }

            return 0;
        }

        private Vector3 RotateAroundTarget(Vector3 center, Vector3 axis, float angle)
        {

            Quaternion rot = Quaternion.AngleAxis(angle, axis);
            Vector3 rot_Dir = rot * new Vector3(0, m_yOffset, m_zOffset);
            rot_Dir = center + rot_Dir;

            var myrot = transform.rotation;
            Quaternion fq = rot * Quaternion.identity;
            transform.rotation = Quaternion.Slerp(myrot, fq, m_rotateSmooth * Time.deltaTime);
            return rot_Dir;
        }

        void TrackPlayer()
        {
            int sideAdjust = SideDoorAdjust();
            Vector3 pos_target;
            float pos_targetX = transform.position.x;
            float pos_targetY = transform.position.y;
            float pos_targetZ = transform.position.z;

            if (sideAdjust != 0)
            {
                if (sideAdjust == 1)
                {
                    pos_target = RotateAroundTarget(m_target.position, Vector3.up, m_sideAdjustAngle);
                }
                else
                {
                    pos_target = RotateAroundTarget(m_target.position, Vector3.up, -m_sideAdjustAngle);
                }
            }
            else
            {
                RotateAroundTarget(m_target.position, Vector3.up, 0);
                pos_target = m_target.position + new Vector3(0, m_yOffset, m_zOffset);

            }

            if (CheckXMargin())
            {
                pos_targetX = Mathf.Lerp(transform.position.x, pos_target.x, Time.deltaTime * m_xSmooth);
            }

            if (CheckYMargin())
            {
                pos_targetY = Mathf.Lerp(transform.position.y, pos_target.y, Time.deltaTime * m_ySmooth);
            }
            if (CheckZMargin())
            {
                pos_targetZ = Mathf.Lerp(transform.position.z, pos_target.z, Time.deltaTime * m_zSmooth);
            }

            pos_targetX = Mathf.Clamp(pos_targetX, m_minXAndY.x, m_maxXAndY.x);
            pos_targetY = Mathf.Clamp(pos_targetY, m_minXAndY.y, m_maxXAndY.y);
            pos_targetZ = Mathf.Clamp(pos_targetZ, m_minXAndY.y, m_maxXAndY.y);

            transform.position = new Vector3(pos_targetX, pos_targetY, pos_targetZ);
        }



        internal void SetTarget(GameObject target)
        {
            if (target == null) return;
            if (m_target != null)
            {
                Debug.LogError("Camera Controller is not null, you will replace the original target");
            }
            m_target = target.transform;
        }
    }
}