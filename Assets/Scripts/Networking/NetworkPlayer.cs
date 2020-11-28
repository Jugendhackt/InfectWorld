using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkPlayer : MonoBehaviourPun
{
    public GameObject camera;
    private PlayerState _playerState;
    private void Start()
    {
        _playerState = PlayerState.life;
        if (photonView.IsMine)
        {
            GetComponent<PlayerMovement>().enabled = true;
            camera.SetActive(true);
        }


        if (!PhotonNetwork.IsMasterClient || !photonView.IsMine) return;
        var randomPlayer = Random.Range(0, PhotonNetwork.PlayerList.Length);
        photonView.RPC("RPC_SelectPatient", PhotonNetwork.CurrentRoom.GetPlayer(randomPlayer));
    }

    [PunRPC]
    private void RPC_SelectPatient()
    {
        _playerState = PlayerState.patient;
        FindObjectOfType<LevelUIScript>().ChangePatientView();
        Debug.Log($"Du bist {_playerState}!");
    }
}