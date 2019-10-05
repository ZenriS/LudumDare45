using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager_script : MonoBehaviour
{
    private UIManager_script _uiManager;
    public int TotalScore;

    public void IncreaseScore(int a)
    {
        TotalScore += a;
    }

    public void DecreaseScore(int a)
    {
        TotalScore -= a;
    }

    public int GetScore()
    {
        return TotalScore;
    }
}
