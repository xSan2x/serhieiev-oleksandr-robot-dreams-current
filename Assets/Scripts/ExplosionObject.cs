using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Startnig the explosion coroutine
        StartCoroutine(Explosion(transform.position, 5, 2500, 1));
    }

    //explosion coroutine
    private IEnumerator Explosion(Vector3 position, float radius, float force, float duration)
    {
        //Get all the colliders in the radius
        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (var collider in colliders)
        {
            //Get the rigidbody of the collider
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //Add the explosion force
                rb.AddExplosionForce(force, position, radius);
            }
        }
        //Wait for the duration
        yield return new WaitForSeconds(duration);
        //Destroy the gameobject
        Destroy(gameObject);
    }
}
