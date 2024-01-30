using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get { return _instance; }
    }

    private PlayerControls _playerControls;
    private RCC_InputActions _inputActions;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
            return;
        }
        _instance = this;

    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public bool PlayerInteractThisFrame()
    {
        return _playerControls.Player.Interact.triggered;
    }

    public bool PlayerStartStopEngineThisFrame()
    {
        return _playerControls.Player.StartStopEngine.triggered;
    }
}
