using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Movement _move;

    private Vector2 _input;

    private readonly float _acceleration = Constants.CarPhysics.acceleration;
    private readonly float _boostAcceleration = Constants.CarPhysics.boostAcceleration;
    private readonly float _driftScale = Constants.CarPhysics.driftScale;
    private readonly float _turnSpeed = Constants.CarPhysics.turnSpeed;
    private readonly float _boostTurnSpeed = Constants.CarPhysics.boostTurnSpeed;
    private readonly float _maxSpeed = Constants.CarPhysics.maxSpeed;
    private readonly float _boostMaxSpeed = Constants.CarPhysics.boostMaxSpeed;

    private GameObject _leftTurn;
    private GameObject _rightTurn;
    private GameObject _flames;

    private void Awake()
    {
        _move = new Movement(gameObject, _driftScale , _acceleration, _turnSpeed, _maxSpeed);
        _leftTurn = transform.GetChild(0).gameObject;
        _rightTurn = transform.GetChild(1).gameObject;
        _flames = transform.GetChild(2).gameObject;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Inputs
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (_input.x > 0)
        {
            _rightTurn.SetActive(true);
            _leftTurn.SetActive(false);
        }
        else if (_input.x < 0)
        {
            _rightTurn.SetActive(false);
            _leftTurn.SetActive(true);
        }
        else
        {
            _rightTurn.SetActive(false);
            _leftTurn.SetActive(false);
        }
        
        if (Input.GetKey(KeyCode.Space) && _input.y > 0)
        {
            _move.UpdateProfile(_driftScale, _boostAcceleration, _boostTurnSpeed, _boostMaxSpeed);
            _flames.SetActive(true);
        }
        else
        {
            _move.UpdateProfile(_driftScale , _acceleration, _turnSpeed, _maxSpeed);
            _flames.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        _move.Update(_input.y, _input.x);
    }

}
