using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement_script : MonoBehaviour
{
    public float MoveSpeed;
    public float RunSpeed;
    private float _currentMoveSpeed;
    public float JumpPower;
    public float GravityPower;
    private float _currentJumpForce;
    private Rigidbody2D _rb;
    private bool _blockJump;
    private bool _doFall;
    private float _timer;
    private CharacterCollision_script _characterCollision;
    private EffectMananger_script _effectMananger;
    public AudioClip JumpClip;

    void OnEnable()
    {
        _currentJumpForce = -GravityPower;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _characterCollision = GetComponent<CharacterCollision_script>();
        _effectMananger = _characterCollision._gameController.GetComponent<EffectMananger_script>();
        _currentMoveSpeed = MoveSpeed;
        _currentJumpForce = -GravityPower;
    }
    
    void FixedUpdate()
    {
        //Move();
        /*if (Input.GetButtonDown("Sprint"))
        {
            _currentMoveSpeed = RunSpeed;
        }*/
        if (Input.GetButtonUp("Sprint") && !_blockJump)
        {
            _currentMoveSpeed = MoveSpeed;
        }
        //Jump();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float hor = Input.GetAxis("Horizontal");
        float step = (hor * _currentMoveSpeed) * Time.deltaTime;
        float jumpStep = _currentJumpForce * Time.deltaTime;
        Vector2 vel = new Vector2(step, jumpStep);
        //Debug.Log("Move vector: " +vel);
        _rb.velocity = vel;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_blockJump)
        {
            //Debug.Log("Do jump");
            //_currentJumpForce = JumpPower;
            _currentJumpForce = JumpPower;
            _blockJump = true;
            _timer = 0.25f;
            _effectMananger.PlayEffect(JumpClip);
            //_currentMoveSpeed *= 0.5f;
        }
        if (_timer > 0 && !_doFall)
        {
            //Debug.Log("Do Timer");
            /*if (_currentJumpForce < JumpPower)
            {
                _currentJumpForce += JumpPower * Time.deltaTime;
            }
            else
            {
                _currentJumpForce = JumpPower;
            }*/
            
            _timer -= Time.deltaTime;
        }
        if (_timer <= 0 && _blockJump)
        {
            //Debug.Log("Do Fall");
            //Debug.Log("Timer: " + _timer);
            /*if (_currentJumpForce > -GravityPower)
            {
                _currentJumpForce -= GravityPower * Time.deltaTime;
            }
            else
            {
                _currentJumpForce = -GravityPower;
            }*/
            _currentJumpForce -= GravityPower * Time.deltaTime;
            _doFall = true;
            _timer = 0;
        }
    }

    public void Landed()
    {
        _blockJump = false;
        _doFall = false;
        _currentMoveSpeed = MoveSpeed;
        _currentJumpForce = 0;
    }

    public void NoGround()
    {
        if (!_blockJump)
        {
            Debug.Log("No Ground");
            _doFall = true;
            _blockJump = true;
            _timer = 0;
        }
    }
}
