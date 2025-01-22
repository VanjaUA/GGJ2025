using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Engine
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject followObject;
        [SerializeField] private float speed;


        [SerializeField] private int maxHealth;
        private int _currentHealth;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _currentHealth = maxHealth;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement() 
        {
            //transform.position = Vector3.MoveTowards(transform.position, followObject.transform.position,speed * Time.deltaTime);
            Vector3 newPosition = Vector3.MoveTowards(_rigidbody.position, followObject.transform.position, speed * Time.deltaTime);
            _rigidbody.MovePosition(newPosition);
        }

        public void Heal(int healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > maxHealth)
            {
                _currentHealth = maxHealth;
            }
        }

        public void Damage(int damageAmount)
        {
            _currentHealth -= damageAmount;
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
