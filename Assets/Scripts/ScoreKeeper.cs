using System;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    int score;

    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        int instanceCount = FindObjectsByType<ScoreKeeper>(FindObjectsSortMode.None).Length;

        if (instanceCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int value)
    {
        score += value;
        Mathf.Clamp(score, 0, int.MaxValue);
    }

    public void ResetScore()
    {
        score = 0;
    }
}
