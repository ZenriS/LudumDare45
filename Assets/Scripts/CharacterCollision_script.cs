using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision_script : MonoBehaviour
{
    private CharacterMovement_script _characterMovement;
    private GameController_script _gameController;

    void Start()
    {
        _characterMovement = GetComponent<CharacterMovement_script>();
    }

    public void Config(GameController_script gc)
    {
        _gameController = gc;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            Debug.Log("Landed");
            _characterMovement.Landed();
        }
        else if (col.tag == "Finish")
        {
            Debug.Log("Finish");
            _gameController.CheckGameOver(true);
        }
        else if (col.tag == "Void")
        {
            _gameController.CheckGameOver(false);
        }
    }
}
