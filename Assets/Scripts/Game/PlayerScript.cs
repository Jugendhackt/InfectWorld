using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    private InputMaster _input;
    public GameObject cameraObject;

    private float _mXRotation;

    public float speed = 12f;
    public float gravity = -9.81f;

    private Vector3 _mVelocity;
    public float jumpHeight = 3;

    private Vector3 _mCheckpoint;

    public float deathYPoint;

    private bool _mJump;

    public int jumps;

    private int _mCurrentJumps = 0;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
    }

    private void Awake()
    {
        _mCheckpoint = transform.position;
        _input = new InputMaster();
        _input.Player.Jump.performed += _ => _mJump = true;
        _input.Player.Jump.canceled += _ => _mJump = false;
    }


    private void Update()
    {
        var cc = GetComponent<CharacterController>();
        _mVelocity = cc.velocity;
        Jump();
        Move();
        Look();
        cc.Move(_mVelocity * Time.deltaTime);
        GroundCheck();
    }
    private void Look()
    {
        var vector = _input.Player.Look.ReadValue<Vector2>();
        transform.Rotate(Vector3.up * vector.x);
        _mXRotation -= vector.y;
        _mXRotation = Mathf.Clamp(_mXRotation, -90f, 90f);
        cameraObject.transform.localRotation = Quaternion.Euler(_mXRotation, 0f, 0f);
    }


    private void Move()
    {
        var cc = GetComponent<CharacterController>();
        var tf = transform;
        var vector = _input.Player.Movement.ReadValue<Vector2>();
        var move = tf.right * vector.x + tf.forward * vector.y;
        cc.Move(move * (speed * Time.deltaTime));
        if(!cc.isGrounded)
            _mVelocity.y += gravity * Time.deltaTime;
    }

    private void Jump()
    {
        var cc = GetComponent<CharacterController>();
        if (cc.isGrounded)
            _mCurrentJumps = 0;
        if (jumps == 0 || (!_mJump || (!cc.isGrounded && jumps >= 0 && jumps <= _mCurrentJumps)) ||
            !(_mVelocity.y <= 0)) return;
        _mCurrentJumps++;
        _mVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void GroundCheck()
    {
        var cc = GetComponent<CharacterController>();
        var tf = transform;
        print(_mCheckpoint);
        if (tf.position.y < deathYPoint)
            tf.position = _mCheckpoint;
    }
}
