using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiDeviceKeybindManager : MonoBehaviour
{
    #region Public variables

    /// <summary>
    /// Singleton instance of the <see cref="MultiDeviceKeybindManager"/>.
    /// </summary>
    public static MultiDeviceKeybindManager Instance { get; private set; }

    /// <summary>
    /// Defines the Unity TMP text for the mouse, keyboard and gamepad buttons
    /// </summary>
    public TextMeshProUGUI mouseText, keyboardText, gamepadText;

    [Header("Alert pop up")]
    public GameObject popupUI;
    public TMP_Text countdownText;
    public TMP_Text messageText;
    public GameObject ButtonsContainer;

    #endregion Public variables

    #region Private variables

    private const string DEFAULT_MESSAGE_VALUE = "Press key for";
    private const string WRONG_KEY_MESSAGE_VALUE = "The used key is not on the correct device";

    /// <summary>
    /// Defines the unity player input containing the input action keybinds
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// Defines the unity player input action for PrincipalAction
    /// </summary>
    private InputAction action;

    #endregion Private variables

    /// <summary>
    /// Default awake unity method
    /// </summary>
    void Awake()
    {
        SetInstance();
    }

    /// <summary>
    /// Default start unity method
    /// </summary>
    void Start()
    {
        playerInput = InputManager.instance.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found");
            return;
        }

        action = playerInput.actions["PrincipalAction"];
        LoadBindings();
    }

    /// <summary>
    /// Sets the singleton instance for <see cref="MultiDeviceKeybindManager"/>
    /// </summary>
    private void SetInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Loads the player keybinds or default and apply the modify buttons the correct text
    /// </summary>
    /// <param name="isPlayerPrefToLoad">True, loads player bindings. False load default bindings</param>
    public void LoadBindings(bool isPlayerPrefToLoad = true)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            string deviceKey = "";

            if (binding.isComposite || binding.isPartOfComposite)
                continue;

            if (binding.path.Contains("Mouse"))
                deviceKey = "Mouse";
            else if (binding.path.Contains("Keyboard"))
                deviceKey = "Keyboard";
            else if (binding.path.Contains("Gamepad"))
                deviceKey = "Gamepad";

            if (isPlayerPrefToLoad)
            {
                string savedPath = PlayerPrefs.GetString($"PrincipalAction_{deviceKey}", binding.path);

                if (!string.IsNullOrEmpty(savedPath))
                {
                    action.ApplyBindingOverride(i, savedPath);
                }
            }

            switch (deviceKey)
            {
                case "Keyboard":
                    keyboardText.SetText(GetDisplayName(i)); break;
                case "Mouse":
                    mouseText.SetText(GetDisplayName(i)); break;
                case "Gamepad":
                    gamepadText.SetText(GetDisplayName(i)); break;
            }
        }
    }

    /// <summary>
    /// Rebind keys for the selected device
    /// </summary>
    /// <param name="deviceKey">The device key</param>
    public void RebindKeys(string deviceKey)
    {
        int bindingIndex = FindBindingIndex(deviceKey);

        if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
        {
            Debug.LogError("Invalid binding index.");
            return;
        }

        TextMeshProUGUI uiText = deviceKey switch
        {
            "Mouse" => mouseText,
            "Gamepad" => gamepadText,
            _ => keyboardText,
        };

        uiText.text = "Press key...";
        //TODO: change to pop up waiting for confirmation
        HandlePopUp(true, $"Press Esc to cancel\n", $"{DEFAULT_MESSAGE_VALUE} {deviceKey}");

        action.Disable();

        var rebindOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnPotentialMatch(operation =>
            {
                var actualDevice = operation.selectedControl.device;
                if (!IsAllowedDevice(actualDevice, deviceKey))
                {
                    Debug.LogWarning($"Device type mismatch. Expected: {deviceKey}, but got: {actualDevice.name}");
                    //TODO pop up alert
                    operation.Dispose();
                    RebindKeys(deviceKey);
                    return;
                }
            })
            .OnComplete(operation =>
            {
                var selectedControl = operation.selectedControl;

                action.ApplyBindingOverride(bindingIndex, selectedControl.path);
                PlayerPrefs.SetString($"PrincipalAction_{deviceKey}", selectedControl.path);
                PlayerPrefs.Save();

                uiText.SetText(GetDisplayName(bindingIndex));
                operation.Dispose();
                HandlePopUp(false);
                action.Enable();
            })
            .OnCancel(operation =>
            {
                uiText.SetText(GetDisplayName(bindingIndex));
                HandlePopUp(false);
                operation.Dispose();
                action.Enable();
            });

        rebindOperation.Start();
    }

    /// <summary>
    /// 
    /// </summary>
    public void RestoreDefaultKeybinds()
    {
        action.Disable();
        action.RemoveAllBindingOverrides();
        action.Enable();
        LoadBindings(false);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            string deviceKey = GetDeviceKey(action.bindings[i]);
            PlayerPrefs.SetString($"PrincipalAction_{deviceKey}", action.bindings[i].path);
        }
    }

    /// <summary>
    /// Returns the bindings index for the selected device
    /// </summary>
    /// <param name="deviceKey">The device key</param>
    /// <returns>The bindings index</returns>
    private int FindBindingIndex(string deviceKey)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];
            if (b.isComposite || b.isPartOfComposite) continue;

            if (deviceKey == "Mouse" && b.path.Contains("Mouse")) return i;
            if (deviceKey == "Keyboard" && b.path.Contains("Keyboard")) return i;
            if (deviceKey == "Gamepad" && b.path.Contains("Gamepad")) return i;
        }
        return -1;
    }

    /// <summary>
    /// Returns the human readable bind key name
    /// </summary>
    /// <param name="bindingIndex">The binding index</param>
    /// <returns>The display name</returns>
    private string GetDisplayName(int bindingIndex)
    {
        return InputControlPath.ToHumanReadableString(
            action.GetBindingDisplayString(bindingIndex),
            InputControlPath.HumanReadableStringOptions.UseShortNames |
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="binding"></param>
    /// <returns></returns>
    private string GetDeviceKey(InputBinding binding)
    {
        string deviceKey = "";

        if (binding.path.Contains("Mouse"))
            deviceKey = "Mouse";
        else if (binding.path.Contains("Keyboard"))
            deviceKey = "Keyboard";
        else if (binding.path.Contains("Gamepad"))
            deviceKey = "Gamepad";

        return deviceKey;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="device"></param>
    /// <param name="deviceKey"></param>
    /// <returns></returns>
    private bool IsAllowedDevice(InputDevice device, string deviceKey)
    {
        if (device is Keyboard && deviceKey == "Keyboard")
            return true;

        if (device is Mouse && deviceKey == "Mouse")
            return true;

        if (device is Gamepad && deviceKey == "Gamepad")
            return true;

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isToBeDisplayed"></param>
    /// <param name="message"></param>
    /// <param name="countdownMessage"></param>
    private void HandlePopUp(bool isToBeDisplayed, string message = "", string countdownMessage = "")
    {
        messageText.SetText(message);
        countdownText.SetText(countdownMessage);
        ButtonsContainer.SetActive(false);
        popupUI.SetActive(isToBeDisplayed);
    }
}
