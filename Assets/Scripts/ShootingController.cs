using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private Transform _shootingPointTransform;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _splashPrefab;
    [SerializeField] private Transform _aimingPoint;

    [SerializeField] private Transform _itemPickupUI;

    private GameObject _currentItem;


    private readonly Vector3[] _points = new Vector3[2];

    private Coroutine _coroutine;
    private float _rayLifetime;

    private void OnEnable()
    {
        InputController.OnPrimaryInput += FireHandler;
        InputController.OnSecondaryInput += ExplosionHandler;
        InputController.OnInteractInput += InteractHandler;
    }

    private void InteractHandler()
    {
        if(_itemPickupUI.gameObject.activeSelf)
        {
            if(_currentItem != null && _currentItem.TryGetComponent<Item>(out Item item))
            {
                if (item != null)
                {
                    //Pickup the item
                    item.PickupItem();
                    _currentItem.SetActive(false);
                    _currentItem = null;
                }
            }
        }
    }

    private void ExplosionHandler(bool obj)
    {
        if (!UIController._instance._isPaused)
        {
            Debug.DrawRay(_shootingPointTransform.position, _aimingPoint.position - _shootingPointTransform.position, UnityEngine.Color.green, 1f);
            Physics.Raycast(_shootingPointTransform.position, _aimingPoint.position - _shootingPointTransform.position, out RaycastHit hitFromWeapon, 100);
            if (hitFromWeapon.collider != null)
            {
                //Start the explosion
                Instantiate(_explosionPrefab, hitFromWeapon.point, Quaternion.identity);
            }
            else
            {
                Debug.Log("No hit from explosion");
            }
        }
    }

    private void FireHandler()
    {
        if (!UIController._instance._isPaused && !UIController._instance._isInInventory)
        {
            Bullet newBullet = Instantiate(_bulletPrefab, _shootingPointTransform.position, Quaternion.identity).GetComponent<Bullet>();
            newBullet.SetDirectionPoint(_aimingPoint.position);
            Instantiate(_splashPrefab, _shootingPointTransform.position, Quaternion.identity);
        }
        
    }

    private void FixedUpdate()
    {
        //Move the aiming point to the hit point of camera center
        Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, 100);
        if (hit.collider != null) 
        {
            _aimingPoint.position = hit.point;
        } else
        {
            _aimingPoint.position = _cameraTransform.forward * 100;
        }
        //Raycast to items
        Physics.Raycast(_cameraTransform.position, _aimingPoint.position - _cameraTransform.position, out RaycastHit hitFromCamera, 5);
        if (hitFromCamera.collider != null)
        {
            if(hitFromCamera.collider.transform.TryGetComponent<Item>(out Item item))
            {
                _itemPickupUI.gameObject.SetActive(true);
                _currentItem = hitFromCamera.collider.gameObject;
            }
            else
            {
                _itemPickupUI.gameObject.SetActive(false);
            }
        }
        else
        {
            _itemPickupUI.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        InputController.OnPrimaryInput -= FireHandler;
        InputController.OnSecondaryInput -= ExplosionHandler;
    }

    

}

