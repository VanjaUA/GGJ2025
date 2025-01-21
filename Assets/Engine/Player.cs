using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 1f;
        [SerializeField] private float walkSpeed;

        private Vector2 _mouseInput;
        private Vector3 _moveDirection;
        private Vector3 _movement;

        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            RotationHandle();
            MoveHandle();

            CursorHandle();
        }

        private void RotationHandle()
        {
            _mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * _mouseInput.x);         
        }

        private void MoveHandle()
        {
            _moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

            _movement = ((transform.right * _moveDirection.x) + (transform.forward * _moveDirection.z)).normalized * walkSpeed;

            _characterController.Move(_movement * Time.deltaTime);
        }

        private void CursorHandle()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
                return;
            }
            if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }
        }
    }
}
