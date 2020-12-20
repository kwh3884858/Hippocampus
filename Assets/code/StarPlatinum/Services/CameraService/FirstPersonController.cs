using StarPlatinum.Services;
using UnityEngine;

namespace Assets.code.StarPlatinum.Services.CameraService
{
    public class FirstPersonController : BaseCamera
    {
        public enum RotationAxes
        {
            MouseXAndY = 0,
            MouseX = 1,
            MouseY = 2
        }

        public RotationAxes axes = RotationAxes.MouseXAndY;
        public float sensitivityX = 15F;
        public float sensitivityY = 15F;

        public float minimumX = -360F;
        public float maximumX = 360F;

        public float minimumY = -60F;
        public float maximumY = 60F;

        float rotationY = 0F;

        public override void SetCameraRotation(bool brotate)
        {
            base.SetCameraRotation(brotate);
        }

        void Update()
        {
            if (bDisableRotation)
            {
                return;
            }
            if (axes == RotationAxes.MouseXAndY)
            {
                Vector2 lookDelta = InputService.Instance.Input.PlayerControls.Look.ReadValue<Vector2>();
                float rotationX = transform.localEulerAngles.y + lookDelta.x * sensitivityX;

                rotationY += lookDelta.y * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            }
            else if (axes == RotationAxes.MouseX)
            {
                transform.Rotate(0, InputService.Instance.Input.PlayerControls.Look.ReadValue<Vector2>().x * sensitivityX, 0);
            }
            else
            {
                rotationY += InputService.Instance.Input.PlayerControls.Look.ReadValue<Vector2>().y * sensitivityY;
                rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
            }
        }

        void Start()
        {
            // Make the rigid body not change rotation
        }
    }
}
