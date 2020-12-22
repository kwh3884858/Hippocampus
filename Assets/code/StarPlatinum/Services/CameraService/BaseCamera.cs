using StarPlatinum.Services;
using UnityEngine;



namespace Assets.code.StarPlatinum.Services.CameraService
{
    [System.Serializable]
    public enum VMCameraType
    {
        None,
        FirstPerson_MG,
        ThirdPerson_MG,
        VMCamera_FirstPerson_Fixed,
        VMCamera_ThirdPerson_Fixed
    }
    public class BaseCamera : MonoBehaviour
    {
        public VMCameraType m_VMCameraType = VMCameraType.None;
        public bool bDisableRotation = false;
        public float m_lookRotateSmooth = 3.0f;
        protected bool isLookAt = false;
        protected Quaternion m_desRotation;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {
            if (isLookAt)
            {
                var myrot = transform.rotation;
                //Debug.Log(this.name + " camera is set to look at in Qua: " + Quaternion.Angle(myrot, m_desRotation));
                //Vector2 lookDelta = InputService.Instance.Input.PlayerControls.Look.ReadValue<Vector2>();
                //Debug.Log(lookDelta);
                if (Quaternion.Angle(myrot, m_desRotation) <5)
                {
                    isLookAt = false;
                    SetCameraRotation(true);
                    return;
                }
                transform.rotation = Quaternion.Slerp(myrot, m_desRotation, m_lookRotateSmooth * Time.deltaTime);
            }
        }

        public virtual void SetLookRotation(float x,float y, float z)
        {
            SetCameraRotation(false);
            Vector3 elurVector3 = new Vector3(x,y,z);
            m_desRotation = elurVector3 == Vector3.zero ? Quaternion.identity : Quaternion.Euler(elurVector3);
            isLookAt = true;
        }
        public virtual void SetCameraRotation(bool brotate)
        {
            bDisableRotation = !brotate;
        }
    }
}
