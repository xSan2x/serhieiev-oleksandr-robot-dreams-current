using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    [SerializeField] float _destructionDelay = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        //Start coroutine to destroy the bullet after 5 seconds
        StartCoroutine(DestroyBullet(_destructionDelay));
    }

    public void SetDirectionPoint(Vector3 directionPoint)
    {
        //Set the direction of the bullet
        transform.LookAt(directionPoint);
    }

    private void Update()
    {
        //Move the bullet forward
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Ricochet the bullet
        transform.forward = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        _speed /= 2;
        Debug.Log("Bullet collided with " + collision.gameObject.name);
    }

    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
