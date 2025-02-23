using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectionManager : MonoBehaviour
{
    public static ButtonSelectionManager instance;
    public GameObject lastSelectedButton { get; set; }
    public int lastSelectedButtonIndex { get; set; }

    public GameObject[] buttons;

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
    }

    private void OnEnable()
    {
        StartCoroutine(SetSelectedAfterOneFrame());
    }

    private void Update()
    {
        if(InputManager.instance.MovementInput.y > 0)
        {
            if (lastSelectedButtonIndex > 0)
            {
                HandleNextButtonSelection(-1);
            }
        }
        else if(InputManager.instance.MovementInput.y < 0)
        {
            HandleNextButtonSelection(1);
        }
    }

    private IEnumerator SetSelectedAfterOneFrame()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(buttons[0]);
    }

    private void HandleNextButtonSelection(int addition)
    {
        if(EventSystem.current.currentSelectedGameObject == null && lastSelectedButton != null)
        {
            int newIndex = lastSelectedButtonIndex + addition;
            newIndex = Mathf.Clamp(newIndex, 0, buttons.Length - 1);
            EventSystem.current.SetSelectedGameObject(buttons[newIndex]);
        }
    }
}
