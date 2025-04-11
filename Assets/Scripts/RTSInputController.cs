using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RTSInputController : MonoBehaviour
{
    public static event Action OnChooseInput;
    public static event Action OnActInput;
    public static event Action OnPauseInput;
    public static event Action OnControlInput;
    public static event Action OnControlInputCanceled;

    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private string _mapRTSName;
    [SerializeField] private string _chooseRTSName;
    [SerializeField] private string _actRTSName;
    [SerializeField] private string _pauseName;
    [SerializeField] private string _controlName;

    InputAction _chooseAction;
    InputAction _actAction;
    InputAction _pauseAction;
    InputAction _controlAction;

    private InputActionMap _RTSActionMap;

    private void OnEnable()
    {
        _inputActionAsset.Enable();
        _RTSActionMap = _inputActionAsset.FindActionMap(_mapRTSName);
        _chooseAction = _RTSActionMap[_chooseRTSName];
        _actAction = _RTSActionMap[_actRTSName];
        _pauseAction = _RTSActionMap[_pauseName];
        _controlAction = _RTSActionMap[_controlName];

        _chooseAction.performed += ChoosePerformedHandler;
        _actAction.performed += ActPerformedHandler;
        _pauseAction.performed += PausePerformedHandler;
        _controlAction.performed += ControlPerformedHandler;
        _controlAction.canceled += ControlCanceledHandler;
    }

    private void ControlCanceledHandler(InputAction.CallbackContext context)
    {
        OnControlInputCanceled?.Invoke();
    }

    private void ControlPerformedHandler(InputAction.CallbackContext context)
    {
        OnControlInput?.Invoke();
    }

    private void OnDestroy()
    {
        _chooseAction.performed -= ChoosePerformedHandler;
        _actAction.performed -= ActPerformedHandler;
        _pauseAction.performed -= PausePerformedHandler;

        OnChooseInput = null;
        OnActInput = null;
        OnPauseInput = null;
    }

    private void PausePerformedHandler(InputAction.CallbackContext context)
    {
        OnPauseInput?.Invoke();
    }

    private void ChoosePerformedHandler(InputAction.CallbackContext context)
    {
        OnChooseInput?.Invoke();
    }
    private void ActPerformedHandler(InputAction.CallbackContext context)
    {
        OnActInput?.Invoke();
    }
}
