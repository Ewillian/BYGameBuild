using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages input actions and provides movement and action inputs.
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Fields

    /// <summary>
    /// Singleton instance of the InputManager.
    /// </summary>
    public static InputManager instance { get; private set; }

    /// <summary>
    /// PlayerInput component.
    /// </summary>
    public static PlayerInput PlayerInput { get; private set; }

    /// <summary>
    /// Stores the movement input.
    /// </summary>
    public Vector2 MovementInput { get; private set; }

    /// <summary>
    /// Stores the action input.
    /// </summary>
    public bool ActionInput { get; private set; }

    /// <summary>
    /// Stores the action input.
    /// </summary>
    public InputAction PauseAction { get; private set; }

    private InputAction _movementAction;
    private InputAction _principalAction;

    #endregion Fields

    #region Public methods

    /// <summary>
    /// Returns the movement input.
    /// </summary>
    public static Vector2 Movement()
    {
        return instance.MovementInput;
    }

    /// <summary>
    /// Returns the action input.
    /// </summary>
    public static bool Action()
    {
        return instance.ActionInput;
    }

    #endregion Public methods

    #region Private methods

    /// <summary>
    /// Initializes the singleton instance and sets up input actions.
    /// </summary>
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
        if (PlayerInput == null)
        {
            Debug.LogError("PlayerInput not found");
            return;
        }

        _movementAction = PlayerInput.actions["Navigate"];
        _principalAction = PlayerInput.actions["PrincipalAction"];
        PauseAction = PlayerInput.actions["PauseAction"];
    }

    /// <summary>
    /// Updates the movement and action inputs.
    /// </summary>
    private void Update()
    {
        MovementInput = _movementAction.ReadValue<Vector2>();
        ActionInput = _principalAction.IsPressed();
    }

    #endregion Private methods
}
