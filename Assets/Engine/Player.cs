using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class Player : MonoBehaviour,IDamageable
    {
        private const int MaxHealth = 100;

        [SerializeField] private float mouseSensitivity = 1f;
        [SerializeField] private float moveSpeed;

        private int _currentHealth;
        private int allBullets = 98, bulletsInClip = 12;
        private int money = 9998;

        private float _characterVelocityY;

        private CharacterController _characterController;

        private void Awake()
        {
            _currentHealth = MaxHealth;

            UIManager.Instance.UpdateBulletsText(allBullets, bulletsInClip);
            UIManager.Instance.UpdateHealthBar(_currentHealth, MaxHealth);
            UIManager.Instance.UpdateMoneyText(money);


            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            HandleCharacterLook();
            HandleCharacterMovement();

            CursorHandle();
            HandleShooting();

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

        private void HandleShooting()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Shoot");

                //Vector3 halfBoxSize = new Vector3(.7f, .75f, 20f);
                //float playerHeightOffset = .8f;
                //Collider[] colliderArray = Physics.OverlapBox(transform.position + transform.up * playerHeightOffset + transform.forward * halfBoxSize.z, halfBoxSize, transform.rotation);
                //foreach (Collider collider in colliderArray)
                //{
                //    var shootingTarget = collider.GetComponent<BoxCollider>();
                //    if (shootingTarget != null)
                //    {
                //        //shootingTarget.Damage();
                //    }
                //}
            }
        }


        private void TakeDamage() 
        {
            Damage(5);
            bulletsInClip -= 2;
            money -= 98;

            UIManager.Instance.UpdateBulletsText(allBullets,bulletsInClip);

            UIManager.Instance.UpdateMoneyText(money);
        }

        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth;
            }
            UIManager.Instance.UpdateHealthBar(_currentHealth, MaxHealth);
        }

        public void Damage(int damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
            UIManager.Instance.UpdateHealthBar(_currentHealth, MaxHealth);
        }

        public void Die()
        {
            Debug.Log("Player died");
        }
    }
}
