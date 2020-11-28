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

        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("RPC_SelectPatient", RpcTarget.All, Random.Range(0, PhotonNetwork.PlayerList.Length));
    }

    [PunRPC]
    private void RPC_SelectPatient(int patient)
    {
        if (photonView.ViewID == patient) _playerState = PlayerState.patient;
        Debug.Log($"{photonView.name} ist {_playerState}!");
    }
}