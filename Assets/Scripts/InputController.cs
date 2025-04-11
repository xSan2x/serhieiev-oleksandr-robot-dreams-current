using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;
    public static event Action<Vector2> OnLookInput;
    public static event Action OnPrimaryInput;
    public static event Action<bool> OnSecondaryInput;
    public static event Action OnStatsInput;
    public static event Action OnStatsCanceledInput;
    public static event Action OnPauseInput;
    public static event Action OnInventoryInput;
    public static event Action OnInteractInput;

    [SerializeField] private CursorLockMode _enabledCursorMode;
    [SerializeField] private CursorLockMode _disabledCursorMode;

    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private string _mapFPSName;
    [SerializeField] private string _moveFPSName;
    [SerializeField] private string _lookFPSAroundName;
    [SerializeField] private string _primaryFireName;
    [SerializeField] private string _secondaryFireName;
    [SerializeField] private string _statsName;
    [SerializeField] private string _pauseName;
    [SerializeField] private string _inventoryName;
    [SerializeField] private string _interactName;

    private InputAction _moveFPSAction;
    private InputAction _lookAroundFPSAction;
    private InputAction _primaryFireAction;
    private InputAction _secondaryFireAction;
    private InputAction _statsAction;
    private InputAction _pauseAction;
    private InputAction _inventoryAction;
    private InputAction _interactAction;

    private InputActionMap _actionFPSMap;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _inputActionAsset.Enable();

        _actionFPSMap = _inputActionAsset.FindActionMap(_mapFPSName);

        _moveFPSAction = _actionFPSMap[_moveFPSName];
        _lookAroundFPSAction = _actionFPSMap[_lookFPSAroundName];
        //_pointerPositionAction = actionMap[_pointerPositionName];
        _primaryFireAction = _actionFPSMap[_primaryFireName];
        _secondaryFireAction = _actionFPSMap[_secondaryFireName];
        _statsAction = _actionFPSMap[_statsName];
        _pauseAction = _actionFPSMap[_pauseName];
        _inventoryAction = _actionFPSMap[_inventoryName];
        _interactAction = _actionFPSMap[_interactName];

        _moveFPSAction.performed += MovePerformedHandler;
        _moveFPSAction.canceled += MoveCanceledHandler;

        _lookAroundFPSAction.performed += LookPerformedHandler;
        _lookAroundFPSAction.canceled += LookPerformedHandler;

        _primaryFireAction.performed += PrimaryFirePerformedHandler;

        _secondaryFireAction.performed += SecondaryFirePerformedHandler;
        _secondaryFireAction.canceled += SecondaryFireCanceledHandler;

        _statsAction.performed += StatsPerformedHandler;
        _statsAction.canceled += StatsCanceledHandler;

        _pauseAction.performed += PausePerformedHandler;
        _inventoryAction.performed += InventoryPerformedHandler;
        _interactAction.performed += InteractPerformedHandler;
    }

    private void InteractPerformedHandler(InputAction.CallbackContext context)
    {
        OnInteractInput?.Invoke();
    }

    private void InventoryPerformedHandler(InputAction.CallbackContext context)
    {
        OnInventoryInput?.Invoke();
    }

    private void PausePerformedHandler(InputAction.CallbackContext context)
    {
        OnPauseInput?.Invoke();
    }

    private void StatsCanceledHandler(InputAction.CallbackContext context)
    {
        OnStatsInput?.Invoke();
    }

    private void StatsPerformedHandler(InputAction.CallbackContext context)
    {
        OnStatsCanceledInput?.Invoke();
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = _disabledCursorMode;

        _actionFPSMap.Disable();
    }

    private void OnDestroy()
    {
        _moveFPSAction.performed -= MovePerformedHandler;
        _moveFPSAction.canceled -= MoveCanceledHandler;

        _lookAroundFPSAction.performed -= LookPerformedHandler;

        _primaryFireAction.performed -= PrimaryFirePerformedHandler;

        _secondaryFireAction.performed -= SecondaryFirePerformedHandler;
        _secondaryFireAction.canceled -= SecondaryFireCanceledHandler;

        _statsAction.performed -= StatsPerformedHandler;
        _statsAction.canceled -= StatsCanceledHandler;

        OnMoveInput = null;
        OnLookInput = null;
        OnPrimaryInput = null;
        OnSecondaryInput = null;
        OnStatsInput = null;
        OnStatsCanceledInput = null;
        OnPauseInput = null;
    }

    private void SecondaryFireCanceledHandler(InputAction.CallbackContext context)
    {
        OnSecondaryInput?.Invoke(false);
    }

    private void SecondaryFirePerformedHandler(InputAction.CallbackContext context)
    {
        OnSecondaryInput?.Invoke(true);
    }

    private void PrimaryFirePerformedHandler(InputAction.CallbackContext context)
    {
        OnPrimaryInput?.Invoke();
    }

    private void LookPerformedHandler(InputAction.CallbackContext context)
    {
        OnLookInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void MoveCanceledHandler(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    private void MovePerformedHandler(InputAction.CallbackContext context)
    {
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }
}
