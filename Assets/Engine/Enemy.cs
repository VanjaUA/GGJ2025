using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Engine
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject followObject;
        [SerializeField] private float speed;

        [SerializeField] private int damage;
        [SerializeField] private float attackCooldown = 2f;
        private bool isPlayerInRange = false; // Чи знаходиться гравець у зоні атаки
        private float nextAttackTime = 0f;
        private Player player;

        private Rigidbody _rigidbody;
        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            HandleMovement();

            if (nextAttackTime > 0)
            {
                nextAttackTime -= Time.deltaTime;
            }
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Player>(out player))
            {
                isPlayerInRange = true;
                if (nextAttackTime <= 0f)
                {
                    player.Damage(damage);
                    nextAttackTime = attackCooldown;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Player>(out player))
            {
                isPlayerInRange = false;
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (isPlayerInRange == false)
            {
                return;
            }
            if (player == null)
            {
                return;
            }
            if (nextAttackTime <= 0f)
            {
                player.Damage(damage);
                nextAttackTime = attackCooldown;
            }
        }

        private void HandleMovement() 
        {
            _navMeshAgent.SetDestination(followObject.transform.position);

            //Vector3 newPosition = Vector3.MoveTowards(_rigidbody.position, followObject.transform.position, speed * Time.deltaTime);
            //_rigidbody.MovePosition(newPosition);
        }

    }
}
