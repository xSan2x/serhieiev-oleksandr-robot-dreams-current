using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    [SerializeField] float _destructionDelay = 0.5f;

    bool _isHeadshot = false;
    bool _isHit = false;

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
        
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Ricochet the bullet
        Vector3 copyVelocity = _rigidbody.velocity;
        _rigidbody.velocity = Vector3.zero;
        transform.forward = Vector3.Reflect(transform.forward, collision.contacts[0].normal);

        _rigidbody.velocity = copyVelocity / 2;
        if (collision.transform.tag == "Body")
        {
            HitController(false, collision);
            _isHit = true;
        }
        else if (collision.transform.tag == "Head")
        {
            HitController(true, collision);
            _isHit = true;
            _isHeadshot = true;
        } 
    }

    private void HitController(bool HS, Collision collision)
    {
        Dummy dummy = collision.transform.GetComponentInParent<Dummy>();
        if (HS)
        {
            dummy.PlayHeadshotSound();
            dummy.TakeDamage(20);
        } else
        {
            dummy.TakeDamage(10);
        }
    }


    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIController._instance.UpdateStats(_isHit, _isHeadshot);
        Destroy(gameObject);
    }
}
