using UnityEngine;


namespace Engine
{
    public class CameraShake : MonoBehaviour
    {
        public float shakeAmplitude = 0.2f;
        public float shakeFrequency = 8.0f;
        public float smoothTransition = 10.0f;

        private Vector3 originalPosition;
        private float shakeTimer = 0.0f;
        private float currentShakeAmplitude = 0.0f;

        void Start()
        {
            originalPosition = transform.localPosition;
        }

        void Update()
        {
            ShakeHandle();
        }

        private void ShakeHandle() 
        {
            if (InputManager.Instance.IsMoving() == false)
            {
                currentShakeAmplitude = Mathf.Lerp(currentShakeAmplitude, 0, Time.deltaTime * smoothTransition);
                transform.localPosition = Vector3.Slerp(transform.localPosition, originalPosition, Time.deltaTime * smoothTransition);
                return;
            }

            currentShakeAmplitude = Mathf.Lerp(currentShakeAmplitude, shakeAmplitude, Time.deltaTime * smoothTransition);
            shakeTimer += Time.deltaTime * shakeFrequency;
            float offsetX = Mathf.Sin(shakeTimer) * currentShakeAmplitude;
            transform.localPosition = originalPosition + new Vector3(offsetX, 0, 0);
        }

        private bool IsMoving()
        {
            return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
        }
    }

}
