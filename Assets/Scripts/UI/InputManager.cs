using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public static InputManager instance;
    public static PlayerInput PlayerInput { get; set; }

    public Vector2 MovementInput { get; set; }
    private InputAction _movementAction;


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
        _movementAction = PlayerInput.actions["Navigate"];
    }

    private void Update()
    {
        MovementInput = _movementAction.ReadValue<Vector2>();
    }
}
