using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiDeviceKeybindManager : MonoBehaviour
{
    public TextMeshProUGUI mouseText, keyboardText, gamepadText;

    private PlayerInput playerInput;
    private InputAction action;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = InputManager.instance.GetComponent<PlayerInput>();
        if(playerInput == null)
        {
            Debug.LogError("PlayerInput not found");
            return;
        }

        action = playerInput.actions["PrincipalAction"];
        LoadBindings();
    }

    /// <summary>
    /// 
    /// </summary>
    void LoadBindings()
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            string deviceKey = "";

            if (binding.isComposite || binding.isPartOfComposite)
                continue;

            if (binding.path.Contains("mouse"))
                deviceKey = "Mouse";
            else if (binding.path.Contains("Keyboard"))
                deviceKey = "Keyboard";
            else if (binding.path.Contains("Gamepad"))
                deviceKey = "Gamepad";

            string savedPath = PlayerPrefs.GetString($"PrincipalAction_{deviceKey}", "");

            if (!string.IsNullOrEmpty(savedPath))
            {
                action.ApplyBindingOverride(i, savedPath);
            }
                
            // Affichage
            string display = InputControlPath.ToHumanReadableString(
                string.IsNullOrEmpty(savedPath) ? binding.effectivePath : savedPath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            switch (deviceKey)
            {
                case "Keyboard": keyboardText.text = display; break;
                case "Mouse": mouseText.text = display; break;
                case "Gamepad": gamepadText.text = display; break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
