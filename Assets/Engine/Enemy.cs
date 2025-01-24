using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Engine
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private GameObject followObject;
        [SerializeField] private float speed;

        private Rigidbody _rigidbody;

        private void Awake()
        {
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

    }
}
