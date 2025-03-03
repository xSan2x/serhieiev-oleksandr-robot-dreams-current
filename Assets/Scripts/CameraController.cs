using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _pitchAnchor;
    [SerializeField] private Transform _yawAnchor;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _sensitivity;

    private float _pitch = 20f;
    private float _yaw = 0f;

    private Vector2 _lookInput;

    private void Start()
    {
        InputController.OnLookInput += LookHandler;
    }

    private void Update()
    {

        _yaw += _lookInput.x * _sensitivity * Time.deltaTime;
        _playerTransform.rotation = Quaternion.Euler(0f, _yaw, 0f);
    }
    private void LateUpdate()
    {
        _pitch -= _lookInput.y * _sensitivity * Time.deltaTime;

        //_pitchAnchor.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        _yawAnchor.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    private void LookHandler(Vector2 lookInput)
    {
        _lookInput = lookInput;
    }
}
