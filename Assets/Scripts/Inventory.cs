using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    List<GameObject> items = new List<GameObject>();
    List<int> itemsQuantity = new List<int>();
    int gold;

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryContentUI;
    [SerializeField] private GameObject itemPrefabUI;
    [SerializeField] private Text goldText;
    [SerializeField] private Text itemDescriptionText;

    [SerializeField] private PlayerController _playerController;

    //Singleton
    public static Inventory _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GetItem(GameObject item)
    {
        if(item.TryGetComponent<Item>(out Item itemComponent))
        {
            if(items.Contains(item))
            {
                itemsQuantity[items.IndexOf(item)] += itemComponent.GetItemQuantity();
            }
            else
            {
                items.Add(item);
                itemsQuantity.Add(itemComponent.GetItemQuantity());
                RedrawUI();
            }
        }
    }

    private void RedrawUI()
    {
        foreach (Transform child in inventoryContentUI.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (GameObject item in items)
        {
            GameObject itemUI = Instantiate(itemPrefabUI, inventoryContentUI.transform);
            itemUI.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<Item>().GetItemIcon();
            itemUI.transform.GetChild(1).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemName();
            itemUI.transform.GetChild(2).GetComponent<TMP_Text>().text = itemsQuantity[items.IndexOf(item)].ToString();
            Button buttonO = itemUI.transform.GetChild(3).GetComponent<Button>();
            buttonO.onClick.AddListener(() => DropItem(item));

            itemUI.transform.SetParent(inventoryContentUI.transform, false);
        }
    }

    private void DropItem(GameObject item)
    {
        item.GetComponent<Item>().DropItem(_playerController.GetPlayerPosition(1));
        itemsQuantity.RemoveAt(items.IndexOf(item));
        items.Remove(item);
        
        RedrawUI();
    }
}
