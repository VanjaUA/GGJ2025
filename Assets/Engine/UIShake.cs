using UnityEngine;

namespace Engine
{
    public class UIShake : MonoBehaviour
    {
        public float shakeAmplitude = 10f;
        public float shakeFrequency = 5f;
        public float smoothTransition = 5f;

        private RectTransform weaponTransform;
        private Vector3 originalPosition;
        private float shakeTimer = 0f;
        private float currentShakeAmplitude = 0f;

        void Start()
        {
            weaponTransform = GetComponent<RectTransform>();
            originalPosition = weaponTransform.localPosition;
        }

        void Update()
        {
            if (IsMoving() == false)
            {
                currentShakeAmplitude = Mathf.Lerp(currentShakeAmplitude, 0, Time.deltaTime * smoothTransition);
                weaponTransform.localPosition = Vector3.Lerp(weaponTransform.localPosition, originalPosition, Time.deltaTime * smoothTransition);
                return;
            }

            currentShakeAmplitude = Mathf.Lerp(currentShakeAmplitude, shakeAmplitude, Time.deltaTime * smoothTransition);
            shakeTimer += Time.deltaTime * shakeFrequency;
            float offsetX = Mathf.Sin(shakeTimer) * currentShakeAmplitude;
            float offsetY = Mathf.Cos(shakeTimer * 2) * currentShakeAmplitude * 0.5f;
            weaponTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
        }

        private bool IsMoving()
        {
            return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
        }
    }
}
