using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    Inventory _inventory;

    private void Start()
    {
        _inventory = Inventory._instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other == _inventory._pickupCollider)
        {
            _inventory._merchantHintUI.gameObject.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _inventory._pickupCollider)
        {
            _inventory._merchantHintUI.gameObject.SetActive(false);
        }
    }
}
