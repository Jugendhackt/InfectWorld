using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int Color = Shader.PropertyToID("_Color");
    public GameObject cameraObject;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public float deathYPoint;

    public int jumps;
    private InputMaster _input;

    private Vector3 _mCheckpoint;

    private int _mCurrentJumps;

    private bool _mJump;

    private Vector3 _mVelocity;

    private float _mXRotation;

    private void Awake()
    {
        _mCheckpoint = transform.position;
        _input = new InputMaster();
        _input.Player.Jump.performed += _ => _mJump = true;
        _input.Player.Jump.canceled += _ => _mJump = false;
        _input.Player.Pause.performed += _ => FindObjectOfType<LevelUIScript>().TogglePause();
        _input.Player.Use.performed += _ => GetComponent<NetworkPlayer>().Use();
        var color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        if (GetComponent<PhotonView>().IsMine)
            GetComponent<PhotonView>().RPC("RPC_SendColor", RpcTarget.All,
                GetComponent<PhotonView>().Controller.ActorNumber, new Vector3(color.r, color.g, color.b));
    }


    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    private void OnEnable()
    {
        _input.Player.Enable();
    }

    private void OnDisable()
    {
        _input.Player.Disable();
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
        if (!cc.isGrounded)
            _mVelocity.y += gravity * Time.deltaTime;
    }

    private void Jump()
    {
        var cc = GetComponent<CharacterController>();
        if (cc.isGrounded)
            _mCurrentJumps = 0;
        if (jumps == 0 || !_mJump || !cc.isGrounded && jumps >= 0 && jumps <= _mCurrentJumps ||
            !(_mVelocity.y <= 0)) return;
        _mCurrentJumps++;
        _mVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void GroundCheck()
    {
        var tf = transform;
        if (tf.position.y < deathYPoint)
            tf.position = _mCheckpoint;
    }

    [PunRPC]
    private void RPC_SendColor(int id, Vector3 randomColor)
    {
        if (GetComponent<PhotonView>().Controller.ActorNumber == id)
            GetComponent<Renderer>().material.SetColor(Color, new Color(randomColor.x, randomColor.y, randomColor.z));
    }
}