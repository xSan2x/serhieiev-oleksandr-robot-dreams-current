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

    //Move the player 
    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movementForward = Vector3.Normalize(_bottomTransform.forward) * vertical;
        transform.Translate(movementForward * _speed * Time.deltaTime, Space.World);
        Vector3 movementSides = new Vector3(0, horizontal, 0);
        transform.Rotate(movementSides * _speed * 10f * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.eulerAngles.y, 0);
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
