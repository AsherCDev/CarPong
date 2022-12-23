
using UnityEngine;

public class Movement
{
    private Rigidbody2D _rb;
    private Transform _transform;

    private float _driftScale;
    private float _acceleration;
    private float _turnSpeed;
    private float _maxSpeed;

    private float _rotationAngle;
    private float _velocityVsUp;

    public Movement(GameObject obj, float driftScale, float acceleration, float turnSpeed, float maxSpeed)
    {
        _rb = obj.GetComponent<Rigidbody2D>();
        _transform = obj.transform;
        _rotationAngle = _transform.eulerAngles.z;
        _driftScale = driftScale;
        _acceleration = acceleration;
        _turnSpeed = turnSpeed;
        _maxSpeed = maxSpeed;
    }

    public void UpdateProfile(float driftScale, float acceleration, float turnSpeed, float maxSpeed)
    {
        _driftScale = driftScale;
        _acceleration = acceleration;
        _turnSpeed = turnSpeed;
        _maxSpeed = maxSpeed;
    }

    public void Update(float accel, float turn)
    {
        ApplyAcceleration(accel);
        
        KillDrift();
        
        ApplySteering(turn);
    }

    private void ApplyAcceleration(float accel)
    {
        // Limits velocities
        _velocityVsUp = Vector2.Dot(_transform.up, _rb.velocity);

        if (_velocityVsUp > _maxSpeed + 0.5f && accel > 0)
        {
            _rb.drag = Mathf.Lerp(_rb.drag, 6.0f, Time.fixedDeltaTime * 6);
        }

        if (_velocityVsUp > _maxSpeed && accel > 0) return;
        
        if (_velocityVsUp < -_maxSpeed * 0.5f && accel < 0) return;

        if (_rb.velocity.sqrMagnitude > _maxSpeed * _maxSpeed && accel > 0) return;

        // Adds drag when there is no throttle
        if (accel == 0)
        {
            _rb.drag = Mathf.Lerp(_rb.drag, 3.0f, Time.fixedDeltaTime * 4);
        }
        else
        {
            _rb.drag = 0;
        }
        
        // Throttle
        Vector2 throttleDirection = _transform.up * accel * _acceleration;
        _rb.AddForce(throttleDirection, ForceMode2D.Force);
    }

    private void ApplySteering(float turn)
    {
        // Steering
        float minSpeedToTurn = _rb.velocity.magnitude / 8;
        _velocityVsUp = Vector2.Dot(_transform.up, _rb.velocity);
        if (_velocityVsUp < 0)
        {
            turn *= -1;
        }
        minSpeedToTurn = Mathf.Clamp01(minSpeedToTurn);
        
        _rotationAngle -= turn * _turnSpeed * minSpeedToTurn;
        _rb.MoveRotation(_rotationAngle);
    }

    private void KillDrift()
    {
        Vector2 forwardVelocity = _transform.up * Vector2.Dot(_rb.velocity, _transform.up);
        Vector2 rightVelocity = _transform.right * Vector2.Dot(_rb.velocity, _transform.right);

        _rb.velocity = forwardVelocity + rightVelocity * _driftScale;
    }
}
