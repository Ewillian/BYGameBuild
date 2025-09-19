using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInitiator : MonoBehaviour
{
    #region private variables 

    [SerializeField] private List<GameObject> MenuPanels;
    [SerializeField] private GameObject MainPanel;
    #endregion private variables

    #region Private Methods
    private IEnumerator Start()
    {
        yield return StartCoroutine(InitiateMenu());
    }

    private IEnumerator InitiateMenu()
    {
        MainPanel.SetActive(true);
        
        foreach (var panel in MenuPanels)
        {
            panel.SetActive(false);
        }

        yield break;
    }
    #endregion Private Methods

    #region Public Methods

    public void ShowMenu(GameObject panelToActivate)
    {
        if (!panelToActivate.activeSelf)
        {
            HideMenu();
            panelToActivate.SetActive(true);
        }
    }

    public void HideMenu()
    {
        foreach (var panel in MenuPanels)
        {
            panel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        StopAllCoroutines();
        Application.Quit();
    }
    
    #endregion Public Methods
}
