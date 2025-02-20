using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform _bottomTransform;
    [SerializeField]
    private Transform _topTransform;
    [SerializeField]
    private Transform _cameraTransform;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _sensitivity = 10f;


    // Update is called once per frame
    void Update()
    {
        // Move the player
        MovePlayer();
        // Rotate the player and camera
        RotatePlayer();
    }

    private void LateUpdate()
    {
        // Disable X rotation if it is more than 20 and lower than -20 and Disable Z rotation
        if (transform.localRotation.eulerAngles.x < 20 || transform.localRotation.eulerAngles.x > 340)
        {
            transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y, 0);
        }
        else if (transform.localRotation.eulerAngles.x > 20 && transform.localRotation.eulerAngles.x < 90)
        {
            transform.localRotation = Quaternion.Euler(19.9f, transform.localRotation.eulerAngles.y, 0);
        }
        else if (transform.localRotation.eulerAngles.x < 340)
        {
            transform.localRotation = Quaternion.Euler(-19.9f, transform.localRotation.eulerAngles.y, 0);
        }
        transform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    //Move the player 
    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movementForward = Vector3.Normalize(_bottomTransform.forward) * vertical;
        transform.Translate(movementForward * _speed * Time.deltaTime, Space.World);
        Vector3 movementSides = new Vector3(0, horizontal, 0);
        transform.Rotate(movementSides * _speed * 25f * Time.deltaTime);
    }

    //Rotate the player and camera
    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 rotation = new Vector3(0, mouseX, 0) * _sensitivity;
        _topTransform.Rotate(rotation);
    }
}
