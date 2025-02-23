using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        StartCoroutine(SetEndAndStartButton());
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

    private IEnumerator SetEndAndStartButton()
    {
        yield return null;
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation navigation = buttons[i].GetComponent<Button>().navigation;
            if(i == 0){
                navigation.selectOnUp = buttons.LastOrDefault().GetComponent<Button>();
                navigation.selectOnDown = buttons[Mathf.Clamp(i + 1, 0, buttons.Length - 1)].GetComponent<Button>();
                buttons[i].GetComponent<Button>().navigation = navigation;
            } else if(i == buttons.Length - 1){
                navigation.selectOnUp = buttons[Mathf.Clamp(i - 1, 0, buttons.Length - 1)].GetComponent<Button>();
                navigation.selectOnDown = buttons.FirstOrDefault().GetComponent<Button>();
                buttons[i].GetComponent<Button>().navigation = navigation;
            }
            else{
                navigation.selectOnUp = buttons[Mathf.Clamp(i - 1, 0, buttons.Length - 1)].GetComponent<Button>();
                navigation.selectOnDown = buttons[Mathf.Clamp(i + 1, 0, buttons.Length - 1)].GetComponent<Button>();
                buttons[i].GetComponent<Button>().navigation = navigation;
            }
        }
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
