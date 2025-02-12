using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    #region Fields

    private static float POWER_VALUE_GAIN = .1f;
    private static float POWER_VALUE_LOSE = .15f;

    private Mouse _mouse;


    [SerializeField]
    private Slider _slider;
    public float PowerValue
    {
        get
        {
            return this._slider.value;
        }
        set
        {
            this._slider.value = value;
        }
    }

    private bool _isPowerClick = false;

    #endregion Fields

    #region Public methods

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _mouse = Mouse.current;

        PowerValue = 0;
    }

    private void Start()
    {
        // Starting in 0 seconds, a projectile will be launched every 0.1 seconds
        InvokeRepeating("UpdatePowerValue", 0, .01f);
        // CancelInvoke("UpdatePowerValue");
    }

    private void Update()
    {
        _isPowerClick = (_mouse.leftButton.isPressed ? true : (_mouse.leftButton.wasReleasedThisFrame ? false : _isPowerClick));
    }

    private void UpdatePowerValue()
    {
        // Debug.Log("UpdatePowerValue");
        if (_isPowerClick)
        {
            PowerValue = PowerValue + POWER_VALUE_GAIN < _slider.maxValue ? PowerValue + POWER_VALUE_GAIN : _slider.maxValue;
            // Debug.Log("Pressed left-click.");
        }
        else
        {
            PowerValue = PowerValue - POWER_VALUE_LOSE > _slider.minValue ? PowerValue - POWER_VALUE_LOSE : _slider.minValue;
            // Debug.Log("Unpressed left-click.");
        }

        // Debug.Log("Power: " + _slider.value);
    }

    #endregion Private methods
}
