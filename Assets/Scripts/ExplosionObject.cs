using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    [SerializeField] float _duration = 1f; 
    [SerializeField] float _currentState = 0f; 
    [SerializeField] float _radius = 5f;
    [SerializeField] float _force = 2500f;
    
    private void FixedUpdate()
    {
        //Increase the current state
        _currentState += Time.fixedDeltaTime;
        if (_currentState > _duration)
        {
            //Destroy the gameobject
            Destroy(gameObject);
        }
        else
        {
            float scaleKf = _currentState / _duration;
            float scale = scaleKf * _radius;
            transform.localScale = new Vector3(scale, scale, scale);
            //Get all the colliders in the radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, scale);
            foreach (var collider in colliders)
            {
                //Get the rigidbody of the collider
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    //Add the explosion force by the scale
                    rb.AddExplosionForce(_force*(1-scaleKf), transform.position, scale);
                }
            }
        }

    }
    
}
