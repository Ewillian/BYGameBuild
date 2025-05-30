using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Fields

    private static float POWER_VALUE_GAIN = .1f;
    private static float POWER_VALUE_LOSE = .15f;

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

    #endregion Fields

    #region Public methods

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        _slider = GetComponent<Slider>();

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

    }

    private void UpdatePowerValue()
    {
        if (InputManager.Action())
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
