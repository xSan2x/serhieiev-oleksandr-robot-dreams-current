using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class Inventory : MonoBehaviour
{
    //Player items
    List<GameObject> items = new List<GameObject>();
    List<int> itemsQuantity = new List<int>();

    //Exchange items to buy
    List<GameObject> _exchangeItemsToBuy = new List<GameObject>();
    List<int> _exchangeItemsToBuyQuantity = new List<int>();

    //Exchange items to sell
    List<GameObject> _exchangeItemsToSell = new List<GameObject>();
    List<int> _exchangeItemsToSellQuantity = new List<int>();

    //Merchant items
    List<GameObject> _merchantItems = new List<GameObject>();
    List<int> _mecrchantItemsQuantity = new List<int>();
    int _gold = 0;
    int _merchantGold = 0; //gold of merchant 
    int _goldFromPlayer = 0; //gold from player for exchange
    int _goldFromMerchant = 0; //gold from merchant for exchange
    int _goldForExchange = 0; //gold for exchange

    int _choosenItemIndex = -1;
    int _choosenHolder = -1; //0 - player, 1 - exchange, 2 - merchant
    [SerializeField] private TMP_InputField _exchangeInput;
    int _exchangeCount = 0; //count of items in exchange

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject inventoryContentUI;
    [SerializeField] private GameObject itemPrefabUI;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Sprite[] _spritesForItems;

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text invGoldText;
    [SerializeField] private TMP_Text exGoldText;
    [SerializeField] private TMP_Text merchGoldText;
    [SerializeField] private TMP_Text itemDescriptionText;

    [SerializeField] private GameObject exchangeUI;
    [SerializeField] private GameObject playerContentUI;
    [SerializeField] private GameObject exchangeToBuyContentUI;
    [SerializeField] private GameObject exchangeToSellContentUI;
    [SerializeField] private GameObject merchantContentUI;
    [SerializeField] private GameObject itemPrefabExchangeUI;

    [SerializeField] private Button _allLeftButton;
    [SerializeField] private Button _allRightButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [SerializeField] private PlayerController _playerController;

    [SerializeField] private Transform _spawnPointForItems;

    [SerializeField] public Collider _pickupCollider;
    [SerializeField] public Transform _itemPickupUI;
    [SerializeField] public Transform _merchantHintUI;
    [SerializeField] public Transform _messagingUI;

    public GameObject _currentItem;

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
    private void Start()
    {
        InitMerchant();
        InputController.OnInteractInput += InteractHandler;
    }

    private void InteractHandler()
    {
        if (_currentItem == null)
        {
            if(_merchantHintUI.gameObject.activeSelf)
            {
                _merchantHintUI.gameObject.SetActive(false);
                exchangeUI.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                UIController._instance._isInInventory = true;
                RedrawExchangeUI();
                return;
            }
            return;
        }
        if (_currentItem.TryGetComponent<Item>(out Item item))
        {
            item.PickupItem();
            _itemPickupUI.gameObject.SetActive(false);
            _currentItem.SetActive(false);
            _currentItem = null;
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
    private void InitMerchant()
    {
        GameObject newItem = Instantiate(itemPrefab, _spawnPointForItems.position,Quaternion.identity);
        newItem.GetComponent<Item>().SetParams("Ammo", "Ammunition for shooting. Cost: 2g", 2, _spritesForItems[0], 100, 2, itemPrefab);
        newItem.GetComponent<Item>().MerchantHoldsItem();
        _merchantItems.Add(newItem);
        _mecrchantItemsQuantity.Add(100);
        GameObject newItem1 = Instantiate(itemPrefab, _spawnPointForItems.position, Quaternion.identity);
        newItem1.GetComponent<Item>().SetParams("Healing salve", "Potion for heling. Cost: 100g", 3, _spritesForItems[1], 10, 100, itemPrefab);
        newItem1.GetComponent<Item>().MerchantHoldsItem();
        _merchantItems.Add(newItem1);
        _mecrchantItemsQuantity.Add(10);
        GameObject newItem2 = Instantiate(itemPrefab, _spawnPointForItems.position, Quaternion.identity);
        newItem2.GetComponent<Item>().SetParams("Rare merchant stuff", "Merchant stuff. rare. Cost: 250g", 4, _spritesForItems[2], 2, 250, itemPrefab);
        newItem2.GetComponent<Item>().MerchantHoldsItem();
        _merchantItems.Add(newItem2);
        _mecrchantItemsQuantity.Add(2);
        _merchantGold = 1000;
        RedrawExchangeUI();
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
            Button buttonO1 = itemUI.transform.GetChild(4).GetComponent<Button>();
            buttonO.onClick.AddListener(() => DropItem(item));
            buttonO1.onClick.AddListener(() => DrawDescription(item));


            itemUI.transform.SetParent(inventoryContentUI.transform, false);
        }
        //draw gold
        invGoldText.text = "Gold: " + _gold.ToString();
    }

    private void RedrawExchangeUI()
    {
        //clear exchange UI
        foreach (Transform child in playerContentUI.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in exchangeToBuyContentUI.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in exchangeToSellContentUI.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in merchantContentUI.transform)
        {
            Destroy(child.gameObject);
        }
        //draw player items
        foreach (GameObject item in items)
        {
            GameObject itemUI = Instantiate(itemPrefabExchangeUI, playerContentUI.transform);
            itemUI.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<Item>().GetItemIcon();
            itemUI.transform.GetChild(1).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemName();
            itemUI.transform.GetChild(3).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemCost().ToString();
            itemUI.transform.GetChild(2).GetComponent<TMP_Text>().text = itemsQuantity[items.IndexOf(item)].ToString();
            Button buttonO = itemUI.transform.GetChild(4).GetComponent<Button>();
            buttonO.onClick.AddListener(() => ExchangeChoose(items.IndexOf(item),0));
            itemUI.transform.SetParent(playerContentUI.transform, false);
        }
        //draw exchange items to buy
        foreach (GameObject item in _exchangeItemsToBuy)
        {
            GameObject itemUI = Instantiate(itemPrefabExchangeUI, exchangeToBuyContentUI.transform);
            itemUI.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<Item>().GetItemIcon();
            itemUI.transform.GetChild(1).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemName();
            itemUI.transform.GetChild(3).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemCost().ToString();
            itemUI.transform.GetChild(2).GetComponent<TMP_Text>().text = _exchangeItemsToBuyQuantity[_exchangeItemsToBuy.IndexOf(item)].ToString();
            Button buttonO = itemUI.transform.GetChild(4).GetComponent<Button>();
            buttonO.onClick.AddListener(() => ExchangeChoose(_exchangeItemsToBuy.IndexOf(item), 1));
            itemUI.transform.SetParent(exchangeToBuyContentUI.transform, false);
        }
        //draw exchange items to sell
        foreach (GameObject item in _exchangeItemsToSell)
        {
            GameObject itemUI = Instantiate(itemPrefabExchangeUI, exchangeToSellContentUI.transform);
            itemUI.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<Item>().GetItemIcon();
            itemUI.transform.GetChild(1).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemName();
            itemUI.transform.GetChild(3).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemCost().ToString();
            itemUI.transform.GetChild(2).GetComponent<TMP_Text>().text = _exchangeItemsToSellQuantity[_exchangeItemsToSell.IndexOf(item)].ToString();
            Button buttonO = itemUI.transform.GetChild(4).GetComponent<Button>();
            buttonO.onClick.AddListener(() => ExchangeChoose(_exchangeItemsToSell.IndexOf(item), 2));
            itemUI.transform.SetParent(exchangeToSellContentUI.transform, false);
        }
        //draw merchant items
        foreach (GameObject item in _merchantItems)
        {
            GameObject itemUI = Instantiate(itemPrefabExchangeUI, merchantContentUI.transform);
            itemUI.transform.GetChild(0).GetComponent<Image>().sprite = item.GetComponent<Item>().GetItemIcon();
            itemUI.transform.GetChild(1).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemName();
            itemUI.transform.GetChild(3).GetComponent<TMP_Text>().text = item.GetComponent<Item>().GetItemCost().ToString();
            itemUI.transform.GetChild(2).GetComponent<TMP_Text>().text = _mecrchantItemsQuantity[_merchantItems.IndexOf(item)].ToString();
            Button buttonO = itemUI.transform.GetChild(4).GetComponent<Button>();
            buttonO.onClick.AddListener(() => ExchangeChoose(_merchantItems.IndexOf(item), 3));
            itemUI.transform.SetParent(merchantContentUI.transform, false);
        }
        //draw gold
        goldText.text = "Gold: "+ _gold.ToString();
        exGoldText.text = "Gold: " + (_goldFromMerchant-_goldFromPlayer).ToString();
        merchGoldText.text = "Gold: " + _merchantGold.ToString();

    }

    private void ExchangeChoose(int index, int choosenFlag)
    {
        Debug.Log(index + " " + choosenFlag);
        if (index< 0)
        {
            Debug.LogError("Invalid index for ExchangeChoose");
            return;
        }
        if(choosenFlag < 0 || choosenFlag > 3)
        {
            Debug.LogError("Invalid choosenFlag for ExchangeChoose");
            return;
        }
        _choosenHolder = choosenFlag;
        _choosenItemIndex = index;
        //clear exchange UI choosen color
        foreach (Transform child in playerContentUI.transform)
        {
            child.GetComponent<Image>().enabled = false;
        }
        foreach (Transform child in exchangeToBuyContentUI.transform)
        {
            child.GetComponent<Image>().enabled = false;
        }
        foreach (Transform child in exchangeToSellContentUI.transform)
        {
            child.GetComponent<Image>().enabled = false;
        }
        foreach (Transform child in merchantContentUI.transform)
        {
            child.GetComponent<Image>().enabled = false;
        }
        switch (choosenFlag)
        {
            case 0:
                //Player
                playerContentUI.transform.GetChild(index).GetComponent<Image>().enabled = true;
                _allLeftButton.interactable = false;
                _leftButton.interactable = false;
                _rightButton.interactable = true;
                _allRightButton.interactable = true;
                break;
            case 1:
                //Exchange to buy
                exchangeToBuyContentUI.transform.GetChild(index).GetComponent<Image>().enabled = true;
                _allLeftButton.interactable = false;
                _leftButton.interactable = false;
                _rightButton.interactable = true;
                _allRightButton.interactable = true;
                break;
            case 2:
                //Exchange to sell
                exchangeToSellContentUI.transform.GetChild(index).GetComponent<Image>().enabled = true;
                _allLeftButton.interactable = true;
                _leftButton.interactable = true;
                _rightButton.interactable = false;
                _allRightButton.interactable = false;
                break;
            case 3:
                //Merchant
                merchantContentUI.transform.GetChild(index).GetComponent<Image>().enabled = true;
                _allLeftButton.interactable = true;
                _leftButton.interactable = true;
                _rightButton.interactable = false;
                _allRightButton.interactable = false;
                break;
            default:
                Debug.LogError("Invalid choosenFlag for ExchangeChoose");
                break;
        }
    }

    public void DragToLeft(bool all)
    {
        if (_choosenHolder < 2 || _choosenHolder > 3)
        {
            Debug.LogError("Invalid choosenFlag for DragToLeft");
            return;
        }
        if (_choosenItemIndex < 0)
        {
            Debug.LogError("Invalid index for DragToLeft");
            return;
        }
        switch (_choosenHolder)
        {
            case 2:
                //Exchange to sell
                if (all)
                {
                    //from exchange back to player
                    if (items.Contains(_exchangeItemsToSell[_choosenItemIndex]))
                    {
                            itemsQuantity[items.IndexOf(_exchangeItemsToSell[_choosenItemIndex])] += _exchangeItemsToSellQuantity[_choosenItemIndex];
                    }
                    else
                    {
                         items.Add(_exchangeItemsToSell[_choosenItemIndex]);
                         itemsQuantity.Add(_exchangeItemsToSellQuantity[_choosenItemIndex]);

                    }
                    //_gold += _exchangeItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsQuantity[_choosenItemIndex];
                    _goldFromMerchant -= _exchangeItemsToSell[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsToSellQuantity[_choosenItemIndex];
                    _exchangeItemsToSellQuantity.RemoveAt(_choosenItemIndex);
                     _exchangeItemsToSell.RemoveAt(_choosenItemIndex);
                }
                else
                {
                        //get exchange count
                        if (int.TryParse(_exchangeInput.text, out _exchangeCount))
                        {
                            if (_exchangeCount > _exchangeItemsToSellQuantity[_choosenItemIndex])
                            {
                                _exchangeCount = _exchangeItemsToSellQuantity[_choosenItemIndex];
                            }
                            if (_exchangeCount <= 0)
                            {
                                _exchangeCount = 1;
                                _exchangeInput.text = "1";
                            }
                        }
                        else
                        {
                            _exchangeCount = 1;
                            _exchangeInput.text = "1";
                        }
                        if (items.Contains(_exchangeItemsToSell[_choosenItemIndex]))
                        {
                            itemsQuantity[items.IndexOf(_exchangeItemsToSell[_choosenItemIndex])] += _exchangeCount;
                        }
                        else
                        {
                            items.Add(_exchangeItemsToSell[_choosenItemIndex]);
                            itemsQuantity.Add(_exchangeCount);

                        }
                        
                        
                        if (_exchangeItemsToSellQuantity[_choosenItemIndex] > _exchangeCount)
                        {
                            //_gold += _exchangeItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                            _goldFromMerchant -= _exchangeItemsToSell[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                            _exchangeItemsToSellQuantity[_choosenItemIndex] -= _exchangeCount;
                        }
                        else
                        {
                            //_gold += _exchangeItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsQuantity[_choosenItemIndex];
                            _goldFromMerchant -= _exchangeItemsToSell[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsToSellQuantity[_choosenItemIndex];
                            _exchangeItemsToSellQuantity.RemoveAt(_choosenItemIndex);
                            _exchangeItemsToSell.RemoveAt(_choosenItemIndex);
                        }
                    
                }
                break;
            case 3:
                //Merchant
                if (all)
                {
                    //from merchant to exchange
                        if (_exchangeItemsToBuy.Contains(_merchantItems[_choosenItemIndex]))
                        {
                            _exchangeItemsToBuyQuantity[_exchangeItemsToBuy.IndexOf(_merchantItems[_choosenItemIndex])] += _mecrchantItemsQuantity[_choosenItemIndex];
                        }
                        else
                        {
                            _exchangeItemsToBuy.Add(_merchantItems[_choosenItemIndex]);
                            _exchangeItemsToBuyQuantity.Add(_mecrchantItemsQuantity[_choosenItemIndex]);
                        }
                        //_gold -= _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _mecrchantItemsQuantity[_choosenItemIndex];
                        _goldFromPlayer += _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _mecrchantItemsQuantity[_choosenItemIndex];
                        _mecrchantItemsQuantity.RemoveAt(_choosenItemIndex);
                        _merchantItems.RemoveAt(_choosenItemIndex);
                    
                }
                else
                {
                        //get exchange count
                        if (int.TryParse(_exchangeInput.text, out _exchangeCount))
                        {
                            if (_exchangeCount > _mecrchantItemsQuantity[_choosenItemIndex])
                            {
                                _exchangeCount = _mecrchantItemsQuantity[_choosenItemIndex];
                            }
                            if (_exchangeCount <= 0)
                            {
                                _exchangeCount = 1;
                                _exchangeInput.text = "1";
                            }
                        }
                        else
                        {
                            _exchangeCount = 1;
                            _exchangeInput.text = "1";
                        }
                        if (_exchangeItemsToBuy.Contains(_merchantItems[_choosenItemIndex]))
                        {
                            _exchangeItemsToBuyQuantity[_exchangeItemsToBuy.IndexOf(_merchantItems[_choosenItemIndex])] += _exchangeCount;
                        }
                        else
                        {
                            _exchangeItemsToBuy.Add(_merchantItems[_choosenItemIndex]);
                            _exchangeItemsToBuyQuantity.Add(_exchangeCount);
                        }
                        //remove from merchant
                        if (_mecrchantItemsQuantity[_choosenItemIndex] > _exchangeCount)
                        {
                            //_gold -= _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                            _goldFromPlayer += _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                            _mecrchantItemsQuantity[_choosenItemIndex] -= _exchangeCount;
                        }
                        else
                        {
                            //_gold -= _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _mecrchantItemsQuantity[_choosenItemIndex];
                            _goldFromPlayer += _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _mecrchantItemsQuantity[_choosenItemIndex];
                            _mecrchantItemsQuantity.RemoveAt(_choosenItemIndex);
                            _merchantItems.RemoveAt(_choosenItemIndex);
                        }
                    }
                break;
            default:
                Debug.LogError("Invalid choosenFlag for DragToLeft");
                break;

        }
        RedrawExchangeUI();
    }

    public void DragToRight(bool all)
    {
        if (_choosenHolder < 0 || _choosenHolder > 1)
        {
            Debug.LogError("Invalid choosenFlag for DragToLeft");
            return;
        }
        if (_choosenItemIndex < 0)
        {
            Debug.LogError("Invalid index for DragToLeft");
            return;
        }
        switch (_choosenHolder)
        {
            case 0:
                //from player to exchange
                if (all)
                {
                    //from player to exchange
                    if (_exchangeItemsToSell.Contains(items[_choosenItemIndex]))
                    {
                        _exchangeItemsToSellQuantity[_exchangeItemsToSell.IndexOf(items[_choosenItemIndex])] += itemsQuantity[_choosenItemIndex];
                    }
                    else
                    {
                        _exchangeItemsToSell.Add(items[_choosenItemIndex]);
                        _exchangeItemsToSellQuantity.Add(itemsQuantity[_choosenItemIndex]);
                    }
                    //_gold += items[_choosenItemIndex].GetComponent<Item>().GetItemCost() * itemsQuantity[_choosenItemIndex];
                    _goldFromMerchant += items[_choosenItemIndex].GetComponent<Item>().GetItemCost() * itemsQuantity[_choosenItemIndex];
                    itemsQuantity.RemoveAt(_choosenItemIndex);
                    items.RemoveAt(_choosenItemIndex);
                }
                else
                {
                    //get exchange count
                    if (int.TryParse(_exchangeInput.text, out _exchangeCount))
                    {
                        if (_exchangeCount > itemsQuantity[_choosenItemIndex])
                        {
                            _exchangeCount = itemsQuantity[_choosenItemIndex];
                        }
                        if (_exchangeCount <= 0)
                        {
                            _exchangeCount = 1;
                            _exchangeInput.text = "1";
                        }
                    }
                    else
                    {
                        _exchangeCount = 1;
                        _exchangeInput.text = "1";
                    }
                    if (_exchangeItemsToSell.Contains(items[_choosenItemIndex]))
                    {
                        _exchangeItemsToSellQuantity[_exchangeItemsToSell.IndexOf(items[_choosenItemIndex])] += _exchangeCount;
                    }
                    else
                    {
                        _exchangeItemsToSell.Add(items[_choosenItemIndex]);
                        _exchangeItemsToSellQuantity.Add(_exchangeCount);
                    }
                    //remove from player
                    if (itemsQuantity[_choosenItemIndex] > _exchangeCount)
                    {
                        //_gold += items[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                        _goldFromMerchant += items[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                        itemsQuantity[_choosenItemIndex] -= _exchangeCount;
                    }
                    else
                    {
                        //_gold += items[_choosenItemIndex].GetComponent<Item>().GetItemCost() * itemsQuantity[_choosenItemIndex];
                        _goldFromMerchant += items[_choosenItemIndex].GetComponent<Item>().GetItemCost() * itemsQuantity[_choosenItemIndex];
                        itemsQuantity.RemoveAt(_choosenItemIndex);
                        items.RemoveAt(_choosenItemIndex);
                    }
                }
                break;
            case 1:
                //from exchange to merchant
                if (all)
                {
                    //from exchange to merchant
                    if (_merchantItems.Contains(_exchangeItemsToBuy[_choosenItemIndex]))
                    {
                        _mecrchantItemsQuantity[_merchantItems.IndexOf(_exchangeItemsToBuy[_choosenItemIndex])] += _exchangeItemsToBuyQuantity[_choosenItemIndex];
                    }
                    else
                    {
                        _merchantItems.Add(_exchangeItemsToBuy[_choosenItemIndex]);
                        _mecrchantItemsQuantity.Add(_exchangeItemsToBuyQuantity[_choosenItemIndex]);
                    }
                    //_gold += _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsQuantity[_choosenItemIndex];
                    _goldFromPlayer -= _exchangeItemsToBuy[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsToBuyQuantity[_choosenItemIndex];
                    _exchangeItemsToBuyQuantity.RemoveAt(_choosenItemIndex);
                    _exchangeItemsToBuy.RemoveAt(_choosenItemIndex);
                }
                else
                {
                    //get exchange count
                    if (int.TryParse(_exchangeInput.text, out _exchangeCount))
                    {
                        if (_exchangeCount > _exchangeItemsToBuyQuantity[_choosenItemIndex])
                        {
                            _exchangeCount = _exchangeItemsToBuyQuantity[_choosenItemIndex];
                        }
                        if (_exchangeCount <= 0)
                        {
                            _exchangeCount = 1;
                            _exchangeInput.text = "1";
                        }
                    }
                    else
                    {
                        _exchangeCount = 1;
                        _exchangeInput.text = "1";
                    }
                    if (_merchantItems.Contains(_exchangeItemsToBuy[_choosenItemIndex]))
                    {
                        _mecrchantItemsQuantity[_merchantItems.IndexOf(_exchangeItemsToBuy[_choosenItemIndex])] += _exchangeCount;
                    }
                    else
                    {
                        _merchantItems.Add(_exchangeItemsToBuy[_choosenItemIndex]);
                        _mecrchantItemsQuantity.Add(_exchangeCount);
                    }
                    //remove from exchange
                    if (_exchangeItemsToBuyQuantity[_choosenItemIndex] > _exchangeCount)
                    {
                        //_gold -= _exchangeItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                        _goldFromPlayer -= _exchangeItemsToBuy[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeCount;
                        _mecrchantItemsQuantity[_choosenItemIndex] -= _exchangeCount;
                    }
                    else
                    {
                        //_gold += _merchantItems[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsQuantity[_choosenItemIndex];
                        _goldFromPlayer -= _exchangeItemsToBuy[_choosenItemIndex].GetComponent<Item>().GetItemCost() * _exchangeItemsToBuyQuantity[_choosenItemIndex];
                        _exchangeItemsToBuyQuantity.RemoveAt(_choosenItemIndex);
                        _exchangeItemsToBuy.RemoveAt(_choosenItemIndex);
                    }
                }
                break;
            default:
                Debug.LogError("Invalid choosenFlag for DragToLeft");
                break;
        }
        RedrawExchangeUI();
    }

    public void ConfirmExchange()
    {
        //check if player has enough gold
        if ((_goldFromPlayer - _goldFromMerchant) > _gold) 
        {
            ShowMessage("Not enough gold");
            CancelExchange();
            return;
        }
        //check if merchant has enough gold
        if ((_goldFromMerchant-_goldFromPlayer) > _merchantGold)
        {
            ShowMessage("Merchant not enough gold");
            CancelExchange();
            return;
        }
        if(_goldFromPlayer==0 && _goldFromMerchant == 0)
        {
            ShowMessage("Nothing to exchange");
            CancelExchange();
            return;
        }
        //exchange items
        _gold += _goldFromMerchant;
        _merchantGold -= _goldFromMerchant;
        _gold -= _goldFromPlayer;
        _merchantGold += _goldFromPlayer;
        _goldFromPlayer = 0;
        _goldFromMerchant = 0;
        //transfer items to buy to player
        foreach (GameObject item in _exchangeItemsToBuy)
        {
            if (items.Contains(item))
            {
                itemsQuantity[items.IndexOf(item)] += _exchangeItemsToBuyQuantity[_exchangeItemsToBuy.IndexOf(item)];
            }
            else
            {
                items.Add(item);
                itemsQuantity.Add(_exchangeItemsToBuyQuantity[_exchangeItemsToBuy.IndexOf(item)]);
            }
        }
        _exchangeItemsToBuy.Clear();
        _exchangeItemsToBuyQuantity.Clear();
        //transfer items to sell to merchant
        foreach (GameObject item in _exchangeItemsToSell)
        {
            if (_merchantItems.Contains(item))
            {
                _mecrchantItemsQuantity[_merchantItems.IndexOf(item)] += _exchangeItemsToSellQuantity[_exchangeItemsToSell.IndexOf(item)];
            }
            else
            {
                _merchantItems.Add(item);
                _mecrchantItemsQuantity.Add(_exchangeItemsToSellQuantity[_exchangeItemsToSell.IndexOf(item)]);
            }
        }
        _exchangeItemsToSell.Clear();
        _exchangeItemsToSellQuantity.Clear();
        ShowMessage("Exchange complete");
        FinishTrading();
    }

    private void FinishTrading()
    {
        RedrawExchangeUI();
        RedrawUI();
        exchangeUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        UIController._instance._isInInventory = false;
    }

    public void CancelExchange()
    {
        //transfer items to sell back to player
        foreach(GameObject item in _exchangeItemsToSell)
        {
            if (items.Contains(item))
            {
                itemsQuantity[items.IndexOf(item)] += _exchangeItemsToSellQuantity[_exchangeItemsToSell.IndexOf(item)];
            }
            else
            {
                items.Add(item);
                itemsQuantity.Add(_exchangeItemsToSellQuantity[_exchangeItemsToSell.IndexOf(item)]);
            }
        }
        //transfer items to buy back to merchant
        foreach (GameObject item in _exchangeItemsToBuy)
        {
            if (_merchantItems.Contains(item))
            {
                _mecrchantItemsQuantity[_merchantItems.IndexOf(item)] += _exchangeItemsToBuyQuantity[_exchangeItemsToBuy.IndexOf(item)];
            }
            else
            {
                _merchantItems.Add(item);
                _mecrchantItemsQuantity.Add(_exchangeItemsToBuyQuantity[_exchangeItemsToBuy.IndexOf(item)]);
            }
        }
        _goldFromPlayer = 0;
        _goldFromMerchant = 0;
        _exchangeItemsToBuy.Clear();
        _exchangeItemsToBuyQuantity.Clear();
        _exchangeItemsToSell.Clear();
        _exchangeItemsToSellQuantity.Clear();
        FinishTrading();
    }

    private void DrawDescription(GameObject item)
    {
        string newDescription = item.GetComponent<Item>().GetItemDescription();
        itemDescriptionText.text = newDescription;
    }

    private void DropItem(GameObject item)
    {
        item.GetComponent<Item>().DropItem(_playerController.GetPlayerPosition(1));
        itemsQuantity.RemoveAt(items.IndexOf(item));
        items.Remove(item);
        
        RedrawUI();
    }
    //show messages
    void ShowMessage(string msg)
    {
        _messagingUI.gameObject.SetActive(true);
        _messagingUI.GetChild(0).GetComponent<TMP_Text>().text = msg;
        StartCoroutine(StopShowMessage());
    }



    //stop showing messages coroutine
    private IEnumerator StopShowMessage()
    {
        yield return new WaitForSeconds(3);
        _messagingUI.gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        InputController.OnInteractInput -= InteractHandler;
    }
}
