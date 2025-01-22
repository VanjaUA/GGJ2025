using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float mouseSensitivity = 1f;
        [SerializeField] private float moveSpeed;

        private int allBullets = 98, bulletsInClip = 12;
        [SerializeField] private int maxhealth = 100;
        [SerializeField] private int currentHealth = 100;
        private int money = 9998;

        private float _characterVelocityY;

        private CharacterController _characterController;

        private void Awake()
        {
            UIManager.Instance.UpdateBulletsText(allBullets, bulletsInClip);

            UIManager.Instance.UpdateHealthBar(currentHealth, maxhealth);

            UIManager.Instance.UpdateMoneyText(money);


            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleCharacterLook();
            HandleCharacterMovement();

            CursorHandle();

            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage();
            }
        }

        private void HandleCharacterLook()
        {
            float lookX = Input.GetAxisRaw("Mouse X");
            transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);
        }

        private void HandleCharacterMovement()
        {
            Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            Vector3 characterVelocity = (transform.right * moveDirection.x * moveSpeed + transform.forward * moveDirection.y * moveSpeed);

            if (_characterController.isGrounded)
            {
                _characterVelocityY = 0f;
                //if (Input.GetKeyDown(KeyCode.Space))
                //{
                //    float jumpSpeed = 30f;
                //    _characterVelocityY = jumpSpeed;
                //}
            }
            float gravityDownForce = -110f;
            _characterVelocityY += gravityDownForce * Time.deltaTime;
            characterVelocity.y = _characterVelocityY;
            _characterController.Move(characterVelocity * Time.deltaTime);
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


        private void TakeDamage() 
        {
            currentHealth -= 5;
            bulletsInClip -= 2;
            money -= 98;

            UIManager.Instance.UpdateBulletsText(allBullets,bulletsInClip);

            UIManager.Instance.UpdateHealthBar(currentHealth, maxhealth);

            UIManager.Instance.UpdateMoneyText(money);
        }
    }
}
