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

    private readonly Vector3[] _points = new Vector3[2];

    private Coroutine _coroutine;
    private float _rayLifetime;

    private void OnEnable()
    {
        InputController.OnPrimaryInput += FireHandler;
        InputController.OnSecondaryInput += ExplosionHandler;
    }

    private void ExplosionHandler(bool obj)
    {
        Physics.Raycast(_cameraTransform.position, _cameraTransform.forward * 1, out RaycastHit hit, 100);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
            Debug.Log(hit.point);
            Debug.DrawRay(_shootingPointTransform.position, hit.point - _shootingPointTransform.position, UnityEngine.Color.green, 1f);
            Physics.Raycast(_shootingPointTransform.position, hit.point - _shootingPointTransform.position, out RaycastHit hitFromWeapon, 100);
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
        else
        {
            Debug.Log("No hit");
        }
    }

    private void FireHandler()
    {
        //Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * 100, UnityEngine.Color.red, 1f);
        //Physics.Raycast(_cameraTransform.position, _cameraTransform.forward*1, out RaycastHit hit, 100);
        //if (hit.collider != null)
        //{
        //    Debug.Log(hit.collider.name);
        //    Debug.Log(hit.point);
        //    Debug.DrawRay(_shootingPointTransform.position, hit.point - _shootingPointTransform.position, UnityEngine.Color.green, 1f);
        //    Physics.Raycast(_shootingPointTransform.position, hit.point - _shootingPointTransform.position, out RaycastHit hitFromWeapon, 100);
        //    if(hitFromWeapon.collider != null)
        //    {
        //        Debug.Log(hitFromWeapon.collider.name);
        //        Debug.Log(hitFromWeapon.point);
        //    }
        //    else
        //    {
        //        Debug.Log("No hit from weapon");
        //    }
        //}
        //else
        //{
        //    Debug.Log("No hit");
        //}
        //Debug.Break();
    }

    private void OnDisable()
    {
        InputController.OnPrimaryInput -= FireHandler;
        InputController.OnSecondaryInput -= ExplosionHandler;
    }
}

