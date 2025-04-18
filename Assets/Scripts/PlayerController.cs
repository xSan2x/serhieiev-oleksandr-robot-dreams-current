using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _speed;

    private Vector3 _localDirection;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        InputController.OnMoveInput += MoveHandler;
    }

    private void MoveHandler(Vector2 input)
    {
        _localDirection = new Vector3(input.x, 0, input.y);
    }

    private void FixedUpdate()
    {
        Vector3 forward = _transform.forward;
        Vector3 right = _transform.right;

        Vector3 direction = forward * _localDirection.z + right * _localDirection.x;

        _characterController.SimpleMove(direction * _speed);
    }

    public Vector3 GetPlayerPosition(int variants) //0 - pure pos, 1- pos + forward
    {
        switch(variants)
        {
            case 0:
                return _transform.position;
            case 1:
                return _transform.position + _transform.forward * 2;
            default:
                Debug.LogError("Invalid variant for GetPlayerPosition");
                return _transform.position;
        }
        
    }
}
