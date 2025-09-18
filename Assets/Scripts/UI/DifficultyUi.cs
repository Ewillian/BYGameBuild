using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("DifficultyLevel", (int) DifficultyEnum.Easy);
    }

    public void SetDifficulty(int newDifficulty)
    {
        PlayerPrefs.SetInt("DifficultyLevel", DifficultyEnum.IsDefined(typeof(DifficultyEnum), newDifficulty) ? newDifficulty : (int) DifficultyEnum.Easy);
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
