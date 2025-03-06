using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles button selection animations and interactions.
/// </summary>
public class ButtonSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    /// <summary>
    /// Amount to move the button vertically when selected.
    /// </summary>
    [SerializeField] private float _verticalMoveAmount = 1.0f;

    /// <summary>
    /// Time taken to move the button.
    /// </summary>
    [SerializeField] private float _moveTime = 0.1f;

    /// <summary>
    /// Scale factor to apply when the button is selected.
    /// </summary>
    [Range(0.0f, 2.0f), SerializeField] private float _scaleFactor = 1.2f;

    /// <summary>
    /// The initial position of the button.
    /// </summary>
    private Vector3 _startPosition;

    /// <summary>
    /// The original scale of the button.
    /// </summary>
    private Vector3 _originalScale;

    /// <summary>
    /// Initializes the start position and original scale of the button.
    /// </summary>
    void Start()
    {
        _startPosition = transform.position;
        _originalScale = transform.localScale;
    }

    /// <summary>
    /// Moves and scales the button.
    /// </summary>
    /// <param name="startingAnimation">True if starting animation, false if ending animation.</param>
    private IEnumerator MoveButton(bool startingAnimation)
    {
        float elapsedTime = 0.0f;
        Vector3 targetPosition = startingAnimation ? _startPosition + new Vector3(0.0f, _verticalMoveAmount, 0.0f) : _startPosition;
        Vector3 startingPosition = transform.position;
        Vector3 startingScale = transform.localScale;
        Vector3 targetScale = startingAnimation ? _originalScale * _scaleFactor : _originalScale;

        while (elapsedTime < _moveTime)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / _moveTime);
            transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / _moveTime);
            elapsedTime += Time.deltaTime;
        }
        transform.position = targetPosition;
        transform.localScale = targetScale;
        yield return null;
    }

    /// <summary>
    /// Called when the pointer enters the button.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    /// <summary>
    /// Called when the pointer exits the button.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    /// <summary>
    /// Called when the button is selected.
    /// </summary>
    /// <param name="eventData">Base event data.</param>
    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(true));

        ButtonSelectionManager.instance.lastSelectedButton = gameObject;

        for (int i = 0; i < ButtonSelectionManager.instance.buttons.Length; i++)
        {
            if (ButtonSelectionManager.instance.buttons[i] == gameObject)
            {
                ButtonSelectionManager.instance.lastSelectedButtonIndex = i;
                break;
            }
        }
    }

    /// <summary>
    /// Called when the button is deselected.
    /// </summary>
    /// <param name="eventData">Base event data.</param>
    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(false));
    }
}
