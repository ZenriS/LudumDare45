using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController_script : MonoBehaviour
{
    private UIManager_script _uiManager;
    private bool _gameIsOver;
    public CharacterCollision_script Player;
    private Transform _spawnPoint;
    public GameObject[] LevelPresets;
    private int _levelCount;
    private ObjectPlacerMananger_script _objectPlacerMananger;
    private ScoreManager_script _scoreManager;
    private EffectMananger_script _effectMananger;
    public AudioClip ButtonClip;

    void Awake()
    {
        _uiManager = GetComponent<UIManager_script>();
        _objectPlacerMananger = GetComponent<ObjectPlacerMananger_script>();
        _scoreManager = GetComponent<ScoreManager_script>();
        _effectMananger = GetComponent<EffectMananger_script>();
        Player.Config(this);
        Player.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            RestartGame();
        }
        else if (Input.GetButtonDown("Retry"))
        {
            StartPlacement();
        }
    }

    public void PlacementDone()
    {
        _spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        if (_spawnPoint == null)
        {
            return;
        }
        _uiManager.TogglePlacementUI(false);
        Player.transform.localPosition = _spawnPoint.localPosition;
        Player.gameObject.SetActive(true);
        _effectMananger.PlayEffect(ButtonClip);
        _objectPlacerMananger.ClearSelectedObject();
    }

    public void StartPlacement()
    {
        _uiManager.TogglePlacementUI(true);
        _uiManager.LevelOverUI(false, _levelCount);
        Player.gameObject.SetActive(false);
    }

    public void CheckGameOver(bool b)
    {
        Player.gameObject.SetActive(false);
        if (!b)
        {
            StartPlacement();
        }
        else
        {
            _uiManager.LevelOverUI(b, _levelCount);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNewLevel()
    {
        _levelCount++;
        if (_levelCount > LevelPresets.Length)
        {
            Debug.Log("No more levels");
            _uiManager.GameCompleteUI();
            return;
        }
        _gameIsOver = false;
        _uiManager.LevelOverUI(false,_levelCount);
        foreach (GameObject go in LevelPresets)
        {
            go.SetActive(false);
        }
        LevelPresets[_levelCount - 1].SetActive(true);
        Transform goal = LevelPresets[_levelCount - 1].transform.GetChild(0);
        _objectPlacerMananger.SetGoal(goal);
        _scoreManager.NewLevel();
        _uiManager.UpdateScoreUI(0);
        _objectPlacerMananger.RemoveAllObject();
        StartPlacement();
    }

    public int GetLevelCount()
    {
        return _levelCount;
    }
}
