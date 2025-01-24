using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Engine
{
    public interface IDamageable
    {
        public void Heal(int healAmount);
        public void Damage(int damageAmount);
        public void Die();
    }
}
