using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private TMPro.TextMeshProUGUI _accuracyText;
    [SerializeField] private TMPro.TextMeshProUGUI _headshotsText;
    private float _shotsMade = 0;
    private float _shotsHit = 0;
    private float _headshots = 0;
    public static UIController _instance;

    public bool _isPaused = false;
    public bool _isInInventory = false;

    private void Awake()
    {
        //singleton
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _accuracyText.text = "Accuracy: 0%";
        _headshotsText.text = "Headshots: 0%";
        _statsPanel.SetActive(false);
        Time.timeScale = 1;
        InputController.OnStatsInput += StatsInputHandler;
        InputController.OnStatsCanceledInput += StatsCanceledInputHandler;
        InputController.OnPauseInput += PauseInputHandler;
        InputController.OnInventoryInput += InventoryInputHandler;
    }

    private void InventoryInputHandler()
    {
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
        if (_inventoryPanel.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _isInInventory = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isInInventory = false;
        }
    }

    private void PauseInputHandler()
    {
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        if(_pausePanel.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            _isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isPaused = false;
            Time.timeScale = 1;
        }
        
    }

    private void StatsCanceledInputHandler()
    {
        _statsPanel.SetActive(true);
    }

    private void StatsInputHandler()
    {
        _statsPanel.SetActive(false);
    }

    public void UpdateStats(bool hit, bool headshot)
    {
        _shotsMade++;
        if (hit)
        {
            _shotsHit++;
        }
        if (headshot)
        {
            _headshots++;
        }
        _accuracyText.text = "Accuracy: " + (_shotsHit / _shotsMade * 100).ToString("F2") + "% "+"("+_shotsHit+" / "+_shotsMade+")";
        _headshotsText.text = "Headshots: " + (_headshots / _shotsHit * 100).ToString("F2") + "%"+ "(" + _headshots + " / " + _shotsHit + ")";
        
    }

    private void OnDestroy()
    {
        InputController.OnStatsInput -= StatsInputHandler;
        InputController.OnStatsCanceledInput -= StatsCanceledInputHandler;
        InputController.OnPauseInput -= PauseInputHandler;
        InputController.OnInventoryInput -= InventoryInputHandler;
    }
    public void ExitToMenu()
    {
        //Load the main menu
        SceneManager.LoadScene(0);
    }
}
