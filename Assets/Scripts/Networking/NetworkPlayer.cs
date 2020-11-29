using System;
using System.Collections;
using Bolt;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;
using UnityTemplateProjects;
using Random = UnityEngine.Random;

public class NetworkPlayer : MonoBehaviourPun
{
    public GameObject playerCamera;
    private PlayerState _playerState;
    public int infectRange = 3;
    public int infectTime = 10;
    private LevelUIScript _ui;
    private bool _movement;

    public bool movement
    {
        get => _movement;
        set
        {
            _movement = value;
            GetComponent<PlayerMovement>().enabled = value;
        }
    }

    private void Start()
    {
        _playerState = PlayerState.life;
        if (photonView.IsMine)
        {
            playerCamera.SetActive(true);
            movement = true;
            _ui = FindObjectOfType<LevelUIScript>();
            _ui.currentPlayer = this;
        }


        if (!PhotonNetwork.IsMasterClient || !photonView.IsMine) return;
        var randomPlayer = Random.Range(0, PhotonNetwork.PlayerList.Length);
        photonView.RPC("RPC_SelectPatient", PhotonNetwork.CurrentRoom.GetPlayer(randomPlayer));
    }

    private GameObject _currentNearestPlayer;
    private float _currentTimeInfected;
    

    private void FixedUpdate()
    {
        if (_currentNearestPlayer == null || Vector3.Distance(transform.position, _currentNearestPlayer.transform.position) > infectRange)
        {
            GameObject bestTarget = null;
            var closestDistanceSqr = Mathf.Infinity;
            var currentPosition = transform.position;
            // Keine Patienten!
            foreach(var potentialTarget in GameObject.FindGameObjectsWithTag("player"))
            {
                var directionToTarget = potentialTarget.transform.position - currentPosition;
                var dSqrToTarget = directionToTarget.sqrMagnitude;
                if (!(dSqrToTarget < closestDistanceSqr)) continue;
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }

            _currentNearestPlayer = bestTarget;
            _currentTimeInfected = 0;
        }
        else
            _currentTimeInfected += Time.fixedDeltaTime;

        var percent = _currentTimeInfected / infectTime;
        _ui.ChangeInfectedTime(percent);
        if (!(percent > 1f)) return;
        // Infiziere andere!
        _currentTimeInfected = 0;
        _currentNearestPlayer = null;
    }

    [PunRPC]
    private void RPC_SelectPatient()
    {
        _playerState = PlayerState.patient;
        FindObjectOfType<LevelUIScript>().ChangePatientView();
        Debug.Log($"Du bist {_playerState}!");
    }
}