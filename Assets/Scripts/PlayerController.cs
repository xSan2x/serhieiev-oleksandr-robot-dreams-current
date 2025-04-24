using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private float _speed;
    
    [SerializeField]private Transform _itemPickupUI;

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
        /*Collider[] colliders = Physics.OverlapSphere(_transform.position, 0.75f, LayerMask.GetMask("Item"), QueryTriggerInteraction.Collide);
        if (colliders.Length == 0)
        {
            _itemPickupUI.gameObject.SetActive(false);
            Inventory._instance._currentItem = null;
        }
        else
        {
            Debug.Log("Item detected");
            _itemPickupUI.gameObject.SetActive(true);
            _itemPickupUI.transform.GetChild(0).GetComponent<TMP_Text>().text = colliders[0].GetComponent<Item>().GetItemName();
            Inventory._instance._currentItem = colliders[0].gameObject;
        }*/
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
