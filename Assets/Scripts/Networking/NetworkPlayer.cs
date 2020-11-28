using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkPlayer : MonoBehaviourPun
{
    public GameObject camera;
    public PlayerState playerState = PlayerState.life;
    private void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<PlayerMovement>().enabled = true;
            camera.SetActive(true);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SelectPatient", RpcTarget.All, Random.Range(0, PhotonNetwork.PlayerList.Length));
        }
    }

    [PunRPC]
    private void SelectPatient(int patient)
    {
        if (photonView.ViewID == patient) playerState = PlayerState.patient;
        Debug.Log($"{photonView.name} ist {playerState}!");
    }
}