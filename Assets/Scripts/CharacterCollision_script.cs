using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollision_script : MonoBehaviour
{
    private CharacterMovement_script _characterMovement;
    [HideInInspector]public GameController_script _gameController;
    private EffectMananger_script _effectMananger;
    public AudioClip DeathClip;
    public AudioClip WinClip;

    void Start()
    {
        _characterMovement = GetComponent<CharacterMovement_script>();
        _effectMananger = _gameController.GetComponent<EffectMananger_script>();
    }

    public void Config(GameController_script gc)
    {
        _gameController = gc;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            //Debug.Log("Landed");
            _characterMovement.Landed();
        }
        else if (col.tag == "Finish")
        {
            //Debug.Log("Finish");
            _effectMananger.PlayEffect(WinClip);
            _gameController.CheckGameOver(true);
        }
        else if (col.transform.tag == "Void")
        {
            _gameController.CheckGameOver(false);
            _effectMananger.PlayEffect(DeathClip);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            _characterMovement.NoGround();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "Void")
        {
            _gameController.CheckGameOver(false);
            _effectMananger.PlayEffect(DeathClip);
        }
    }
}
