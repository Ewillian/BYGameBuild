using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float _verticalMoveAmount = 1.0f;
    [SerializeField] private float _moveTime = 0.1f;
    [Range(0.0f, 2.0f), SerializeField] private float _scaleFactor = 1.2f;

    private Vector3 _startPosition;
    private Vector3 _originalScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startPosition = transform.position;
        _originalScale = transform.localScale;
    }

    private IEnumerator MoveButton(bool startingAnimation){
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
            yield return null;
        }
        transform.position = targetPosition;
        transform.localScale = targetScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

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

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(MoveButton(false));
    }
}
