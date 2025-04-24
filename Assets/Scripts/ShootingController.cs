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



    private void OnEnable()
    {
        InputController.OnPrimaryInput += FireHandler;
        InputController.OnSecondaryInput += ExplosionHandler;
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
        
    }

    private void OnDisable()
    {
        InputController.OnPrimaryInput -= FireHandler;
        InputController.OnSecondaryInput -= ExplosionHandler;
    }

    

}

