using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager_script : MonoBehaviour
{
    private UIManager_script _uiManager;
    private GameController_script _gameController;
    public int TotalScore;
    public int DistMultiplier;
    private List<int> _levelScores;

    void Awake()
    {
        _uiManager = GetComponent<UIManager_script>();
        _gameController = GetComponent<GameController_script>();
        _levelScores = new List<int>();
        //_uiManager.UpdateScoreUI(0);
    }

    public void IncreaseScore(int a)
    {
        TotalScore += a;
        _uiManager.UpdateScoreUI(TotalScore);
    }

    public void DecreaseScore(int a)
    {
        TotalScore -= a;
        _uiManager.UpdateScoreUI(TotalScore);
    }

    public int GetScore()
    {
        return TotalScore;
    }

    public void CalcBasScore(float d, bool b = true)
    {
        float temp = d * DistMultiplier;
        if (b)
        {
            TotalScore += Mathf.CeilToInt(temp);
        }
        else
        {
            TotalScore -= Mathf.CeilToInt(temp);
        }
        _uiManager.UpdateScoreUI(TotalScore);
        
    }

    public void NewLevel()
    {
        _levelScores.Add(TotalScore);
        TotalScore = 0;
        //_uiManager.UpdateScoreUI(0);
    }

    public int GetLevelHighScore()
    {
        string s = "level:" + _gameController.GetLevelCount();
        int hs = PlayerPrefs.GetInt(s);
        Debug.Log("Current HS:" +hs +" Current Level: " + _gameController.GetLevelCount());
        return hs;
    }

    public void GetFinalScores(out List<int> s,out int total, out int hs)
    {
        s = _levelScores;
        hs = PlayerPrefs.GetInt("FinalHS");
        total = 0;
        foreach (int score in _levelScores)
        {
            total += score;
        }
        if (total > hs)
        {
            PlayerPrefs.SetInt("FinalHS",total);
        }
    }


    public bool CheckHighScore(int level)
    {
        bool b = false;
        string s = "level:" + level;
        int hs = PlayerPrefs.GetInt(s);
        if (TotalScore > hs)
        {
            Debug.Log("New High Score");
            PlayerPrefs.SetInt(s, TotalScore);
            b = true;
        }
        return b;
    }
}
