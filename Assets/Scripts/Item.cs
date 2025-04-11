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
    [SerializeField] private GameObject _itemPrefab;

    public void PickupItem()
    {
        Debug.Log("Picked up item: " + _itemName);
        Inventory._instance.GetItem(gameObject);
    }

    public void DropItem(Vector3 dropPosition)
    {
        Debug.Log("Dropped item: " + _itemName);
        GameObject droppedItem = Instantiate(_itemPrefab, dropPosition+transform.up, Quaternion.identity);
        droppedItem.GetComponent<Rigidbody>().AddForce(transform.up * 1.5f, ForceMode.Impulse);
        droppedItem.GetComponent<Item>().SetParams(_itemName, _itemDescription, _itemID, _itemIcon, _itemQuantity,_itemPrefab);
    }

    private void SetParams(string itemName, string itemDescription, int itemID, Sprite itemIcon, int itemQuantity, GameObject itemPrefab)
    {
        _itemName = itemName;
        _itemDescription = itemDescription;
        _itemID = itemID;
        _itemIcon = itemIcon;
        _itemQuantity = itemQuantity;
        _itemPrefab = itemPrefab;
    }

    public int GetItemQuantity()
    {
        return _itemQuantity;
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
