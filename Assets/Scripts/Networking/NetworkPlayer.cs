using System;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
    {
        private void Start()
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                GetComponent<PlayerMovement>().enabled = true;
                GetComponentInChildren<Camera>().gameObject.SetActive(true);
            }
        }
    }