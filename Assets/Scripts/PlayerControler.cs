using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    private static float POWER_VALUE_GAIN = .1f;
    private static float POWER_VALUE_LOSE = .15f;

    private Mouse _mouse;
    private Slider _slider;

    private bool _isPowerClick = false;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _mouse = Mouse.current;
    }

    private void Start()
    {
        InvokeRepeating("UpdatePowerValue", 0, .01f);
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
            _slider.value = _slider.value + POWER_VALUE_GAIN < _slider.maxValue ? _slider.value + POWER_VALUE_GAIN : _slider.maxValue;
            Debug.Log("Pressed left-click.");
        }
        else
        {
            _slider.value = _slider.value - POWER_VALUE_LOSE > _slider.minValue ? _slider.value - POWER_VALUE_LOSE : _slider.minValue;
            Debug.Log("Unpressed left-click.");
        }

        Debug.Log("Power: " + _slider.value);
    }
}
