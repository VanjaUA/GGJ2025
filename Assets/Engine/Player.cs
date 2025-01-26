using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Engine
{
    public class Player : MonoBehaviour,IDamageable
    {
        private const int MaxHealth = 100;
        private const int ClipCapacity = 12;

        [SerializeField] private float mouseSensitivity = 1f;
        [SerializeField] private float moveSpeed;

        [SerializeField] private int damage;
        [SerializeField] private int entityLayer;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private LayerMask shootLayer;

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

            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            PlayerHUDManager.Instance.UpdateBulletsText(allBullets, bulletsInClip);
            PlayerHUDManager.Instance.UpdateHealthBar(_currentHealth, MaxHealth);
            PlayerHUDManager.Instance.UpdateMoneyText(money);

            InputManager.Instance.OnLeftMouseButtonPressed += OnLeftMouseButtonPressed;
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

            if (Input.GetKeyDown(KeyCode.Z))
            {
                var playerData = new GameData { Money = money, HealthPoints = _currentHealth };
                JsonSaveService.SaveData(playerData,"data");
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                var loadedData = JsonSaveService.LoadData<GameData>("data");
                money = loadedData.Money;
                PlayerHUDManager.Instance.UpdateMoneyText(money);
                _currentHealth = loadedData.HealthPoints;
                PlayerHUDManager.Instance.UpdateHealthBar(_currentHealth,MaxHealth);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                TryToReload();
            }
        }


        private void HandleCharacterLook()
        {
            float lookX = Input.GetAxisRaw("Mouse X");
            transform.Rotate(new Vector3(0f, lookX * mouseSensitivity, 0f), Space.Self);
        }

        private void HandleCharacterMovement()
        {
            Vector2 moveDirection = InputManager.Instance.GetMoveDirection();
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

        private void OnLeftMouseButtonPressed()
        {
            if (TryToShoot() == false)
            {
                return;
            }

            float spread = 0.005f;
            if (InputManager.Instance.IsMoving())
            {
                spread = 0.02f;
            }
            Vector2 randomOffset = Random.insideUnitCircle * spread;
            Vector3 targetPoint = new Vector3(0.5f + randomOffset.x, 0.5f + randomOffset.y, 0f);
            Ray ray = mainCamera.ViewportPointToRay(targetPoint);
            //ray.origin = mainCamera.transform.position;
            ray.origin = shootPoint.position;

            float minRadius = 0.1f;
            Collider[] hits = Physics.OverlapSphere(transform.position, minRadius);
            foreach (var collider in hits)
            {
                if (collider.TryGetComponent<EnemyHitBox>(out EnemyHitBox target))
                {
                    target.Damage(damage);
                    return;
                }
            }

            float shootDistance = 100f;
            if (Physics.Raycast(ray, out RaycastHit hit, shootDistance, shootLayer, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.TryGetComponent<IDamageable>(out IDamageable target))
                {
                    target.Damage(damage); 
                }
                else
                {
                    if (hit.collider.gameObject.layer != entityLayer)
                    {
                        GameObject bulletImpactObject = Instantiate(bulletImpact, hit.point + hit.normal * 0.002f,
                                Quaternion.LookRotation(hit.normal, Vector3.up));
                        Destroy(bulletImpactObject, 5f);
                    }
                }
            }
        }

        private bool TryToShoot() 
        {
            bool canShoot = false;
            if (bulletsInClip > 0)
            {
                bulletsInClip--;
                canShoot = true;
            }
            else
            {
                TryToReload();
            }
            PlayerHUDManager.Instance.UpdateBulletsText(allBullets, bulletsInClip);
            return canShoot;
        }

        private void TryToReload() 
        {
            if (allBullets <= 0) 
            {
                return;
            }
            if (bulletsInClip > 0)
            {
                allBullets += bulletsInClip;
                bulletsInClip = 0;
            }
            if (allBullets <= ClipCapacity)
            {
                bulletsInClip = allBullets;
                allBullets = 0;
                PlayerHUDManager.Instance.UpdateBulletsText(allBullets, bulletsInClip);
                return;
            }
            bulletsInClip = ClipCapacity;
            allBullets -= bulletsInClip;
            PlayerHUDManager.Instance.UpdateBulletsText(allBullets, bulletsInClip);
        }


        private void TakeDamage() 
        {
            Damage(5);
            money -= 98;

            PlayerHUDManager.Instance.UpdateMoneyText(money);
        }

        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth;
            }
            PlayerHUDManager.Instance.UpdateHealthBar(_currentHealth, MaxHealth);
        }

        public void Damage(int damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
            PlayerHUDManager.Instance.UpdateHealthBar(_currentHealth, MaxHealth);
        }

        public void Die()
        {
            Debug.Log("Player died");
        }
    }
}
