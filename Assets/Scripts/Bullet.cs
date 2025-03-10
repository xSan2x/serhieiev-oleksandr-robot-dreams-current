using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    [SerializeField] float _destructionDelay = 0.5f;

    Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        //Start coroutine to destroy the bullet after 5 seconds
        StartCoroutine(DestroyBullet(_destructionDelay));
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * _speed, ForceMode.Force);
    }

    public void SetDirectionPoint(Vector3 directionPoint)
    {
        //Set the direction of the bullet
        transform.LookAt(directionPoint);
    }

    private void FixedUpdate()
    {
        //Move the bullet forward
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Ricochet the bullet
        Debug.Log("Bullet velocity before = "+_rigidbody.velocity);
        Vector3 copyVelocity = _rigidbody.velocity;
        _rigidbody.velocity = Vector3.zero;
        transform.forward = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        
        _rigidbody.velocity = copyVelocity / 2;
        Debug.Log("Bullet velocity after = " + _rigidbody.velocity);
    }

    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
