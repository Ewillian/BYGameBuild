using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("DifficultyLevel", (int) DifficultyType.Easy);
    }

    public void SetDifficulty(int newDifficulty)
    {
        PlayerPrefs.SetInt("DifficultyLevel", DifficultyType.IsDefined(typeof(DifficultyType), newDifficulty) ? newDifficulty : (int) DifficultyType.Easy);
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
