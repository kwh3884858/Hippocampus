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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void SetCameraRotation(bool brotate)
        {
            bDisableRotation = !brotate;
        }
    }
}
