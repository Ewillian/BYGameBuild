using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiDeviceKeybindManager : MonoBehaviour
{
    public TextMeshProUGUI mouseText, keyboardText, gamepadText;

    public static MultiDeviceKeybindManager Instance { get; private set; }

    private PlayerInput playerInput;
    private InputAction action;


    void Awake()
    {
        SetInstance();
    }

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
    /// 
    /// </summary>
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

            if (isPlayerPrefToLoad) {
                string savedPath = PlayerPrefs.GetString($"PrincipalAction_{deviceKey}", "");

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
    /// 
    /// </summary>
    /// <param name="deviceKey"></param>
    public void StartRebind(string deviceKey)
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
                    operation.Cancel();
                }
            })
            .OnComplete(operation =>
            {
                var selectedControl = operation.selectedControl;

                Debug.Log(selectedControl.path);

                action.ApplyBindingOverride(bindingIndex, selectedControl.path);
                PlayerPrefs.SetString($"{action.name}_binding_{bindingIndex}", selectedControl.path);
                PlayerPrefs.Save();

                uiText.SetText(GetDisplayName(bindingIndex));
                operation.Dispose();
                action.Enable();
            })
            .OnCancel(operation =>
            {
                uiText.SetText(GetDisplayName(bindingIndex));
                operation.Dispose();
                action.Enable();
            });

        rebindOperation.Start();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="deviceKey"></param>
    /// <returns></returns>
    int FindBindingIndex(string deviceKey)
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
    /// 
    /// </summary>
    /// <param name="bindingIndex"></param>
    /// <returns></returns>
    string GetDisplayName(int bindingIndex)
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
    /// <param name="device"></param>
    /// <param name="deviceKey"></param>
    /// <returns></returns>
    bool IsAllowedDevice(InputDevice device, string deviceKey)
    {
        if (device is Keyboard && deviceKey == "Keyboard")
            return true;

        if (device is Mouse && deviceKey == "Mouse")
            return true;

        if (device is Gamepad && deviceKey == "Gamepad")
            return true;

        return false;
    }

    public void RestoreDefaultKeybinds()
    {
        action.Disable();
        action.RemoveAllBindingOverrides();
        action.Enable();
        LoadBindings(false);
    }
}
