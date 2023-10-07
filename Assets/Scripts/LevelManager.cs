using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float loadSceneDelay = 2f;

    ScoreKeeper scoreKeeper;

    void Awake()
    {
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
    }

    public void LoadGameScene()
    {
        scoreKeeper.ResetScore();
        LoadScene("Game", 0f);
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu", 0f);
    }

    public void LoadGameOver()
    {
        LoadScene("GameOver", loadSceneDelay);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void LoadScene(string sceneName, float delay)
    {
        StartCoroutine(WaitAndLoad(sceneName, delay));
    }

    IEnumerator WaitAndLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
