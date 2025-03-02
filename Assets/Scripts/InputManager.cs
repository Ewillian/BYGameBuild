using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Fields

    public static InputManager instance { get; private set; }
    public static PlayerInput PlayerInput { get; private set; }

    public Vector2 MovementInput { get; private set; }
    public bool ActionInput { get; private set; }

    private InputAction _movementAction;
    private InputAction _principalAction;

    #endregion Fields

    #region Public methods

    public static Vector2 Movement()
    {
        return instance.MovementInput;
    }

    public static bool Action()
    {
        return instance.ActionInput;
    }

    #endregion Public methods

    #region Private methods

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        PlayerInput = GetComponent<PlayerInput>();
        if(PlayerInput == null)
        {
            Debug.LogError("PlayerInput not found");
            return;
        }
        _movementAction = PlayerInput.actions["Navigate"];
        _principalAction = PlayerInput.actions["PrincipalAction"];
    }

    private void Update()
    {
        MovementInput = _movementAction.ReadValue<Vector2>();
        ActionInput = _principalAction.IsPressed();
    }

    #endregion Private methods
}
