using System;
using UnityEngine;

namespace Engine
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public event Action OnLeftMouseButtonPressed;
        public event Action OnSpaceButtonPressed;

        private void Awake()
        {
            SingletonInit();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftMouseButtonPressed?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnSpaceButtonPressed?.Invoke();
            }
        }

        private void SingletonInit()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        public bool IsMoving()
        {
            return Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
        }

        public Vector2 GetMoveDirection() 
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        }
    }
}
