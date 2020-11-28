using System;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    public GameObject camera;
        private void Start()
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PlayerMovement>().enabled = true;
                camera.SetActive(true);
            }
        }
    }