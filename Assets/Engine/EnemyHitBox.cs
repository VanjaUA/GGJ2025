using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Engine 
{
    public class EnemyHitBox : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform enemyTransform;
        [SerializeField] private int maxHealth;
        private int _currentHealth;

        private void Awake()
        {
            _currentHealth = maxHealth;
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
            Destroy(enemyTransform.gameObject);
        }
    }
}
