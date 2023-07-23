using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerMovement
{
    private float _speed;
    private float _originalSpeed;
    private float _dashSpeed=35;
    private Transform _cameraReferenceTransform;
    private Transform _transform;
    private Rigidbody _rigidBody;
    private Vector3 _direction;
    private const int _constZero = 0;
    public Action<float, float> moveDirection;
    public Action<bool> isMoving;
    private bool _dashing;
    private float _dashCurrentTime;
    private float _dashMaxTime=0.3f;
    private BaseModel _model;
    private bool _xYMovement;
    private JumpingFoot _jumpingFoot;
    public PlayerMovement SetSpeed(float speed)
    {
        _speed = speed;
        _originalSpeed = _speed;
        return this;
    }
    public PlayerMovement SetModel(BaseModel model)
    {
        _model = model;
        return this;
    }
    public PlayerMovement SetTransform(Transform transform)
    {
        _transform = transform;
        return this;
    }
    public PlayerMovement SetCameraTransform(Transform transform)
    {
        _cameraReferenceTransform = transform;
        return this;
    }
    public PlayerMovement SetRigidBody(Rigidbody rigidBody)
    {
        _rigidBody = rigidBody;
        return this;
    }
    public PlayerMovement SetDashSpeed(float speed)
    {
        _dashSpeed = speed;
        return this;
    }
    public PlayerMovement SetDashTime(float time)
    {
        _dashMaxTime = time;
        return this;
    }
    public PlayerMovement SetJumpingFoot(JumpingFoot jumpingFoot)
    {
        _jumpingFoot = jumpingFoot;
        return this;
    }
    public void UseXYMovment(bool value)
    {
        _xYMovement = value;
    }
    public float GetYVelocity()
    {
        return _yValue;
    }
    public void Move(float x, float z)
    {
        if (_dashing) 
        {
            Dashing();
            return;
        }
        if (!_xYMovement)
        {
            BasicMovment(x, z);
        }
        else
        {
            XYMovement(x, z);
        }
    }
    private void BasicMovment(float x, float z)
    {
        _direction.y = _constZero;
        _direction.x = -_cameraReferenceTransform.right.x * x + - _cameraReferenceTransform.forward.x * z;
        _direction.z = -_cameraReferenceTransform.forward.z * z + -_cameraReferenceTransform.right.z * x;
        _direction.Normalize();
        _direction.y = AplingY();
        _direction *= _speed;
        CheckDirection();
        _direction.y = _rigidBody.velocity.y;
        /*if (Vector3.Angle(_direction, Vector3.up) > 60)
        {
            _direction *= -1;
        }*/
        _rigidBody.velocity = _direction;
        _rigidBody.AddForce(Vector3.up * AplingY() * Time.deltaTime);
        MoveDirectionInvoke(_direction.x, _direction.z);
        isMoving?.Invoke(true);
    }
    public void AirMovement(float x, float z)
    {
        _direction.y = _constZero;
        _direction.x = -_cameraReferenceTransform.right.x * x + -_cameraReferenceTransform.forward.x * z;
        _direction.z = -_cameraReferenceTransform.forward.z * z + -_cameraReferenceTransform.right.z * x;
        _direction.Normalize();
        _direction.y = AplingY();
        _direction *= _speed;
        CheckAirDirection();
        _direction.y = _rigidBody.velocity.y;
        _rigidBody.velocity = _direction;
        _rigidBody.AddForce(Vector3.up * AplingY() * Time.deltaTime);
        MoveDirectionInvoke(_direction.x, _direction.z);
        isMoving?.Invoke(true);
        if (_direction.y == 0)
        {
            _jumpingFoot.Grounding();
        }
    }
    public void XYMovement(float x, float z)
    {

        _direction = _transform.right * x;
        _direction.y = _transform.up.y * z;
        _direction.Normalize();
        _direction *= _speed;

        _rigidBody.velocity = _direction;
        MoveDirectionInvoke(_direction.x, _direction.y);
        isMoving?.Invoke(true);
    }
    Vector3 crossDirection;
    public void CheckDirection()
    {
        //esto anda hay que refinarlo un poco, pero después
        RaycastHit hit;
        float radiusThing = _model._capsuleCollider.radius * Mathf.PI+1;
        Vector3 dir = _direction;
        dir.y = 0;
        if(Physics.Raycast(_model.transform.position+Vector3.up*2f,
            dir, out hit, _direction.magnitude,  _model.layerToCheckCanMove))
        {
            Vector3 reflectDir = Vector3.Reflect(_direction.normalized, hit.normal.normalized);
            int angle = (int)Vector3.SignedAngle(_direction, reflectDir, Vector3.up);
            Vector3 newDir = Quaternion.AngleAxis(angle, Vector3.up) * _direction;
            newDir.Normalize();
            newDir *= _speed;
            if (Vector3.Distance(_model.transform.position, hit.point) <= radiusThing)
            {
                _direction.x = newDir.x;
                _direction.z = newDir.z;
            }
        }
    }
    public void CheckAirDirection()
    {
        //esto anda hay que refinarlo un poco, pero después
        RaycastHit hit;
        float radiusThing = _model._capsuleCollider.radius * Mathf.PI + 1;
        Vector3 dir = _direction;
        dir.y = 0;
        if (Physics.Raycast(_model.transform.position + Vector3.up * 2f,
            dir, out hit, _direction.magnitude, _model.layerToCheckCanMove))
        {
            if (Vector3.Distance(_model.transform.position, hit.point) <= radiusThing)
            {
                _direction.x = 0;
                _direction.z = 0;
            }
        }
    }
    public void MoveDirectionInvoke(float x, float z)
    {
        moveDirection?.Invoke(_direction.x, _direction.z);
    }
    public void DontMoving()
    {
        StopRunning();
        MoveDirectionInvoke(_constZero, _constZero);
        _direction.x = _constZero;
        _direction.z = _constZero;
        if (_xYMovement)
        {
            _direction.y = 0;
        }
        else
        {
            _direction.y = _rigidBody.velocity.y;
        }
        _rigidBody.velocity = _direction;
        isMoving?.Invoke(false);
    }
    public void Dashing()
    {
        _direction.y = AplingY();
        _rigidBody.velocity = _direction;
        MoveDirectionInvoke(_direction.x, _direction.z);
        _dashCurrentTime += Time.deltaTime;
        if (_dashCurrentTime >= _dashMaxTime)
        {
            _dashCurrentTime = _constZero;
            _speed = _originalSpeed;
            _dashing = false;
        }
    }
    private float _yValue;
    private float AplingY()
    {
        RaycastHit hit;
        if (Physics.Raycast(_transform.position + Vector3.up, -Vector3.up, out hit, 1.1f, _model.layerToCheckCanMove))
        {
            _yValue = hit.normal.y;
            return hit.normal.y;
        }
        _yValue = 0;
        return _rigidBody.velocity.y;
    }
    public void StartRunning()
    {
        if (!_jumpingFoot._isGrounded)
        {
            return;
        }
        _speed = _dashSpeed;
    }
    public void StopRunning()
    {
        _speed = _originalSpeed;
    }
    public void Dash(float x, float z)
    {
        if(_dashing)
        {
            return;
        }
        _dashing = true;
        _speed = _dashSpeed;
        _direction.y = AplingY();
        _direction.x = -_cameraReferenceTransform.right.x * x + -_cameraReferenceTransform.forward.x * z;
        _direction.z = -_cameraReferenceTransform.forward.z * z + -_cameraReferenceTransform.right.z * x;
        _direction.Normalize();
        _direction *= _speed;
        CheckDirection();
    }
}
