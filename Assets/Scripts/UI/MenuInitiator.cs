using System.Collections;
using UnityEngine;

public class MenuInitiator : MonoBehaviour
{
#region private variables
        [SerializeField] private GameObject MainPanel;
        [SerializeField] private GameObject OptionPanel;
        [SerializeField] private GameObject ScorePanel;
        [SerializeField] private GameObject LeavePanel;
#endregion

#region Private Methods
        private IEnumerator Start()
        {
            yield return StartCoroutine(InitiateMenu());
        }
    
        private IEnumerator InitiateMenu()
        {
            MainPanel.SetActive(true);
            OptionPanel.SetActive(false);
            ScorePanel.SetActive(false);
            LeavePanel.SetActive(false);
    
            yield break;
        }
#endregion
}
