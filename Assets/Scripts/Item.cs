using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemDescription;
    [SerializeField] private int _itemID;
    [SerializeField] private Sprite _itemIcon;
    [SerializeField] private int _itemQuantity;
    [SerializeField] private int _itemCost;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] public bool _isMerchantItem = false;

    public void PickupItem()
    {
        Debug.Log("Picked up item: " + _itemName);
        Inventory._instance.GetItem(gameObject);
    }

    public void MerchantHoldsItem()
    {
        _isMerchantItem = true;
    }
    public void DropItem(Vector3 dropPosition)
    {
        Debug.Log("Dropped item: " + _itemName);
        GameObject droppedItem = Instantiate(_itemPrefab, dropPosition+transform.up, Quaternion.identity);
        droppedItem.GetComponent<Rigidbody>().AddForce(transform.up * 1.5f, ForceMode.Impulse);
        droppedItem.GetComponent<Item>().SetParams(_itemName, _itemDescription, _itemID, _itemIcon, _itemQuantity,_itemCost, _itemPrefab);
    }

    public void SetParams(string itemName, string itemDescription, int itemID, Sprite itemIcon, int itemQuantity,int itemCost, GameObject itemPrefab)
    {
        _itemName = itemName;
        _itemDescription = itemDescription;
        _itemID = itemID;
        _itemIcon = itemIcon;
        _itemQuantity = itemQuantity;
        _itemCost = itemCost;
        _itemPrefab = itemPrefab;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other== Inventory._instance._pickupCollider)
        {
            if (!_isMerchantItem)
            {
                Inventory._instance._itemPickupUI.gameObject.SetActive(true);
                Inventory._instance._itemPickupUI.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "PICK UP ITEM: " + _itemName;
                Inventory._instance._currentItem = gameObject;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other == Inventory._instance._pickupCollider)
        {
            if (!_isMerchantItem)
            {
                Inventory._instance._itemPickupUI.gameObject.SetActive(true);
                Inventory._instance._itemPickupUI.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "PICK UP ITEM: " + _itemName;
                Inventory._instance._currentItem = gameObject;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == Inventory._instance._pickupCollider)
        {
            Inventory._instance._itemPickupUI.gameObject.SetActive(false);
            if(Inventory._instance._currentItem == gameObject)
            {
                Inventory._instance._itemPickupUI.gameObject.SetActive(false);
                Inventory._instance._currentItem = null;
            }
        }
    }

    public int GetItemQuantity()
    {
        return _itemQuantity;
    }
    public int GetItemCost()
    {
        return _itemCost;
    }

    public string GetItemName()
    {
        return _itemName;
    }
    public string GetItemDescription()
    {
        return _itemDescription;
    }
    public Sprite GetItemIcon()
    {
        return _itemIcon;
    }
}
