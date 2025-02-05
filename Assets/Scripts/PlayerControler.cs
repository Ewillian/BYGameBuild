using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    private static int POWER_VALUE_MAX = 100;
    private static int POWER_VALUE_MIN = 0;

    private static int POWER_VALUE_GAIN = 10;
    private static int POWER_VALUE_LOSE = 5;

    private Mouse _mouse;

    private int _currentPowerValue = 0;
    private bool _isPowerClick = false;

    private void Awake()
    {
        _mouse = Mouse.current;
    }

    private void Start()
    {
        InvokeRepeating("UpdatePowerValue", 0, 1f);
    }

    private void Update()
    {
        _isPowerClick = (_mouse.leftButton.isPressed ? true : (_mouse.leftButton.wasReleasedThisFrame ? false : _isPowerClick));
    }

    private void UpdatePowerValue()
    {
        Debug.Log("UpdatePowerValue");
        if (_isPowerClick)
        {
            _currentPowerValue = _currentPowerValue + POWER_VALUE_GAIN < POWER_VALUE_MAX ? _currentPowerValue + POWER_VALUE_GAIN : POWER_VALUE_MAX;
            Debug.Log("Pressed left-click.");
        }
        else
        {
            _currentPowerValue = _currentPowerValue - POWER_VALUE_LOSE > POWER_VALUE_MIN ? _currentPowerValue - POWER_VALUE_LOSE : POWER_VALUE_MIN;
            Debug.Log("Unpressed left-click.");
        }

        Debug.Log("Power: " + _currentPowerValue);
    }
}
