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

    void Start()
    {
        _uiManager = GetComponent<UIManager_script>();
        Player.gameObject.SetActive(false);
        Player.Config(this);
    }

    public void PlacementDone()
    {
        _uiManager.TogglePlacementUI(false);
        _spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;
        Player.transform.localPosition = _spawnPoint.localPosition;
        Player.gameObject.SetActive(true);
    }

    public void CheckGameOver(bool b)
    {
        Player.gameObject.SetActive(false);
        _uiManager.GameOverIU(b);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
