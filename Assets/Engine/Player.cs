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

        [SerializeField] private int damage;
        [SerializeField] private LayerMask damageableLayer;

        private int _currentHealth;
        private int allBullets = 98, bulletsInClip = 12;
        private int money = 9998;

        private float _characterVelocityY;

        private CharacterController _characterController;

        [SerializeField] private GameObject bulletImpact;
        private Camera mainCamera;

        private void Awake()
        {
            _currentHealth = MaxHealth;

            mainCamera = Camera.main;

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
                //Debug.Log("Shoot");

                //Vector3 halfBoxSize = new Vector3(.7f, .75f, 20f);
                //float playerHeightOffset = .8f;
                //Collider[] colliderArray = 
                //    Physics.OverlapBox(transform.position + transform.up * playerHeightOffset + transform.forward * halfBoxSize.z,
                //    halfBoxSize, transform.rotation, damageableLayer);
                //Debug.Log(colliderArray.Length);
                //foreach (Collider collider in colliderArray)
                //{
                //    IDamageable hitTarget;
                //    collider.TryGetComponent<IDamageable>(out hitTarget);
                //    if (collider.TryGetComponent<IDamageable>(out hitTarget))
                //    {
                //        Debug.Log("Hit");
                //        hitTarget.Damage(damage);
                //    }
                //}




                Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                ray.origin = mainCamera.transform.position;

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent<IDamageable>(out IDamageable target))
                    {
                        Debug.Log("Hit   " + hit.collider.gameObject.name);
                    }
                    else
                    {
                        GameObject bulletImpactObject = Instantiate(bulletImpact, hit.point + hit.normal * 0.002f, Quaternion.LookRotation(hit.normal, Vector3.up));
                        Destroy(bulletImpactObject, 15f);
                    }
                }

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
