using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _speed = 10f;
    [SerializeField] float _destructionDelay = 5f;
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

    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
