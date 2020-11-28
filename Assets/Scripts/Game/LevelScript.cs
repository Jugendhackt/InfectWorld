using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public int range = 4;
    private void Start()
    {
        var position = transform.position;
        var x = Random.Range(position.x - range, position.x + range);
        var y = Random.Range(position.y - range, position.y + range);
        var z = Random.Range(position.z - range, position.z + range);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(x, y, z), Quaternion.identity, 0);
        
        var players = PhotonNetwork.PlayerList;
    }
}